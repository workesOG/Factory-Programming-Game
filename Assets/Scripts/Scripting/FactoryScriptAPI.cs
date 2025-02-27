using System;
using System.Collections.Generic;
using System.Linq;
using MoonSharp.Interpreter;
using Unity.VisualScripting;
using UnityEngine;

public static class FactoryScriptAPI
{
    public static void Register(Script luaScript)
    {
        Table fsTable = new Table(luaScript);

        fsTable["print"] = (Action<string>)Print;
        fsTable["buyMats"] = (Action<string, int>)BuyMats;
        fsTable["getProcessingTime"] = (Func<string, double>)GetProcessingTime;
        fsTable["finishLog"] = (Func<string, bool>)FinishLog;
        fsTable["getMatAmount"] = (Func<string, int>)GetMatAmount;
        fsTable["sellMat"] = (Action<string, int>)SellMat;

        // New functions:
        fsTable["getPlayerMoney"] = (Func<double>)GetPlayerMoney;
        fsTable["getMatPrice"] = (Func<string, double>)GetMatPrice;

        luaScript.Globals["fs"] = fsTable;

        luaScript.DoString(@"
            fs.sleep = function(secs)
                return coroutine.yield(secs)
            end

            fs.sawLogs = function(name, amount)
                for i = 1, amount do
                    fs.print('Started processing a ' .. name)
                    local waitTime = fs.getProcessingTime(name)
                    if waitTime > 0 then
                        fs.sleep(waitTime)
                        local success = fs.finishLog(name)
                        if success then
                            fs.print('Finished processing a ' .. name)
                        else
                            fs.print('Error processing log ' .. i)
                            break
                        end
                    else
                        break
                    end
                end
                fs.print('Finished processing logs.')
            end
        ");
    }

    public static void Print(string text)
    {
        ConsoleManager.Print(text);
    }

    public static void BuyMats(string name, int amount)
    {
        MaterialSO material = MaterialsManager.Instance.GetMaterial(name);
        if (material == null)
        {
            ConsoleManager.Print($"Could not find material {name}");
            return;
        }

        double totalPrice = material.buyPrice * amount;
        if (!(MoneyManager.Instance.Money >= totalPrice))
        {
            ConsoleManager.Print($"Cannot afford {amount} {name}");
            return;
        }

        MoneyManager.Instance.Money -= totalPrice;
        MaterialsManager.Instance.GetMaterial(name).Add(amount);
    }

    public static double GetProcessingTime(string name)
    {
        MachineSO machine = MachinesManager.Instance.GetMachine("Lumber Mill");
        if (!machine.unlocked)
        {
            ConsoleManager.Print("Access to this function is not unlocked. Buy the correct machine first.");
            return 0;
        }

        MaterialSO material = MaterialsManager.Instance.GetMaterial(name);
        if (material == null)
        {
            ConsoleManager.Print($"Could not find material {name}");
            return 0;
        }

        if (material.amount < 1)
        {
            ConsoleManager.Print($"Not enough {name}");
            return 0;
        }

        List<MaterialSO> materialsWithRecipes = MaterialsManager.Instance.materials
            .Where(x => x.recipe.ingredients.Count > 0).ToList();
        MaterialSO processedMaterial = materialsWithRecipes
            .Find(x => x.recipe.ingredients[0].inputMaterial.materialName == name);
        if (processedMaterial == null)
        {
            ConsoleManager.Print($"Could not find processed material for {name}");
            return 0;
        }

        MaterialRecipe recipe = processedMaterial.recipe;
        return recipe.processingTime;
    }

    public static bool FinishLog(string name)
    {
        MachineSO machine = MachinesManager.Instance.GetMachine("Lumber Mill");
        if (!machine.unlocked)
        {
            ConsoleManager.Print("Access to this function is not unlocked. Buy the correct machine first.");
            return false;
        }

        MaterialSO material = MaterialsManager.Instance.GetMaterial(name);
        if (material == null)
        {
            ConsoleManager.Print($"Could not find material {name}");
            return false;
        }

        if (material.amount < 1)
        {
            ConsoleManager.Print($"Not enough {name}");
            return false;
        }

        List<MaterialSO> materialsWithRecipes = MaterialsManager.Instance.materials
            .Where(x => x.recipe.ingredients.Count > 0).ToList();
        MaterialSO processedMaterial = materialsWithRecipes
            .Find(x => x.recipe.ingredients[0].inputMaterial.materialName == name);
        if (processedMaterial == null)
        {
            ConsoleManager.Print($"Could not find processed material for {name}");
            return false;
        }

        MaterialRecipe recipe = processedMaterial.recipe;
        processedMaterial.Add(recipe.outputAmount);
        material.Remove(1);
        return true;
    }

    public static int GetMatAmount(string name)
    {
        MaterialSO material = MaterialsManager.Instance.GetMaterial(name);
        if (material == null)
        {
            ConsoleManager.Print($"Could not find material {name}");
            return 0;
        }
        return material.amount;
    }

    public static void SellMat(string name, int amount)
    {
        MaterialSO material = MaterialsManager.Instance.GetMaterial(name);
        if (material == null)
        {
            ConsoleManager.Print($"Could not find material {name}");
            return;
        }

        if (material.amount < amount)
        {
            ConsoleManager.Print($"Not enough {name} to sell.");
            return;
        }

        double revenue = material.sellPrice * amount;
        material.Remove(amount);
        MoneyManager.Instance.Money += revenue;
        ConsoleManager.Print($"Sold {amount} {name} for {revenue} dollars.");
    }

    // New function: Returns the player's current money.
    public static double GetPlayerMoney()
    {
        return MoneyManager.Instance.Money;
    }

    // New function: Returns the price of a given material (using its buyPrice).
    public static double GetMatPrice(string name)
    {
        MaterialSO material = MaterialsManager.Instance.GetMaterial(name);
        if (material == null)
        {
            ConsoleManager.Print($"Could not find material {name}");
            return 0;
        }
        return material.buyPrice;
    }
}

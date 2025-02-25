using System;
using MoonSharp.Interpreter;
using UnityEngine;

public static class FactoryScriptAPI
{
    public static void Register(Script luaScript)
    {
        Table fsTable = new Table(luaScript);

        fsTable["print"] = (Action<string>)Print;
        fsTable["buyMats"] = (Action<string, int>)BuyMats;

        luaScript.Globals["fs"] = fsTable;

        luaScript.DoString(@"
            fs.sleep = function(secs)
                return coroutine.yield(secs)
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
            ConsoleManager.Print($"Cound not find material {name}");

        double totalPrice = material.buyPrice * amount;
        if (!(MoneyManager.Instance.Money >= totalPrice))
            ConsoleManager.Print($"Cannot afford {amount} {name}");

        MoneyManager.Instance.Money -= totalPrice;
        MaterialsManager.Instance.GetMaterial(name).Add(amount);
    }
}

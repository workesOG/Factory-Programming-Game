using UnityEngine;
using MoonSharp.Interpreter;
using System;

public class LuaInitializer : MonoBehaviour
{
    void Start()
    {
        // Create a new Lua script environment
        Script luaScript = new Script();
        luaScript.Options.DebugPrint = s => Debug.Log(s);

        // Create a new Lua table to serve as the fs module
        Table fsTable = new Table(luaScript);

        // Assign the fs.print function to the delegate pointing to FactoryScriptAPI.print
        fsTable["print"] = (Action<string>)FactoryScriptAPI.print;

        // Register the fs table in the global namespace
        luaScript.Globals["fs"] = fsTable;

        // Test the function: this should call ConsoleManager.Print and output to the in-game console
        luaScript.DoString("fs.print('Hello from fs.print!')");
    }
}
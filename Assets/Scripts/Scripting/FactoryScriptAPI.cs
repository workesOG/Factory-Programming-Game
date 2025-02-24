using System;
using MoonSharp.Interpreter;

public static class FactoryScriptAPI
{
    // Registers all fs API functions into the provided Lua script
    public static void Register(Script luaScript)
    {
        Table fsTable = new Table(luaScript);

        // Register fs.print as a CLR callback
        fsTable["print"] = (Action<string>)print;

        // Expose the table first
        luaScript.Globals["fs"] = fsTable;

        // Override fs.sleep with a Lua implementation to allow yielding.
        // This defines fs.sleep as a Lua function that yields the provided seconds.
        luaScript.DoString(@"
            fs.sleep = function(secs)
                return coroutine.yield(secs)
            end
        ");
    }

    // fs.print implementation: simply sends text to the in-game console.
    public static void print(string text)
    {
        ConsoleManager.Print(text);
    }
}

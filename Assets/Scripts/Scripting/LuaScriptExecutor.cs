using System;
using MoonSharp.Interpreter;

public class LuaScriptExecutor
{
    private static LuaScriptExecutor _instance;
    public static LuaScriptExecutor Instance
    {
        get
        {
            if (_instance == null)
                _instance = new LuaScriptExecutor();
            return _instance;
        }
    }

    // Executes a Lua script synchronously and returns an error message if one occurs.
    public string ExecuteScript(string scriptContent)
    {
        try
        {
            // Create a new Lua script environment and register the fs API.
            Script luaScript = new Script();
            FactoryScriptAPI.Register(luaScript);

            // Execute the script synchronously.
            luaScript.DoString(scriptContent);
            return null; // No error occurred.
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}

using UnityEngine;
using MoonSharp.Interpreter;

public class LuaInitializer : MonoBehaviour
{
    void Start()
    {
        // Initialize the Lua script
        string luaScript = @"
            function greet()
                print('Hello from Lua!')
            end

            greet()
        ";

        // Create a new MoonSharp script instance
        Script script = new Script();

        // Redirect Lua's print function to Unity's Debug.Log
        script.Options.DebugPrint = s => Debug.Log(s);

        try
        {
            // Execute the Lua script
            script.DoString(luaScript);
        }
        catch (ScriptRuntimeException ex)
        {
            Debug.LogError($"Lua error: {ex.Message}");
        }
    }
}
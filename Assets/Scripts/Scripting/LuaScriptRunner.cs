using MoonSharp.Interpreter;
using UnityEngine;

public class LuaScriptRunner : MonoBehaviour
{
    private static LuaScriptRunner _instance;

    public static LuaScriptRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LuaScriptRunner>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Reference to our central manager (make sure this is set in your scene)
    //public LuaCoroutineManager coroutineManager;

    /// <summary>
    /// Runs a Lua script as a coroutine.
    /// </summary>
    /// <param name="scriptContent">The Lua script source code.</param>
    public void RunScript(string scriptContent)
    {
        // Create a new Lua environment
        Script luaScript = new Script();

        // Register our FactoryScript API (which includes fs.print and fs.sleep)
        FactoryScriptAPI.Register(luaScript);

        // Load the script as a function
        DynValue function = luaScript.LoadString(scriptContent);

        // Create a coroutine from the function
        DynValue coroutineDynValue = luaScript.CreateCoroutine(function);
        MoonSharp.Interpreter.Coroutine luaCoroutine = coroutineDynValue.Coroutine;

        // Register the coroutine with the central manager for scheduling/resuming
        LuaCoroutineManager.Instance.RegisterCoroutine(luaCoroutine);
    }
}

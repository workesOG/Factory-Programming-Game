using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

public class LuaCoroutineManager : MonoBehaviour
{
    private static LuaCoroutineManager _instance;

    public static LuaCoroutineManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LuaCoroutineManager>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Represents a running Lua script coroutine
    public class RunningLuaScript
    {
        public MoonSharp.Interpreter.Coroutine Coroutine;
        public float ResumeTime; // Time when this coroutine should be resumed
    }

    // List of active Lua script coroutines
    private List<RunningLuaScript> runningScripts = new List<RunningLuaScript>();

    /// <summary>
    /// Registers a new Lua script coroutine for management.
    /// </summary>
    /// <param name="luaCoroutine">The MoonSharp coroutine to register.</param>
    public void RegisterCoroutine(MoonSharp.Interpreter.Coroutine luaCoroutine)
    {
        RunningLuaScript newScript = new RunningLuaScript
        {
            Coroutine = luaCoroutine,
            ResumeTime = Time.time  // Start immediately
        };
        runningScripts.Add(newScript);
    }

    void Update()
    {
        float currentTime = Time.time;
        List<RunningLuaScript> finishedScripts = new List<RunningLuaScript>();

        foreach (var script in runningScripts)
        {
            if (currentTime >= script.ResumeTime)
            {
                // Resume the coroutine
                DynValue result = script.Coroutine.Resume();
                Debug.Log($"Resuming coroutine; result type: {result.Type}");

                double waitTime = 0;
                bool yielded = false;

                // If the result is a number, that’s our wait time.
                if (result.Type == DataType.Number)
                {
                    waitTime = result.Number;
                    yielded = true;
                }
                // Otherwise, if it’s a YieldRequest, extract the wait time.
                else if (result.Type == DataType.YieldRequest &&
                         result.YieldRequest != null &&
                         result.YieldRequest.ReturnValues != null &&
                         result.YieldRequest.ReturnValues.Length > 0 &&
                         result.YieldRequest.ReturnValues[0].Type == DataType.Number)
                {
                    waitTime = result.YieldRequest.ReturnValues[0].Number;
                    yielded = true;
                }

                if (yielded)
                {
                    Debug.Log($"Coroutine yielded; waiting for {waitTime} seconds.");
                    script.ResumeTime = currentTime + (float)waitTime;
                }
                else
                {
                    Debug.Log("Coroutine finished execution.");
                    finishedScripts.Add(script);
                }
            }
        }

        // Remove finished scripts
        foreach (var script in finishedScripts)
        {
            runningScripts.Remove(script);
        }
    }


}

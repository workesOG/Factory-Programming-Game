using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ConsoleManager : MonoBehaviour
{
    private TMP_InputField inputField;
    private List<GameObject> lineGOs = new();
    private Transform consoleWindow;
    private GameObject consoleLinePrefab;

    public List<Command> commands = new();
    public List<string> lines = new();

    public void Initialize(Transform windowTransform)
    {
        consoleWindow = windowTransform.Find("Console Main/Main Scroll/Viewport/Content");
        inputField = windowTransform.Find("Console Input").GetComponent<TMP_InputField>();
        consoleLinePrefab = Resources.Load<GameObject>("Prefabs/Console Line");
    }

    private static ConsoleManager _instance;

    public static ConsoleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ConsoleManager>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        PopulateCommandList();
    }

    public void UpdateLog()
    {
        foreach (GameObject go in lineGOs)
        {
            Destroy(go);
        }
        lineGOs.Clear();

        foreach (string line in lines)
        {
            GameObject lineGO = Instantiate(Instance.consoleLinePrefab, Instance.consoleWindow);
            TMP_Text timeTextComponent = lineGO.transform.Find("Date").GetComponent<TMP_Text>();
            TMP_Text commandTextComponent = lineGO.transform.Find("Command").GetComponent<TMP_Text>();

            string[] commandComponents = line.Split("\t", 2);
            timeTextComponent.text = commandComponents[0];
            commandTextComponent.text = commandComponents[1];
            lineGOs.Add(lineGO);
        }
    }

    public static void ParseCommand(string text)
    {
        string baseCommand = text.Split(" ")[0];
        List<string> parameterList = text.Split(" ").ToList();
        parameterList.RemoveAt(0);
        string[] parameters = parameterList.ToArray();

        List<Command> commands = Instance.commands;
        foreach (Command command in commands)
        {
            if (baseCommand == command.baseCommand)
                ConsoleManager.Print(command.EvaluateAndExecute(parameters));
        }
    }

    public static void Print(string text)
    {
        if (text == string.Empty)
            return;

        if (Instance.lines.Count >= 100)
            Instance.lines.RemoveAt(0);

        string time = DateTime.Now.ToString("HH:mm:ss");
        string lineText = $"[{time}]\t{text}";
        Instance.lines.Add(lineText);

        if (AppManager.Instance.currentAppID == 0)
            Instance.UpdateLog();
    }

    public void PopulateCommandList()
    {
        commands.Add(new Command()
        {
            baseCommand = "help",
            description = "Displays all usable commands (not fs functions, which can also be executed via command line)",
            syntax = "help",
            evaluationFunc = (parameters, syntax) =>
            {
                if (parameters.Length != 0)
                    return $"Invalid syntax, correct syntax is: {syntax}";

                string message = "Commands:";
                foreach (Command command in Instance.commands)
                {
                    message += $"\n\"{command.baseCommand}\"\n\t-Description: {command.description}\n\t-Syntax: {command.syntax}";
                }
                return message;
            }

        });
        commands.Add(new Command()
        {
            baseCommand = "nano",
            description = "Used to make a new empty script. All scripts filenames must end with .lua",
            syntax = "nano <file name>*",
            evaluationFunc = (parameters, syntax) =>
            {
                if (parameters.Length != 1)
                    return $"Invalid syntax, correct syntax is: {syntax}";

                return ScriptManager.Instance.CreateScript(parameters[0]);
            }

        });
        commands.Add(new Command()
        {
            baseCommand = "rm",
            description = "Used to delete a script. Needs full name with \".lua\" extension",
            syntax = "rm <file name>*",
            evaluationFunc = (parameters, syntax) =>
            {
                if (parameters.Length != 1)
                    return $"Invalid syntax, correct syntax is: {syntax}";

                return ScriptManager.Instance.RemoveScript(parameters[0]);
            }

        });
        commands.Add(new Command()
        {
            baseCommand = "mv",
            description = "Used to \"move\" a script, effectively renaming it. Needs full name with \".lua\" extension",
            syntax = "mv <file name>* <new file name>*",
            evaluationFunc = (parameters, syntax) =>
            {
                if (parameters.Length != 2)
                    return $"Invalid syntax, correct syntax is: {syntax}";

                return ScriptManager.Instance.RenameScript(parameters[0], parameters[1]);
            }

        });
        commands.Add(new Command()
        {
            baseCommand = "ls",
            description = "Lists all scripts",
            syntax = "ls",
            evaluationFunc = (parameters, syntax) =>
            {
                if (parameters.Length != 0)
                    return $"Invalid syntax, correct syntax is: {syntax}";

                List<string> scripts = ScriptManager.Instance.GetScripts();
                if (scripts.Count == 0)
                    return "No scripts found";

                string message = "Scripts:";
                foreach (string script in scripts)
                {
                    message += $"\n\t-\"{script}\"";
                }
                return message;
            }

        });
    }
}

public class Command
{
    public string baseCommand;
    public string description;
    public string syntax;
    public Func<string[], string, string> evaluationFunc;

    public string EvaluateAndExecute(string[] parameters)
    {
        return evaluationFunc.Invoke(parameters, syntax);
    }
}

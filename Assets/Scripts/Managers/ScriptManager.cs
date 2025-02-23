using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoonSharp.Interpreter;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScriptManager : MonoBehaviour
{
    private TMP_Dropdown scriptSelectorDropdown;
    private Button saveButton;
    private TMP_InputField codeEditorInputField;

    private string gameDirPath;
    private string scriptDirPath;

    public void Initialize(Transform windowTransform)
    {
        scriptSelectorDropdown = windowTransform.Find("Navigation Bar/Dropdown").GetComponent<TMP_Dropdown>();
        saveButton = windowTransform.Find("Navigation Bar/Button").GetComponent<Button>();
        codeEditorInputField = windowTransform.Find("Code Editor Main/Main Scroll/Viewport/Content/Text").GetComponent<TMP_InputField>();
        scriptSelectorDropdown.onValueChanged.AddListener(LoadScript);
        saveButton.onClick.AddListener(SaveScript);
        LoadScriptsIntoDropdown();
        LoadScript();
    }

    private static ScriptManager _instance;

    public static ScriptManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ScriptManager>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        gameDirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Programming Factory Game");
        scriptDirPath = Path.Combine(gameDirPath, "scripts");

        if (!Directory.Exists(gameDirPath))
            Directory.CreateDirectory(gameDirPath);

        if (!Directory.Exists(scriptDirPath))
            Directory.CreateDirectory(scriptDirPath);
    }

    public void LoadScriptsIntoDropdown()
    {
        scriptSelectorDropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new();
        options.Add(new TMP_Dropdown.OptionData("None"));
        List<string> scripts = GetScripts();
        foreach (string script in scripts)
        {
            options.Add(new TMP_Dropdown.OptionData(script));
        }
        scriptSelectorDropdown.AddOptions(options);
    }

    public void LoadScript(int index = 0)
    {
        string selectedScript = scriptSelectorDropdown.captionText.text;

        if (selectedScript == "None")
        {
            codeEditorInputField.text = string.Empty;
            codeEditorInputField.interactable = false;
            saveButton.interactable = false;
            return;
        }

        string scriptPath = Path.Combine(scriptDirPath, selectedScript);

        if (File.Exists(scriptPath))
        {
            codeEditorInputField.text = File.ReadAllText(scriptPath);
        }
        else
        {
            codeEditorInputField.text = string.Empty;
        }

        codeEditorInputField.interactable = true;
        saveButton.interactable = true;
    }

    public void SaveScript()
    {
        string selectedScript = scriptSelectorDropdown.captionText.text;

        if (selectedScript == "None")
        {
            return; // No script selected, nothing to save
        }

        string scriptPath = Path.Combine(scriptDirPath, selectedScript);

        try
        {
            File.WriteAllText(scriptPath, codeEditorInputField.text);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save script: {e.Message}");
        }
    }

    public string CreateScript(string name)
    {
        List<string> scripts = GetScripts();
        if (scripts.Contains(name))
            return "A script with that name already exists"; //Script already exists

        if (!name.EndsWith(".lua"))
            return "Script name does not end with \".lua\""; //Not a script

        File.Create(Path.Combine(scriptDirPath, name));
        return $"Successfully created script {name}";
    }

    public string RemoveScript(string name)
    {
        string scriptPath = Path.Combine(scriptDirPath, name);

        if (!File.Exists(scriptPath))
            return "No script with that name exists"; // Script does not exist

        File.Delete(scriptPath);
        return $"Successfully removed script {name}";
    }

    public string RenameScript(string name, string newName)
    {
        string scriptPath = Path.Combine(scriptDirPath, name);
        string newScriptPath = Path.Combine(scriptDirPath, newName);

        if (!File.Exists(scriptPath))
            return "No script with that name exists"; // Script does not exist

        if (File.Exists(newScriptPath))
            return "A script with the new name already exists"; // Prevent overwriting

        if (!newName.EndsWith(".lua"))
            return "New script name does not end with \".lua\""; // Not a valid script name

        File.Move(scriptPath, newScriptPath);
        return $"Successfully renamed script {name} to {newName}";
    }


    public List<string> GetScripts()
    {
        return Directory.GetFiles(scriptDirPath)
    .Select(path => Path.GetFileName(path))
    .ToList();
    }
}
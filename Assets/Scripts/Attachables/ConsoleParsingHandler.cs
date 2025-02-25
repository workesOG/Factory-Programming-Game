using UnityEngine;
using TMPro;
using System;

public class ConsoleParsingHandler : MonoBehaviour
{
    public TMP_InputField inputField;  // The code editor

    void Start()
    {
        inputField.onValueChanged.AddListener(SubmitCommand);
    }

    void SubmitCommand(string text)
    {
        if (text.Contains("\n"))
        {
            string command = text.Replace("\n", "");
            inputField.text = string.Empty;
            ConsoleManager.Print(command);
            ConsoleManager.ParseCommand(command);
        }
    }
}
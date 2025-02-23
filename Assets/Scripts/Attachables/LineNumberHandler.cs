using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;
using Unity.Collections;
using System.IO.Compression;
using System.IO;

public class LineNumberHandler : MonoBehaviour
{
    public TMP_InputField inputField;  // The code editor
    public TextMeshProUGUI lineNumbers; // The line number display

    void Start()
    {
        inputField.onValueChanged.AddListener(UpdateLineNumbers);
        UpdateLineNumbers(inputField.text);
    }

    void UpdateLineNumbers(string text)
    {
        int lineCount = text.Split('\n').Length;
        string lineText = "";
        for (int i = 1; i <= lineCount; i++)
        {
            lineText += i + "\n";
        }
        lineNumbers.text = lineText;
    }


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;


public class CreateTextFile : EditorWindow
{
    private string windowName = "TextFileCreator";
    private bool isInitialised = false;
    private bool creatingFile = false;

    private string fileName = "";
    private string fileContent = "";
    private string filePath = "";
    private bool overwrite = false;
    private bool append = false;

    private bool filePathChosen = false;

    private FilePathSelector filePathSelector;

    [MenuItem("Window/TextFileCreator")]
    public static void ShowWindow()
    {
        GetWindow<CreateTextFile>("TextFileCreator");
    }

    private void OnEnable()
    {
        Debug.Log($"{windowName} enabled.");
    }

    private void OnDisable()
    {
        SetDefaultValues();
        Debug.Log($"{windowName} disabled.");
    }

    private void SetDefaultValues()
    {
        fileName = "NewTextFile";
        fileContent = "Hello, World!";
        filePath = "";
        overwrite = false;
        filePathChosen = false;
        creatingFile = false;
        append = false;
    }

    private void OnGUI()
    {
        Initialise();

        EditorGUILayout.LabelField(windowName, EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (GUILayout.Button("Reset"))
        {
            Reset();
        }

        EditorGUILayout.Space();

        if (filePathChosen)
        {
            if (GUILayout.Button("Create Text File"))
            {
                creatingFile = true;
            }

            if (creatingFile)
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = "NewTextFile";
                }

                if (string.IsNullOrEmpty(fileContent))
                {
                    fileContent = "Hello, World!";
                }

                EditorGUILayout.BeginVertical("box");

                fileName = EditorGUILayout.TextField(fileName, EditorStyles.textField);

                EditorGUILayout.Space();

                fileContent = EditorGUILayout.TextArea(fileContent, EditorStyles.textArea);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Overwrite?", EditorStyles.label);
                overwrite = EditorGUILayout.Toggle(overwrite, EditorStyles.toggle);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Append?", EditorStyles.label);
                append = EditorGUILayout.Toggle(append, EditorStyles.toggle);

                EditorGUILayout.EndVertical();

                if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(fileContent))
                {
                    if (GUILayout.Button("Confirm"))
                    {
                        CreateFile();
                    }
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("Choose File Path", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            if (filePathSelector == null)
            {
                filePathSelector = CreateInstance<FilePathSelector>();
            }

            if (GUILayout.Button("Choose File Path"))
            {
                filePath = filePathSelector.GetFilePath();
                if (!string.IsNullOrEmpty(filePath))
                {
                    filePathChosen = true;
                }
                else
                {
                    Debug.LogWarning("No file path chosen.");
                }
            }
        }
    }

    private void CreateFile()
    {
        string finalFileName = fileName;
        string newPath = $"{filePath}/{finalFileName}.txt";

        if (!overwrite && !append)
        {
            int fileNumber = 0;
            while (File.Exists(newPath))
            {
                fileNumber++;
                Debug.LogWarning($"File already exists at {newPath}. Attempt: {fileNumber}");
                finalFileName = $"{fileName}({fileNumber})";
                newPath = $"{filePath}/{finalFileName}.txt";

                if (fileNumber > 100)
                {
                    Debug.LogError("Too many files with the same name.");
                    return;
                }
            }
        }
        string tempContent = fileContent;

        if (append) fileContent = $"\n{fileContent}";

        using (StreamWriter writer = new StreamWriter(newPath, append))
        {
            writer.Write(fileContent);
            Debug.Log($"Text file created at {newPath}");
        }

        fileContent = tempContent;
    }

    private void Initialise()
    {
        if (isInitialised) return;

        SetDefaultValues();

        isInitialised = true;
        Debug.Log($"Initialising {windowName}");
    }

    private void Reset()
    {
        isInitialised = false;
        Debug.Log($"Resetting {windowName}");
    }
}

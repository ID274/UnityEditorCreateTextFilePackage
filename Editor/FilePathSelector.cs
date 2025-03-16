using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FilePathSelector : EditorWindow
{
    protected string windowName = "FilePathSelector";
    protected bool isInitialised = false;

    public static void ShowWindow()
    {
        GetWindow<CreateTextFile>("FilePathSelector");
    }

    private void OnEnable()
    {
        Debug.Log($"{windowName} enabled.");
    }

    private void OnDisable()
    {
        Debug.Log($"{windowName} disabled.");
    }

    public string GetFilePath()
    {
        string filePath = EditorUtility.OpenFolderPanel("Choose File Path", "", "");
        return filePath;
    }
}

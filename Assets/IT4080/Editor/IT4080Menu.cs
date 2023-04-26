using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEditor.SceneManagement;


[InitializeOnLoadAttribute]
public static class IT4080Menu
{
    public const string VERSION = "0.1.0";

    [MenuItem("IT4080/About")]
    private static void MnuAbout()
    {
        Debug.Log($"IT4080 Menu Version {VERSION}");
    }
}
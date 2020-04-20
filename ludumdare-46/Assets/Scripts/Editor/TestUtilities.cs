using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using Object = System.Object;

public enum ExtensionTypes { txt, jpg, lua, cs, prefab, dll, csv, json, sql, shader }

public class FileExtensionChanger: EditorWindow
{
    [MenuItem("Tools/Change File Extension")]
    public static void ChangeFileExtension()
    {
        FileExtensionChanger windowTab = (FileExtensionChanger)GetWindow(typeof(FileExtensionChanger), false, "Change File Extension");
        windowTab.Show();
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Change the selected file extension to", EditorStyles.boldLabel);
        foreach (ExtensionTypes extensionType in Enum.GetValues(typeof(ExtensionTypes)))
        {
            if (GUILayout.Button("." + extensionType))
            {
                if (EditorUtility.DisplayDialog("Change object extension?",
                    "Are you sure you want to change " + Selection.activeObject.name + " extension to ." +
                    extensionType + "?", "Change", "Cancel"))
                {
                    ChangingFileExtension(extensionType.ToString());
                }
            }
        }
        GUILayout.FlexibleSpace();
        if(GUILayout.Button("Close tab"))
        {
            this.Close();
        }
        GUIUtility.ExitGUI();
    }

    public void ChangingFileExtension(string type, bool deleteMetaFile = true)
    {
        string file = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (file != null)
        {
            if (deleteMetaFile)
                FileUtil.DeleteFileOrDirectory(file + ".meta");
            File.Move(file, Path.ChangeExtension(file, "." + type));
            AssetDatabase.Refresh();
        }
    }
}
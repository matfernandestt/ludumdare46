using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using Object = System.Object;

public class Utilities
{
    [MenuItem("Tools/Delete PlayerPrefs data #_r", false, 1)]
    private static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Tools/Edit Collider %_e", false, 10)]
    private static void EditCollider()
    {
        var sel = Selection.activeGameObject;
        var col2d = sel.GetComponent<Collider2D>();
        var col3d = sel.GetComponent<Collider>();

        if (!col2d && !col3d) return;

        if (EditMode.editMode == EditMode.SceneViewEditMode.Collider)
        {
            EditMode.ChangeEditMode(EditMode.SceneViewEditMode.None, new Bounds(), null);
        }
        else
        {
            var colliderEditorBase = System.Type.GetType("UnityEditor.ColliderEditorBase,UnityEditor.dll");
            var colliderEditors = Resources.FindObjectsOfTypeAll(colliderEditorBase) as Editor[];

            if (colliderEditors == null || colliderEditors.Length <= 0) return;

            if (col2d)
                EditMode.ChangeEditMode(EditMode.SceneViewEditMode.Collider, col2d.bounds, colliderEditors[0]);
            if (col3d)
                EditMode.ChangeEditMode(EditMode.SceneViewEditMode.Collider, col3d.bounds, colliderEditors[0]);
        }
    }

    [MenuItem("Tools/Reset Transform/Reset Transform &_q", false, 10)]
    public static void ResetTransformTransform()
    {
        GameObject[] selection = Selection.gameObjects;
        if (selection.Length < 1) return;
        Undo.RegisterCompleteObjectUndo(selection, "Zero Position");
        foreach (var gObject in selection)
        {
            gObject.transform.localPosition = Vector3.zero;
            gObject.transform.localEulerAngles = Vector3.zero;
            gObject.transform.localScale = Vector3.one;
        }
    }

    [MenuItem("Tools/Reset Transform/Reset Transform Position &_w", false, 10)]
    public static void ResetTransformPosition()
    {
        GameObject[] selection = Selection.gameObjects;
        if (selection.Length < 1) return;
        Undo.RegisterCompleteObjectUndo(selection, "Zero Position");
        foreach (var gObject in selection)
            gObject.transform.localPosition = Vector3.zero;
    }

    [MenuItem("Tools/Reset Transform/Reset Transform Rotation &_e", false, 10)]
    public static void ResetTransformRotation()
    {
        GameObject[] selection = Selection.gameObjects;
        if (selection.Length < 1) return;
        Undo.RegisterCompleteObjectUndo(selection, "Zero Rotation");
        foreach (var gObject in selection)
            gObject.transform.localEulerAngles = Vector3.zero;
    }

    [MenuItem("Tools/Reset Transform/Reset Transform Scale &_r", false, 10)] // Alt + R
    public static void ResetTransformScale()
    {
        GameObject[] selection = Selection.gameObjects;
        if (selection.Length < 1) return;
        Undo.RegisterCompleteObjectUndo(selection, "Reset Scale");
        foreach (var gObject in selection)
            gObject.transform.localScale = Vector3.one;
    }

    [MenuItem("Tools/Pause Editor &_z", false, 10)] // Alt + Z
    public static void PauseEditor()
    {
        EditorApplication.isPaused = !EditorApplication.isPaused;
    }

    [MenuItem("Tools/Play\\Stop Editor &#_z", false, 10)] // Alt + Shift + Z
    public static void PlayStopEditor()
    {
        EditorApplication.isPlaying = !EditorApplication.isPlaying;
    }

    [MenuItem("Tools/Align to selected &_x", false, 10)] // Alt + X
    public static void AlignToSelected()
    {
        var currentSceneView = SceneView.lastActiveSceneView;
        var selectedObject = Selection.activeGameObject.transform;
        currentSceneView.AlignViewToObject(selectedObject);
        currentSceneView.FrameSelected();
    }

    [MenuItem("Tools/Reset active scene _%q", false, 10)] // Ctrl + T
    public static void ResetCurrentScene()
    {
        if (EditorApplication.isPlaying)
        {
            Time.timeScale = 1;
            EditorSceneManager.LoadScene(EditorSceneManager.GetActiveScene().name);
        }
        else
            Debug.LogWarning("Reset active scene: You can only do that in Play Mode.");
    }

    [MenuItem("Tools/Show in explorer &_c")] // Alt + C
    public static void ShowInExplorer()
    {
        string path = "Assets";
        int assetsLength = path.ToCharArray().Length;

        foreach (var obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }

        var dataPathChar =
            Application.dataPath.ToCharArray(0, Application.dataPath.ToCharArray().Length - assetsLength);
        var dataPath = "";
        foreach (var c in dataPathChar)
        {
            if (c.ToString() == "/")
                dataPath += @"\";
            else
                dataPath += c;
        }

        var assetsPath = "";
        foreach (var c in path.ToCharArray())
        {
            if (c.ToString() == "/")
                assetsPath += @"\";
            else
                assetsPath += c;
        }

        dataPath += assetsPath;
        Debug.Log("Opened " + dataPath);
        System.Diagnostics.Process.Start("explorer.exe", dataPath);
    }
}

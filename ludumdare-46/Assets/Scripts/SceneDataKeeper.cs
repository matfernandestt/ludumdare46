using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneDataKeeper : MonoBehaviour
{
    private static SceneDataKeeper instance;

    private Transform PlayerTransform;
    private Transform CameraTransform;
    
    private Dictionary<string, Vector3> lastPlayerPosition = new Dictionary<string, Vector3>();
    private Dictionary<string, Vector3> lastCameraPosition = new Dictionary<string, Vector3>();

    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        GameEvents.OnLoadSceneController += SceneLoad;
        GameEvents.OnBeforeChangeScene += SceneUnload;
    }

    private void OnDestroy()
    {
        GameEvents.OnLoadSceneController -= SceneLoad;
        GameEvents.OnBeforeChangeScene -= SceneUnload;
    }

    private void SceneLoad(string scene)
    {
        if (!SceneController.IsThereAnInstance()) return;
        PlayerTransform = SceneController.GetPlayerTransform();
        CameraTransform = SceneController.GetCameraTransform();
        if (CheckSceneData(scene))
        {
            PlayerTransform.position = lastPlayerPosition[scene];
            CameraTransform.position = lastCameraPosition[scene];
        }
    }

    private void SceneUnload(string scene)
    {
        if (!SceneController.IsThereAnInstance()) return;
        if (CheckSceneData(scene))
        {
            lastPlayerPosition.Remove(scene);
            lastCameraPosition.Remove(scene);
        }
        lastPlayerPosition.Add(scene, SceneController.GetPlayerTransform().position);
        lastCameraPosition.Add(scene, SceneController.GetCameraTransform().position);
    }

    private bool CheckSceneData(string sceneName)
    {
        return lastPlayerPosition.ContainsKey(sceneName) && lastCameraPosition.ContainsKey(sceneName);
    }

    public static void ResetSavedData()
    {
        instance.lastPlayerPosition.Clear();
        instance.lastCameraPosition.Clear();
    }
}
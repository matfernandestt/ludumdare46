using System;
using UnityEngine;

public enum CameraOverworldMode
{
    SideView,
    TopView
}

public class CameraOverworld : MonoBehaviour
{
    private GameData gameData;

    [SerializeField] private CameraOverworldMode CameraMode;
    [SerializeField] private Transform Target;

    private Camera cam;
    private Vector3 camOffset;

    private void Awake()
    {
        gameData = GameManager.GetGameData();

        cam = GetComponent<Camera>();
        
        switch (CameraMode)
        {
            case CameraOverworldMode.SideView:
                camOffset = gameData.SideViewTargetOffset;
                cam.fieldOfView = gameData.SideViewFov;
                break;
            case CameraOverworldMode.TopView:
                camOffset = gameData.TopViewTargetOffset;
                cam.fieldOfView = gameData.TopViewFov;
                break;
        }
    }

    private void Update()
    {
        switch (CameraMode)
        {
            case CameraOverworldMode.SideView:
                camOffset = Vector3.Lerp(camOffset, gameData.SideViewTargetOffset, Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, Target.position + camOffset, Time.deltaTime * gameData.SideViewFollowSpeed);
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, gameData.SideViewRotation, Time.deltaTime * gameData.SideViewFollowSpeed);
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, gameData.SideViewFov, Time.deltaTime);
                break;
            case CameraOverworldMode.TopView:
                camOffset = Vector3.Lerp(camOffset, gameData.TopViewTargetOffset, Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, Target.position + camOffset, Time.deltaTime * gameData.TopViewFollowSpeed);
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, gameData.TopViewRotation, Time.deltaTime * gameData.TopViewFollowSpeed);
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, gameData.TopViewFov, Time.deltaTime);
                break;
        }
    }
}

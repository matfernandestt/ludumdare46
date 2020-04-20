using System;
using UnityEngine;

public class CameraBattle : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 originalRotation;
    private float originalFieldOfView;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        
        originalPosition = transform.position;
        originalRotation = transform.eulerAngles;
        originalFieldOfView = cam.fieldOfView;
    }
}

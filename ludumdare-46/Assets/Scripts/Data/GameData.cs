using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game Design/Game Data")]
public class GameData : ScriptableObject
{
    [Header("Camera Parameters")]
    public float SideViewFollowSpeed;
    public Vector3 SideViewTargetOffset;
    public Vector3 SideViewRotation;
    public float SideViewFov;
    [Space]
    public float TopViewFollowSpeed;
    public Vector3 TopViewTargetOffset;
    public Vector3 TopViewRotation;
    public float TopViewFov;
    [Space]
    private int a = 0;
}

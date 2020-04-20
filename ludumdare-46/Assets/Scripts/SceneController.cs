using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController instance;

    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private Transform CameraTransform;

    private void Awake()
    {
        instance = this;
        
        GameEvents.OnLoadSceneController?.Invoke(SceneManager.GetActiveScene().name);
    }

    public static Transform GetPlayerTransform()
    {
        return instance.PlayerTransform;
    }

    public static Transform GetCameraTransform()
    {
        return instance.CameraTransform;
    }

    public static bool IsThereAnInstance()
    {
        return instance != null;
    }
}

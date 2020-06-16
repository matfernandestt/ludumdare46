using UnityEngine;

public class BaseMovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform ObjToParent;
    
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.transform.parent = ObjToParent;
            //player.PauseGravity();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.transform.parent = null;
            //player.ResumeGravity();
        }
    }
}

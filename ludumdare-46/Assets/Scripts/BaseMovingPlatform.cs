using UnityEngine;

public class BaseMovingPlatform : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.transform.parent = transform;
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

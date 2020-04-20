using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private GameObject triggerDetector;

    public void StopInputs()
    {
        player.StoppingInputs(true);
        triggerDetector.SetActive(true);
    }
    
    public void RegainInputs()
    {
        player.StoppingInputs(false);
        triggerDetector.SetActive(false);
    }
}

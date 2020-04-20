using UnityEngine;

public class RunAnimation : MonoBehaviour
{
    [SerializeField] private Animator anim;
    
    private bool opened;
    
    private static readonly int Run = Animator.StringToHash("Run");
    private const string TriggerTag = "TriggerAttack";
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TriggerTag) && !opened)
        {
            opened = true;
            anim.SetTrigger(Run);
        }
    }
}

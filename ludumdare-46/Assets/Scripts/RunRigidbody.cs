using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunRigidbody : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private MonoBehaviour mB;
    
    private const string TriggerTag = "TriggerAttack";
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TriggerTag) && rb != null)
        {
            rb.isKinematic = false;
            mB.enabled = true;
        }
    }
}

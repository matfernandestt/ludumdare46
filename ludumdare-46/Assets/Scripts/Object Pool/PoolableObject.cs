using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    [Header("--Optional--")]
    [SerializeField] private AudioSource sfx;

    public void ActivateObject()
    {
        gameObject.SetActive(true);
        if (sfx != null) sfx.Play();
        var rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;
    }

    public void DeactivateObject()
    {
        gameObject.SetActive(false);
        var rb = GetComponent<Rigidbody>();
        if(rb != null) rb.isKinematic = true;
    }
}
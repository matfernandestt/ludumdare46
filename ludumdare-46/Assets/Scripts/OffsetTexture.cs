using UnityEngine;

public class OffsetTexture : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.5f;
    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        float xOffset = Mathf.Sin(Time.time) * scrollSpeed * 2;
        float yOffset = Mathf.Sin(Time.time) * scrollSpeed;
        rend.material.SetTextureOffset("_MainTex", new Vector2(xOffset, yOffset));
    }
}

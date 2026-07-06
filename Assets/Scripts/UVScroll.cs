using UnityEngine;

public class UVScroll : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
    private Material mat;

    void Start()
    {
        mat = GetComponent<Material>();
    }

    void Update()
    {
        Vector2 offset = mat.mainTextureOffset;
        offset.x += scrollSpeed * Time.deltaTime;
        mat.mainTextureOffset = offset;
    }
}

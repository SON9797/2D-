using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UVScroll : MonoBehaviour
{
    public PlayerController player;

    public float scrollSpeed = 0.005f;

    private Material mat;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        mat = meshRenderer.material;

        if (player == null)
        {
            player = Object.FindFirstObjectByType<PlayerController>();
        }
    }

    void Update()
    {
        if (player == null) return;

        if (player.IsMoving)
        {
            float offset = player.HorizontalVelocity * scrollSpeed * Time.deltaTime;
            mat.mainTextureOffset += new Vector2(offset, 0);
        }
    }
}

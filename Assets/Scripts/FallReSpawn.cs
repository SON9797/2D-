using UnityEngine;

public class FallReSpawn : MonoBehaviour
{
    [Header("¼³Á¤")]
    [SerializeField] private GameObject _respawnPos;

    private void OnCollisinter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.position = _respawnPos.transform.position;
        }
    }
}

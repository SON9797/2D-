using UnityEngine;

public class GunnerBullet : MonoBehaviour
{
    [Header("¼³Á¤")]
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GunnerController _gunnerController;

    private float _destroyTime = 4f;

    void Start()
    {
        Destroy(gameObject, _destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_gunnerController != null && _gunnerController.RespawnPos != null)
            {
                collision.transform.position = _gunnerController.RespawnPos.transform.position;
            }

            Destroy(gameObject);
            return;
        }

        if (((1 << collision.gameObject.layer) & _layerMask) != 0
            || collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
    public void SetController(GunnerController controller)
    {
        _gunnerController = controller;
    }
}

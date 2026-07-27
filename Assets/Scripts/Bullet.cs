using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("총알 설정")]
    public float speed = 15f;     
    public float lifetime = 2f;

    [Header("충돌 레이어 설정")]
    [SerializeField] private LayerMask _destroyLayer;

    private float _direction = 1f;

    public void Setup(float dir)
    {
        _direction = dir;

        if (_direction < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.right * _direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((_destroyLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }
}

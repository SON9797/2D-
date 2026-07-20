using System.Collections;
using UnityEngine;

public class GunnerController : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private int _gunnerScore = 20;
    [SerializeField] private GameObject _respawnPos;

    [Header("전투 설정")]
    [SerializeField] private float _detectionRange = 5.0f;
    [SerializeField] private float _fireRate = 3f;
    [SerializeField] private float _bulletSpeed = 10f;
    [SerializeField] private float _chargeTime = 1.0f;

    private Transform _playerTransform;
    private float _fireCooldownTimer = 0f;
    private bool _isAttacking = false;
    private bool _isDead = false;

    public Animator anim;
    Rigidbody2D _rb;
    SpriteRenderer _spriteRenderer;

    public GameObject RespawnPos => _respawnPos;

    void Start()
    {
        anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            _playerTransform = player.transform;
        }

        if (_firePoint == null)
        {
            _firePoint = this.transform;
        }
    }

    void Update()
    {
        if (_playerTransform == null || _isDead) return;

        if (!_isAttacking)
        {
            LookAtPlayer();
        }

        if (_fireCooldownTimer > 0)
        {
            _fireCooldownTimer -= Time.deltaTime;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

        if (distanceToPlayer <= _detectionRange && _fireCooldownTimer <= 0f && !_isAttacking && !_isDead)
        {
            StartCoroutine(ChargeEnergyAndShoot());
        }

    }

    private void LookAtPlayer()
    {
        if (_playerTransform.position.x < transform.position.x)
        {
            _spriteRenderer.flipX = false;
        }

        else if (_playerTransform.position.x > transform.position.x)
        {
            _spriteRenderer.flipX = true;
        }
    }

    private void Shoot()
    {
        Vector2 direction = (_playerTransform.position - _firePoint.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        angle -= 90f;

        Quaternion bulletRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, bulletRotation);

        GunnerBullet bulletScript = bullet.GetComponent<GunnerBullet>();
        if (bulletScript != null)
        {
            bulletScript.SetController(this);
        }

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * _bulletSpeed;
        }
    }

    IEnumerator ChargeEnergyAndShoot()
    {
        _isAttacking = true;

        if (anim != null)
        {
            anim.SetTrigger("Charge");
        }

        yield return new WaitForSeconds(_chargeTime);

        Shoot();

        _fireCooldownTimer = _fireRate;
        _isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 contactPoint = collision.contacts[0].point;
        Vector2 center = transform.position;

        Vector2 direction = (contactPoint - center).normalized;

        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x) && collision.gameObject.CompareTag("Player"))
        {
            if (direction.y > 0)
            {
                GunnerDie();
            }

            else
            {
                Debug.Log("아래");
            }
        }

        else
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.transform.position = _respawnPos.transform.position;
            }
        }
    }

    void GunnerDie()
    {
        _isDead = true;

        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;

        _rb.bodyType = RigidbodyType2D.Kinematic;

        Collider2D enemyCollider = GetComponent<Collider2D>();
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        anim.SetTrigger("Death");

        ScoreManager.instance.PlusScore(_gunnerScore);

        Destroy(gameObject, 5f);
    }
}

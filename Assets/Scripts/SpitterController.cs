using System.Collections;
using UnityEngine; 

public class SpitterController : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private float _spitterSpeed = 1.0f;
    [SerializeField] private float _changeTime = 2f;
    [SerializeField] private GameObject _respawnPos;
    [SerializeField] private int _spitterScore = 10;

    [Header("점프 설정")]
    [SerializeField] private float _minJumpInterval = 2f;
    [SerializeField] private float _maxJumpInterval = 5f;
    [SerializeField] private float _spitterJumpForce = 5f;
    [SerializeField] private LayerMask _groundLayer;

    private float _timer;
    private Vector2 _directionX;

    private float _jumpTimer;
    private float _nextJumpTime;
    private bool _isGrounded;
    private Collider2D _collider;

    public Animator anim;
    Rigidbody2D _rb;
    SpriteRenderer _spriteRenderer;

    void Start()
    {
        anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();

        ChomperMove();

        SetRandomJumpTime();
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _changeTime)
        {
            ChomperMove();
        }

        _jumpTimer += Time.deltaTime;

        if (_jumpTimer >= _nextJumpTime)
        {
            TryJump();
        }

        transform.Translate(new Vector3(_directionX.x * _spitterSpeed * Time.deltaTime, 0, 0));
    }

    void SetRandomJumpTime()
    {
        _nextJumpTime = Random.Range(_minJumpInterval, _maxJumpInterval);
        _jumpTimer = 0f;
    }

    void TryJump()
    {
        _isGrounded = Physics2D.IsTouchingLayers(_collider, _groundLayer);

        if (_isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _spitterJumpForce);
        }

        SetRandomJumpTime();
    }

    void ChomperMove()
    {
        int randomX = Random.Range(0, 2) == 0 ? -1 : 1;

        _directionX = new Vector2(randomX, 0).normalized;

        UpdateSpriteDirection();

        _timer = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("TurningPoint"))
        {
            _directionX *= -1;

            UpdateSpriteDirection();

            _timer = 0f;
        }
    }

    void UpdateSpriteDirection()
    {
        if (_directionX.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_directionX.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
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
                SpitterDie();
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

    void SpitterDie()
    {
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;

        _rb.bodyType = RigidbodyType2D.Kinematic;

        Collider2D enemyCollider = GetComponent<Collider2D>();
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        anim.SetTrigger("Death");

        ScoreManager.instance.PlusScore(_spitterScore);

        Destroy(gameObject, 0.5f);
    }
}

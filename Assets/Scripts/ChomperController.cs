using System.Collections;
using UnityEngine;

public class ChomperController : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private float _chomperSpeed = 1.5f;
    [SerializeField] private float _changeTime = 2f;
    [SerializeField] private GameObject _respawnPos;
    [SerializeField] private int _chomperScore = 5;

    private float _timer;
    private Vector2 _directionX;

    public Animator anim;
    Rigidbody2D _rb;
    SpriteRenderer _spriteRenderer;

    void Start()
    {
        anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        ChomperMove();
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _changeTime)
        {
            ChomperMove();
        }

        transform.Translate(_directionX * _chomperSpeed * Time.deltaTime);
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

        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y > 0)
            {
                ChomperDie();
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
                StartCoroutine(AttackRoutine());
            }
        }
    }

    void ChomperDie()
    {
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;

        _rb.bodyType = RigidbodyType2D.Kinematic;

        anim.SetTrigger("Death");

        ScoreManager.instance.PlusScore(_chomperScore);

        Destroy(gameObject, 0.5f);
    }

    IEnumerator AttackRoutine()
    {
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;

        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(2f);

        ChomperMove();
    }
}

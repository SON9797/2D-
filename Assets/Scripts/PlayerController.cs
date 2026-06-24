using UnityEngine;
using System;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("플레이어 설정")]
    [SerializeField] private float _playerSpeed = 3f;
    [SerializeField] private float _walkJumpForce = 6f;
    [SerializeField] private float _runJumpForce = 8f;

    [Header("바닥 체크")]
    [SerializeField] private float _rayLength = 1f;
    [SerializeField] private LayerMask _groundLayer;

    Animator _anim;
    Rigidbody2D _rb;
    SpriteRenderer _spriteRenderer;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        PlayerMove();
        JumpPlayer();
    }

    void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? _playerSpeed * 2f : _playerSpeed;

        _rb.linearVelocity = new Vector2(h * currentSpeed, _rb.linearVelocity.y);

        _anim.SetFloat("Speed", Mathf.Abs(_rb.linearVelocity.x));

        if (h > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (h < 0)
        {
            _spriteRenderer.flipX = true;        
        }
    }

    void JumpPlayer()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0f);

            float currentJumpForce = Input.GetKey(KeyCode.LeftShift) ? _runJumpForce : _walkJumpForce;

            _rb.AddForce(Vector2.up * currentJumpForce, ForceMode2D.Impulse);
            _anim.SetTrigger("Jump");
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _rayLength, _groundLayer);

        Debug.DrawRay(transform.position, Vector3.down * _rayLength, Color.red);

        return hit.collider != null;
    }
}

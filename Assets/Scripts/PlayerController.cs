using UnityEngine;
using System;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("플레이어 설정")]
    [SerializeField] private float _playerSpeed = 3f;
    [SerializeField] private float _jumpForce = 4f;

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
    }

    void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");

        _anim.SetFloat("Speed", Mathf.Abs(h));
    }
}

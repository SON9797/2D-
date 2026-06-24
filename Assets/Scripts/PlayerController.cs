using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Normal,
        Gun,
        Death,
    }

    public PlayerState currentState;

    [Header("플레이어 설정")]
    [SerializeField] private float _playerSpeed = 3f;
    [SerializeField] private float _walkJumpForce = 6f;
    [SerializeField] private float _runJumpForce = 9f;

    [Header("바닥 체크")]
    [SerializeField] private float _rayLength = 1f;
    [SerializeField] private LayerMask _groundLayer;

    public Animator _anim;
    Rigidbody2D _rb;
    SpriteRenderer _spriteRenderer;

    private PlatformEffector2D _currentEffector;
    private SpriteChanger _spriteChanger;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteChanger = Object.FindFirstObjectByType<SpriteChanger>();

        currentState = PlayerState.Normal;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    currentState = PlayerState.Gun;
        //    _anim.SetTrigger("GetGun");
        //}

        switch (currentState) 
        {
            case PlayerState.Normal:
                PlayerMove();
                CrouchPlayer();
                JumpPlayer();
                break;

            case PlayerState.Gun:
                PlayerMove();
                CrouchPlayer();
                JumpPlayer();
                break;

            case PlayerState.Death:
                break;
        }
    }

    private void LateUpdate()
    {
        LimitPlayerInScreen();
    }

    void PlayerMove()
    {
        if (Input.GetAxisRaw("Vertical") < 0f && IsGrounded()) 
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y); 
            return;
        }

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

    void CrouchPlayer()
    {
        if (Input.GetAxisRaw("Vertical") < 0f && IsGrounded())
        {
            _anim.SetBool("Crouch", true);
        }

        else
        {
            _anim.SetBool("Crouch", false);
        }
    }

    void JumpPlayer()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            if (Input.GetAxisRaw("Vertical") < 0f && _currentEffector != null)
            {
                StartCoroutine(DropDownRoutine());
            }

            else
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0f);

                float currentJumpForce = Input.GetKey(KeyCode.LeftShift) ? _runJumpForce : _walkJumpForce;

                _rb.AddForce(Vector2.up * currentJumpForce, ForceMode2D.Impulse);
                _anim.SetTrigger("Jump");
            }            
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _rayLength, _groundLayer);
        Debug.DrawRay(transform.position, Vector3.down * _rayLength, Color.red);

        if (hit.collider != null)
        {
            _currentEffector = hit.collider.GetComponent<PlatformEffector2D>();
            return true;
        }

        _currentEffector = null;
        return false;
    }

    void LimitPlayerInScreen()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        viewportPos.x = Mathf.Clamp(viewportPos.x, 0.05f, 0.95f);

        transform.position = Camera.main.ViewportToWorldPoint(viewportPos);
    }

    private IEnumerator DropDownRoutine()
    {
        _anim.SetTrigger("Jump");

        _currentEffector.rotationalOffset = 180f;

        yield return new WaitForSeconds(0.5f);

        _currentEffector.rotationalOffset = 0f;
    }
}

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
    [SerializeField] private float _walkJumpForce = 8f;
    [SerializeField] private float _runJumpForce = 13f;

    [Header("바닥 체크")]
    [SerializeField] private float _rayLength = 1f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("총 설정")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireRate = 0.2f;

    private float _nextFireTime = 0f;
    private float _lastShootTime = 0f;

    public Animator _anim;
    Rigidbody2D _rb;
    SpriteRenderer _spriteRenderer;

    private PlatformEffector2D _currentEffector;
    private SpriteChanger _spriteChanger;

    public float HorizontalVelocity => _rb != null ? _rb.linearVelocity.x : 0f;
    public bool IsMoving => Mathf.Abs(HorizontalVelocity) > 0.01f;

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
                HandleShooting();
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

        if (Time.time >= _lastShootTime + 0.2f)
        {
            if (h > 0)
            {
                SetFacingDirection(false);
            }
            else if (h < 0)
            {
                SetFacingDirection(true);
            }
        }
    }

    void SetFacingDirection(bool isLeft)
    {
        _spriteRenderer.flipX = isLeft;

        if (_firePoint != null)
        {
            float xPos = Mathf.Abs(_firePoint.localPosition.x);
            _firePoint.localPosition = new Vector3(isLeft ? -xPos : xPos, _firePoint.localPosition.y, 0);
        }
    }

    void HandleShooting()
    {
        if (Time.time < _nextFireTime) return;

        bool shootLeft = Input.GetKeyDown(KeyCode.Comma);
        bool shootRight = Input.GetKeyDown(KeyCode.Period);

        if (shootLeft || shootRight)
        {
            _nextFireTime = Time.time + _fireRate;
            _lastShootTime = Time.time;

            float fireDirection = 0f;

            if (shootLeft)
            {
                fireDirection = -1f;
                SetFacingDirection(true);
            }
            else if (shootRight)
            {
                fireDirection = 1f;
                SetFacingDirection(false);
            }

            Shoot(fireDirection);
        }
    }

    void Shoot(float direction)
    {
        if (_bulletPrefab != null && _firePoint != null)
        {
            GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);

            Bullet bulletLogic = bullet.GetComponent<Bullet>();
            if (bulletLogic != null)
            {
                bulletLogic.Setup(direction);
            }
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
        PlatformEffector2D cachedEffector = _currentEffector;
        if (cachedEffector == null) yield break;

        _anim.SetTrigger("Jump");

        cachedEffector.rotationalOffset = 180f;

        yield return new WaitForSeconds(0.5f);

        if (cachedEffector != null)
        {
            cachedEffector.rotationalOffset = 0f;
        }
    }
}

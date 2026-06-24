using UnityEngine;

public class GetWeapon : MonoBehaviour
{
    [Header("¼³Á¤")]
    [SerializeField] private GameObject _uiText;

    private bool _interaction = false;
    PlayerController _playerController;

    void Start()
    {
        _uiText.SetActive(false);

        _playerController = Object.FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        GetGun();
    }

    void GetGun()
    {
        if (_interaction == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                _playerController.currentState = PlayerController.PlayerState.Gun;
                _playerController._anim.SetTrigger("GetGun");

                Destroy(_uiText);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _interaction = true;
            _uiText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_uiText == null)
            {
                return;
            }

            _interaction = false;
            _uiText.SetActive(false);
        }
    }
}

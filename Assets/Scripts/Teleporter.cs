using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    [Header("¼³Á¤")]
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject _uiText;

    private bool _interaction = false;
    PlayerController _playerController;

    int endScore = 5;

    bool isClear = false;

    void Start()
    {
        _anim.speed = 0;

        _uiText.SetActive(false);

        _playerController = Object.FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        ClearStage();

        if (isClear && _interaction)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                LoadNextStage();
            }
        }
    }

    void ClearStage()
    {
        if (endScore <= ScoreManager.instance.currentScore)
        {
            _anim.speed = 1;
            isClear = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isClear && collision.gameObject.CompareTag("Player"))
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
    
    void LoadNextStage()
    {
        SceneManager.LoadScene("1_2");
    }
}

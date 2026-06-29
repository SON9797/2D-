using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("≈ÿΩ∫∆Æ")]
    [SerializeField] private TextMeshProUGUI _scoreText;

    int score = 0;
    public int currentScore;

    public static ScoreManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentScore = score;

        if (_scoreText != null)
        {
            _scoreText.text = "Score : " + currentScore;
        }
    }

    void Update()
    {

    }

    public void PlusScore(int coin)
    {
        currentScore += coin;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        _scoreText.text = "Score : " + currentScore;
    }
}

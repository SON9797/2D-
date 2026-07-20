using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;

    [Header("Fade Settings")]
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeDuration = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        if (_fadeImage != null)
        {
            Color color = _fadeImage.color;
            color.a = 0f;
            _fadeImage.color = color;
            _fadeImage.gameObject.SetActive(false);
        }
    }

    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        _fadeImage.gameObject.SetActive(true);
        float elapsedTime = 0f;
        Color color = _fadeImage.color;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / _fadeDuration);
            _fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(0.1f);

        elapsedTime = 0f;
        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsedTime / _fadeDuration);
            _fadeImage.color = color;
            yield return null;
        }

        _fadeImage.gameObject.SetActive(false);
    }
}

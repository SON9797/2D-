using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour
{
    [Header("¿Ã∆Â∆Æ º≥¡§")]
    [SerializeField] private Sprite[] _flashSprites;
    [SerializeField] private float _frameRate = 0.03f;

    private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
    }

    public void ShowEffect(bool isLeft)
    {
        _spriteRenderer.flipX = !isLeft;

        StopAllCoroutines();
        StartCoroutine(PlayFlashAnimation());
    }

    private IEnumerator PlayFlashAnimation()
    {
        _spriteRenderer.enabled = true;

        for (int i = 0; i < _flashSprites.Length; i++)
        {
            _spriteRenderer.sprite = _flashSprites[i];
            yield return new WaitForSeconds(_frameRate);
        }

        _spriteRenderer.enabled = false;
    }
}

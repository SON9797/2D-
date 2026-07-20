using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    [Header("¼³Á¤")]
    [SerializeField] public Sprite _originalSprite;
    [SerializeField] public Sprite _changeSprite;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComp
                
                nt<SpriteRenderer>();
        }
    }

    public void ChangeSprite()
    {
        spriteRenderer.sprite = _changeSprite;
    }
}

using DG.Tweening;
using System.Collections;
using TreeEditor;
using UnityEngine;

public class Coin : MonoBehaviour
{
    int _coinScore = 1;

    public Vector3 viewportTargetPos = new Vector3(0.9f, 0.9f, 0); 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
            }

            StartCoroutine(DOTweenRoutine());
        }
    }

    IEnumerator DOTweenRoutine()
    {
        float duration = 1f;
        float elapsed = 0f;

        Vector3 startPos = transform.position;

        DOTween.To(() => elapsed, x => elapsed = x, 1f, duration)
            .SetEase(Ease.InOutQuad)
            .OnUpdate(() =>
            {
                Vector3 currentTargetWorldPos = Camera.main.ViewportToWorldPoint(viewportTargetPos);
                currentTargetWorldPos.z = transform.position.z;

                transform.position = Vector3.Lerp(startPos, currentTargetWorldPos, elapsed);
            });

        yield return new WaitForSeconds(duration);

        ScoreManager.instance.PlusScore(_coinScore);
        Destroy(gameObject);
    }
}

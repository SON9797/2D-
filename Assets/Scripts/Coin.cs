using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    int _coinScore = 1;

    public Vector3 targetPos = new Vector3(9.8f, 3, 0);

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Destroy(gameObject);
    //
    //        ScoreManager.instance.PlusScore(_coinScore);
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DOTweenRoutine());
        }
    }

    IEnumerator DOTweenRoutine()
    {
        Vector3 wolrdPos = Camera.main.ViewportToScreenPoint(targetPos);

        wolrdPos.z = transform.position.z;

        transform.DOMove(wolrdPos, 1f);

        yield return new WaitForSeconds(1f);

        //Destroy(gameObject);
        ScoreManager.instance.PlusScore(_coinScore);
    }
}

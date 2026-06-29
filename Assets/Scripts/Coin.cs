using UnityEngine;

public class Coin : MonoBehaviour
{
    int _coinScore = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);

            ScoreManager.instance.PlusScore(_coinScore);
        }
    }
}

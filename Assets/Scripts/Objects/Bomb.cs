using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public SpriteRenderer spriteBomba; // trocar pela animação
    public GameObject explosionVFX;

    private void TurnWhite(SpriteRenderer sprite) // trocar pela animação
    {
        sprite.color = Color.white;
    }

    private void TurnRed(SpriteRenderer sprite) // trocar pela animação
    {
        sprite.color = Color.red;
    }

    IEnumerator BeginExplosion()
    {
        for (int i = 0; i < 10; i++) {
            yield return new WaitForSeconds(0.1f);
            TurnWhite(spriteBomba);
            yield return new WaitForSeconds(0.1f);
            TurnRed(spriteBomba);
        }

        Explode();
    }

    public void TriggerExplosion()
    {
        StartCoroutine(BeginExplosion());
    }

    public void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.5f);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].CompareTag("Player"))
            {
                hits[i].GetComponent<PlayerController>().EnterGetHit(1);
            }
        }
    }

    public void Explode()
    {
        Instantiate(explosionVFX, transform.position, transform.rotation);
        DealDamage();
        Destroy(gameObject);
    }
}

using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    public int health = 3;

    [Header("Sprites de estados")]
    public Sprite damagedSprite;
    public Sprite brokenSprite;

    [Header("Efeitos")]
    public GameObject breakEffect;// partículas ou pedaços

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    /*public void TakeDamage(int damage)
    {
        currentHealth -= damage;
// Trocar sprite quando estiver danificada
        if (currentHealth == 1 && damagedSprite != null)
            sr.sprite = damagedSpriteSprite;
 // Quando vida zerar → quebrar
        if (currentHealth <= 0)
            BreakableBox();
    }
    */
    public void Break()
    {
        Instantiate(breakEffect, transform.position, transform.rotation);

        if (brokenSprite != null)
        {
            sr.sprite = brokenSprite;
        }

        // Desativa colisão e destrói depois de um tempo
        //GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }
}


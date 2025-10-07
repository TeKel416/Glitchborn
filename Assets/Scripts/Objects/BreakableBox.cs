using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Sprites de estados")]
    public Sprite damagedSprite;
    public Sprite brokenSprite;

    [Header("Efeitos")]
    public GameObject breakEffect;// partículas ou pedaços

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetCompent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)

    {
        currentHealth -= damage;
// Trocar sprite quando estiver danificada
        if (currentHealth == 1 && damagedSprite != null)
            sr.sprite = damagedSpriteSprite;
 // Quando vida zerar → quebrar
        if (currentHealth <= 0)
            BreakableBox();
    }

    void Break()
    {
        if (breakEffect != null)
            Instantiate(breakEffect, transform.position, Quaternion.identity);

        if (brokenSprite != null)
            sr.sprite = brokenSprite;

        // Desativa colisão e destrói depois de um tempo
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 0.2f);
    }
}


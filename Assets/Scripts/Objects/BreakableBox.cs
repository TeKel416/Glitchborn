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

    public void Break()
    {
        SoundManager.instance.PlaySound2D("BreakBox");
        Instantiate(breakEffect, transform.position, transform.rotation);

        if (brokenSprite != null)
        {
            sr.sprite = brokenSprite;
        }

        Destroy(gameObject);
    }
}


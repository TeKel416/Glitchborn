using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
	[System.NonSerialized] public float damage = 1;
	public GameObject hitVFX;
	public float lifeTime = 5;
    public float knockbackForce = 5f;

    void Start() => Destroy(gameObject, lifeTime);

	public void OnTriggerEnter2D(Collider2D hit)
	{
		if (hit.CompareTag("Player"))
		{
            PlayerController player = hit.GetComponent<PlayerController>();
            player.EnterGetHit(damage);

            Vector2 direction = (player.transform.position - transform.position).normalized;
            player.rb.linearVelocity = direction * knockbackForce;
        }

        GameObject vfx = Instantiate(hitVFX, transform.position, transform.rotation);
		Destroy(gameObject);
        Destroy(vfx, 0.1f);
    }
}
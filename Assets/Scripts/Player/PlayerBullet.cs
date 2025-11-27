using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
	[System.NonSerialized] public float damage = 1;
	public GameObject hitVFX;
	public float lifeTime = 5;

	void Start() => Destroy(gameObject, lifeTime);

	public void OnTriggerEnter2D(Collider2D hit)
	{
		if (hit.CompareTag("EnemyMelee"))
		{
			hit.GetComponent<EnemyMeleeController>().EnterGetHit(damage);
		}
        else if (hit.CompareTag("EnemyShooter"))
        {
            hit.GetComponent<EnemyShooterController>().EnterGetHit(damage);

        }
        else if (hit.CompareTag("Boss"))
		{
            hit.GetComponent<BossController>().EnterGetHit(damage);

        } else if (hit.CompareTag("BossArm"))
		{
            hit.GetComponent<BossArm>().EnterGetHit(damage);
        }

		GameObject vfx = Instantiate(hitVFX, transform.position, transform.rotation);
		Destroy(gameObject);
        Destroy(vfx, 0.1f);
    }
}
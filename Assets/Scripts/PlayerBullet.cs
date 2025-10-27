using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
	[System.NonSerialized] public float damage = 1;
	public GameObject hitVFX;
	public float lifeTime = 5;

	void Start() => Destroy(gameObject, lifeTime);

	public void OnTriggerEnter2D(Collider2D hit)
	{
		if (hit.CompareTag("Enemy"))
		{
			hit.GetComponent<EnemyController>().EnterGetHit(damage);
		}

		Instantiate(hitVFX, transform.position, transform.rotation);
		Destroy(gameObject);
	}
}
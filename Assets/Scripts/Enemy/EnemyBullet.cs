using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
	[System.NonSerialized] public float damage = 1;
	public GameObject hitVFX;
	public float lifeTime = 5;

	void Start() => Destroy(gameObject, lifeTime);

	public void OnTriggerEnter2D(Collider2D hit)
	{
		if(hit.CompareTag("Player"))
		{
			hit.GetComponent<PlayerController>().EnterGetHit(damage);
		}

		Instantiate(hitVFX, transform.position, transform.rotation);
		Destroy(gameObject);
	}
}
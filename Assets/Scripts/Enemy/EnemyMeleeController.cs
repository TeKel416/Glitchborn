using UnityEngine;

public class EnemyMeleeController : EnemyController
{
    public float attackRange = 1.5f;
	public GameObject hitVFX;

    public override void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].CompareTag("Player"))
            {
                hits[i].GetComponent<PlayerController>().EnterGetHit(damage);
                Instantiate(hitVFX, attackPoint.position, attackPoint.rotation);
            }
        }
    }
}
using UnityEngine;

public class EnemyShooterController : EnemyController
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 30;

    public override void DealDamage()
    {
        Vector2 lookPos = (player.transform.position - transform.position).normalized;
        GameObject b = Instantiate(bulletPrefab, attackPoint.position, attackPoint.rotation);
        b.GetComponent<Rigidbody2D>().linearVelocity = lookPos * bulletSpeed;
        b.GetComponent<EnemyBullet>().damage = damage;
    }
}
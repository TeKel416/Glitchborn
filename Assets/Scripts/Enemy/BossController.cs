using System.Net.Sockets;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject bossArmPrefab;
    public GameObject bracos;

    public float getHitDelay = 0.2f;
    public float hp = 12;
    public float knockbackForce = 25f;

    protected PlayerController player;
    public bool locked = false;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    public void EnterGetHit(float dealtDamage)
    {
        locked = true;
        hp -= dealtDamage;
        VerifyHp();

        CancelInvoke("Unlock");
        Invoke("Unlock", getHitDelay);
    }

    private void VerifyHp()
    {
        if (hp == 9)
        {
            GameObject bossArm = Instantiate(bossArmPrefab, transform.position, transform.rotation, bracos.transform);
            Vector2 direction = (player.transform.position - transform.position).normalized;
            player.rb.linearVelocity = direction * knockbackForce;
        }
        if (hp == 6)
        {
            GameObject bossArm = Instantiate(bossArmPrefab, transform.position, transform.rotation, bracos.transform);
            Vector2 direction = (player.transform.position - transform.position).normalized;
            player.rb.linearVelocity = direction * knockbackForce;
        }
        else if (hp == 3)
        {
            GameObject bossArm = Instantiate(bossArmPrefab, transform.position, transform.rotation, bracos.transform);
            Vector2 direction = (player.transform.position - transform.position).normalized;
            player.rb.linearVelocity = direction * knockbackForce;
        }
        else if (hp == 0)
        {
            Destroy(bracos);
            Destroy(gameObject);
        }
    }

    void Unlock()
    {
        locked = false;
    }
}

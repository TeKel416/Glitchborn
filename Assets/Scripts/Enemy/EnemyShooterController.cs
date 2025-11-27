using UnityEngine;

public class EnemyShooterController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 30;

    public UnityEngine.AI.NavMeshAgent agent;
    public Animator anim;
    protected PlayerController player;
    private Rigidbody2D rb;

    private enum EnemyStates { Idle, Attack, Death }
    private EnemyStates state = EnemyStates.Idle;

    private float distance;
    private bool locked = false;

    [Header("Attack")]
    public float attackDistance = 8f;
    public float attackDelay = 2f;
    public Transform attackPoint;
    public float damage = 1;
    public float dealDamageDelay = 0.25f;

    [Header("Get Hit")]
    public float getHitDelay = 0.2f;
    public float hp = 3;
    public float knockbackForce = 6f;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        // congelar rotacao do objeto do inimigo
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (locked || player == null) return;

        distance = Vector2.Distance(transform.position, player.transform.position);

        switch (state)
        {
            case EnemyStates.Idle:
                IdleUpdate();
                break;
            case EnemyStates.Attack:
                AttackUpdate();
                break;
            case EnemyStates.Death:
                DeathUpdate();
                break;
        }
    }


    void IdleUpdate()
    {
        if (distance < attackDistance) // ataca o player
        {
            agent.isStopped = false;
            EnterAttack();
        }
        else
        {
            agent.isStopped = true;
        }
    }

    void AttackUpdate()
    {
        if (!locked && distance < attackDistance) // ataca
        {
            EnterAttack();
        } 
        else
        {
            state = EnemyStates.Idle;
        }
    }

    void DeathUpdate()
    {
        Destroy(gameObject, 0.1f);
    }

    public void EnterGetHit(float dealtDamage)
    {
        SoundManager.instance.PlaySound2D("Hit");
        locked = true;
        agent.isStopped = true;

        Vector2 direction = (transform.position - player.transform.position).normalized;
        agent.velocity = direction * knockbackForce;

        if (hp <= 0 && state != EnemyStates.Death)
        {
            state = EnemyStates.Death;
        }

        hp -= dealtDamage;

        CancelInvoke("Unlock");
        Invoke("Unlock", getHitDelay);
    }

    void EnterAttack()
    {
        LookAtTarget(player.transform.position);
        state = EnemyStates.Attack;
        agent.isStopped = true;
        locked = true;

        CancelInvoke("Unlock");
        Invoke("Unlock", attackDelay);

        CancelInvoke("DealDamage");
        Invoke("DealDamage", dealDamageDelay);
    }

    void Unlock()
    {
        locked = false;
        agent.isStopped = false;
    }

    void LookAtTarget(Vector3 lookTarget)
    {
        Vector2 direction = (lookTarget - transform.position).normalized;
        if (direction != Vector2.zero)
        {
            anim.SetFloat("X", direction.x);
            anim.SetFloat("Y", direction.y);
        }
    }

    public void DealDamage()
    {
        Vector2 lookPos = (player.transform.position - transform.position).normalized;
        GameObject b = Instantiate(bulletPrefab, attackPoint.position, attackPoint.rotation);
        b.GetComponent<Rigidbody2D>().linearVelocity = lookPos * bulletSpeed;
        b.GetComponent<EnemyBullet>().damage = damage;
    }
}
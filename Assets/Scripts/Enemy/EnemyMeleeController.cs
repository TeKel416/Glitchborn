using UnityEngine;

public class EnemyMeleeController : MonoBehaviour
{
    public GameObject hitVFX;

    public UnityEngine.AI.NavMeshAgent agent;
    public Animator anim;
    protected PlayerController player;
    private Rigidbody2D rb;

    private enum EnemyStates { Patrol, Chase, Hit, Death }
    private EnemyStates state = EnemyStates.Patrol;

    private float distance;
    private bool locked = false;

    [Header("Patrol Points")]
    public Transform[] patrolPoints;
    public float idleTimer = 3f;
    private int currentPatrolPoint = 0;

    [Header("Chase")]
    public float chaseDistance = 20f;

    [Header("Get Hit")]
    public float getHitDelay = 0.2f;
    public float hp = 3;
    public float knockbackForce = 7f;

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
            case EnemyStates.Patrol:
                PatrolUpdate();
                break;
            case EnemyStates.Chase:
                ChaseUpdate();
                break;
            case EnemyStates.Hit:
                HitUpdate();
                break;
            case EnemyStates.Death:
                DeathUpdate();
                break;
        }
    }

    void PatrolUpdate()
    {
        if (distance < chaseDistance) // persegue o player
        {
            agent.isStopped = false;
            locked = false;
            anim.SetBool("IsWalking", true);
            state = EnemyStates.Chase;
        }
        else // volta a patrulhar
        {
            agent.isStopped = false;
            locked = false;
            anim.SetBool("IsWalking", true);
            LookAtTarget(patrolPoints[currentPatrolPoint].position);
            agent.SetDestination(patrolPoints[currentPatrolPoint].position);

            if (Vector2.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < 1f)
            {
                currentPatrolPoint++;
                if (currentPatrolPoint >= patrolPoints.Length) currentPatrolPoint = 0;
            }
        }
    }

    void ChaseUpdate()
    {
        if (distance > chaseDistance) // volta a patrulhar
        {
            agent.isStopped = false;
            anim.SetBool("IsWalking", true);
            state = EnemyStates.Patrol;
        }
        else // persegue o player
        {
            if (agent.speed > 0f)
            {
                agent.SetDestination(player.transform.position);
                anim.SetBool("IsWalking", true);
            }
            LookAtTarget(player.transform.position);
        }
    }

    void HitUpdate()
    {
        locked = true;
        agent.isStopped = true;
        anim.SetBool("IsWalking", false);
        anim.SetBool("IsHurting", true);

        Invoke("Unlock", getHitDelay);
    }

    void DeathUpdate()
    {
        anim.SetBool("IsWalking", false);
        //anim.SetTrigger("Death");
        Destroy(gameObject, 0.1f);
    }

    public void EnterGetHit(float dealtDamage)
    {
        SoundManager.instance.PlaySound2D("Hit");
        hp -= dealtDamage;

        Vector2 direction = (transform.position - player.transform.position).normalized;
        agent.velocity = direction * knockbackForce;

        state = EnemyStates.Hit;

        if (hp <= 0 && state != EnemyStates.Death)
        {
            state = EnemyStates.Death;
        }
    }

    void Unlock()
    {
        locked = false;
        agent.isStopped = false;
        anim.SetBool("IsHurting", false);

        if (distance < chaseDistance) // persegue o player
        {
            state = EnemyStates.Chase;
        }
        else
        {
            state = EnemyStates.Patrol;
        }
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

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            state = EnemyStates.Hit;

            player.EnterGetHit(1);

            Vector2 direction = (player.transform.position - transform.position).normalized;
            player.rb.linearVelocity = direction * knockbackForce;
        }
    }
}
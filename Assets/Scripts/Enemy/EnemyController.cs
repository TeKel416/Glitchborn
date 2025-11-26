using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Animator anim;
    protected PlayerController player;
    private Rigidbody2D rb;

    private enum EnemyStates { Patrol, Idle, Chase, Attack, Death }
    private EnemyStates state = EnemyStates.Patrol;

    private float distance;
    private bool locked = false;

    [Header("Patrol Points")]
    public Transform[] patrolPoints;
    public float idleTimer = 3f;
    private int currentPatrolPoint = 0;

    [Header("Attack")]
    public float attackDistance = 1f;
    public float attackDelay = 1.0f;
    public Transform attackPoint;
    public float damage = 1;
    public float dealDamageDelay = 0.25f;

    [Header("Chase")]
    public float chaseDistance = 20f;
    
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
            case EnemyStates.Patrol:
                PatrolUpdate();
            break;
            case EnemyStates.Idle:
                IdleUpdate();
            break;
            case EnemyStates.Chase:
                ChaseUpdate();
            break;
            case EnemyStates.Attack:
                AttackUpdate();
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
            anim.SetBool("IsWalking", true);
            state = EnemyStates.Chase;
        }
        else // volta a patrulhar
        {
            agent.isStopped = false;
            anim.SetBool("IsWalking", true);
            LookAtTarget(patrolPoints[currentPatrolPoint].position);
            agent.SetDestination(patrolPoints[currentPatrolPoint].position);

            if(Vector2.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < 1f)
            {
                currentPatrolPoint++;
                if(currentPatrolPoint >= patrolPoints.Length) currentPatrolPoint = 0;
                state = EnemyStates.Idle;
                Invoke("GoPatrol", idleTimer);
            }
        }
    }

    void IdleUpdate()
    {
        if (distance < chaseDistance) // persegue o player
        {
            agent.isStopped = false;
            anim.SetBool("IsWalking", true);
            state = EnemyStates.Chase;
        }
        else
        {
            anim.SetBool("IsWalking", false);
            agent.isStopped = true;
        }   
    }

    private void GoPatrol()
    {
        state = EnemyStates.Patrol;
    }

    void ChaseUpdate()
    {
        if (distance > chaseDistance) // volta a patrulhar
        {
            agent.isStopped = false;
            if (agent.speed > 0f)
            {
                anim.SetBool("IsWalking", true);
            }
            state = EnemyStates.Patrol;
        }
        else if (!locked && distance < attackDistance) // ataca
        {
            EnterAttack();
        }
        else // persegue o player
        {
            if (agent.speed > 0f)
            {
                agent.SetDestination(player.transform.position);
            }
            LookAtTarget(player.transform.position);
        }
    }

    void AttackUpdate()
    {
        if (!locked && distance < attackDistance) // ataca
        {
            EnterAttack();
        }
        else if (distance < chaseDistance) // persegue player
        {
            agent.isStopped = false;
            anim.SetBool("IsWalking", true);
            state = EnemyStates.Chase;
        }
        else // volta a patrulhar
        {
            agent.isStopped = false;
            anim.SetBool("IsWalking", true);
            state = EnemyStates.Patrol;
        }
    }

    void DeathUpdate()
    {
        anim.SetBool("IsWalking", false);
        //anim.SetTrigger("Death");
        Destroy(gameObject, 0.1f);
    }

    public void EnterGetHit(float dealtDamage)
    {
        if (hp <= 0 && state != EnemyStates.Death)
        {
            state = EnemyStates.Death;
        }
        else
        {
            SoundManager.instance.PlaySound2D("Hit");
            locked = true;
            anim.SetBool("IsWalking", false);
            //tocar animacao de hit
            agent.isStopped = true;
            Vector2 direction = (transform.position - player.transform.position).normalized;
            agent.velocity = direction * knockbackForce;
            hp -= dealtDamage;

            CancelInvoke("Unlock");
            Invoke("Unlock", getHitDelay);
        }   
    }

    void EnterAttack()
    {
        LookAtTarget(player.transform.position);
        state = EnemyStates.Attack;
        anim.SetBool("IsWalking", false);
        anim.SetTrigger("Attack");
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

    public virtual void DealDamage(){}
}
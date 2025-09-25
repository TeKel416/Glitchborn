using UnityEngine;

public class EnemyMeleeController : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Animator anim;
    private PlayerController player;

    private enum EnemyStates { Patrol, Chase, Attack, Death }
    private EnemyStates state = EnemyStates.Patrol;

    private float distance;
    private bool locked = false;

    [Header("Patrol Points")]
    public Transform[] patrolPoints;
    private int currentPatrolPoint = 0;

    [Header("Attack")]
    public float attackDistance = 1.5f;
    public float attackRange = 1.5f;
    public float attackDelay = 1.0f;
    public Transform attackPoint;
    public float damage = 1;
    public float dealDamageDelay = 0.25f;

    [Header("Chase")]
    public float chaseDistance = 20f;
    
    [Header("Get Hit")]
    public float getHitDelay = 0.5f;    
    public float hp = 3;

    
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        // congelar rotacao do objeto do inimigo
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (locked) return;

        distance = Vector2.Distance(transform.position, player.transform.position);

        switch (state)
        {
            case EnemyStates.Patrol:
                PatrolUpdate();
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
            LookAtTarget(patrolPoints[currentPatrolPoint].position);
            agent.SetDestination(patrolPoints[currentPatrolPoint].position);
            if(Vector2.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < 1f)
            {
                currentPatrolPoint++;
                if(currentPatrolPoint >= patrolPoints.Length) currentPatrolPoint = 0;
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
        else if (distance < attackDistance) // ataca
        {
            EnterAttack();
        }
        else // persegue o player
        {
            agent.SetDestination(player.transform.position);
            LookAtTarget(player.transform.position);
        }
    }

    void AttackUpdate()
    {
        if (distance < attackDistance) // ataca
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
        Debug.Log("inimigo morreu");
        //anim.SetTrigger("Death");
        Destroy(gameObject, 0.5f);
    }

    public void EnterGetHit(float dealtDamage)
    {
        Debug.Log("inimigo ai");
        //tocar animacao de hit
        locked = true;
        agent.isStopped = true;
        hp -= dealtDamage;
        if (hp <= 0)
        {
            state = EnemyStates.Death;
        }
        CancelInvoke("Unlock");
        Invoke("Unlock", getHitDelay);
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

    void Unlock() => locked = false;

    void LookAtTarget(Vector3 lookTarget)
    {
        Vector2 direction = (lookTarget - transform.position).normalized;
        if (direction != Vector2.zero)
        {
            anim.SetFloat("X", direction.x);
            anim.SetFloat("Y", direction.y);
        }
    }

    void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].CompareTag("Player"))
            {
                hits[i].GetComponent<PlayerController>().EnterGetHit(damage);
            }
        }
    }
}
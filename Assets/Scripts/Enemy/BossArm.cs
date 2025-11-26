using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    protected PlayerController player;

    public float damage = 1;
    public float dealDamageDelay = 0.25f;

    public float attackDistance = 1f;
    public float attackDelay = 1.0f;
    public float knockbackForce = 6f;

    private float distance;
    private enum EnemyStates { Chase, Attack }
    private EnemyStates state = EnemyStates.Chase;
    bool locked = false;

    public Transform attackPoint;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (player == null) return;

        distance = Vector2.Distance(transform.position, player.transform.position);

        switch (state)
        {
            case EnemyStates.Chase:
                ChaseUpdate();
                break;
            case EnemyStates.Attack:
                AttackUpdate();
                break;
        }
    }

    void ChaseUpdate()
    {
        if (!locked && distance < attackDistance) // ataca
        {
            EnterAttack();
        }
        else // persegue o player
        {
            if (agent.speed > 0f)
            {
                agent.SetDestination(player.transform.position);
            }
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
            agent.isStopped = false;
            state = EnemyStates.Chase;
        }
    }

    void EnterAttack()
    {
        state = EnemyStates.Attack;
        agent.isStopped = true;
        locked = true;

        CancelInvoke("Unlock");
        Invoke("Unlock", attackDelay);

        CancelInvoke("DealDamage");
        Invoke("DealDamage", dealDamageDelay);
    }

    public void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(3f, 2), 0);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].CompareTag("Player"))
            {
                PlayerController player = hits[i].GetComponent<PlayerController>();
                player.EnterGetHit(damage);

                Vector2 direction = (player.transform.position - transform.position).normalized;
                player.rb.linearVelocity = direction * knockbackForce;
            }
        }
    }

    void Unlock()
    {
        locked = false;
    }
}

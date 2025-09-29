using UnityEngine;

public class NPCController : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Animator anim;
    private PlayerController player;

    [Header("Patrol Points")]
    public Transform[] patrolPoints;
    private int currentPatrolPoint = 0;

    private enum NPCStates { Patrol, Idle, Talking }
    private NPCStates state = NPCStates.Patrol;


    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        switch (state)
        {
            case NPCStates.Patrol:
                PatrolUpdate();
                break;
            case NPCStates.Idle:
                IdleUpdate();
                break;
            case NPCStates.Talking:
                TalkingUpdate();
                break;
        }
    }

    void PatrolUpdate()
    {
        agent.isStopped = false;
        anim.SetBool("IsWalking", true);
        LookAtTarget(patrolPoints[currentPatrolPoint].position);
        agent.SetDestination(patrolPoints[currentPatrolPoint].position);

        if (Vector2.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < 0.1f)
        {
            currentPatrolPoint++;
            if (currentPatrolPoint >= patrolPoints.Length) currentPatrolPoint = 0;
            state = NPCStates.Idle;
        }
    }

    void IdleUpdate()
    {
        agent.isStopped = true;
        anim.SetBool("IsWalking", false);
        Invoke("Patrol", 3f);
    }

    void TalkingUpdate()
    {
        LookAtTarget(player.transform.position);
        agent.isStopped = true;
        anim.SetBool("IsWalking", false);
    }

    void Patrol() => state = NPCStates.Patrol;
            
    void LookAtTarget(Vector3 lookTarget)
    {
        Vector2 direction = (lookTarget - transform.position).normalized;
        if (direction != Vector2.zero)
        {
            anim.SetFloat("X", direction.x);
            anim.SetFloat("Y", direction.y);
        }
    }
}

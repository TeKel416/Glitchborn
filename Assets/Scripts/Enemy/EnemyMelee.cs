using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    private Animator animator;
    private PlayerController player;

    [Header("Ataque")]
    public float attackCooldownTimer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        // congelar rotação do objeto do inimigo
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        agent.SetDestination(player.transform.position);

        if (Vector2.Distance(transform.position, player.transform.position) <= agent.stoppingDistance)
        {
            // chegou no limite de distância do player
            animator.SetBool("IsWalking", false);
            // TODO código de ataque do inimigo
            animator.SetBool("IsAttacking", true);
        }
        else
        {
            // inimigo volta a perseguir
            animator.SetBool("IsAttacking", false);

            Vector2 direction = (player.transform.position - transform.position).normalized;

            if (direction != Vector2.zero)
            {
                animator.SetFloat("X", direction.x);
                animator.SetFloat("Y", direction.y);
                animator.SetBool("IsWalking", true);
            }
        }
    }

    private void Attack()
    {

    }
}
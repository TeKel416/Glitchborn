using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private float XMovement;
    private float YMovement;

    public UnityEngine.AI.NavMeshAgent agent;
    private PlayerController player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        // congelar rotação do objeto do inimigo
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        agent.SetDestination(player.transform.position);

        if (Vector2.Distance(transform.position, player.transform.position) <= agent.stoppingDistance)
        {
            // chegou no limite de distância do player
            // TODO código de animação idle/ataque do inimigo
            // TODO código de ataque do inimigo
        }
        else
        {
            // inimigo volta a perseguir
            // TODO código da animação de andar do inimigo
        }
    }
}
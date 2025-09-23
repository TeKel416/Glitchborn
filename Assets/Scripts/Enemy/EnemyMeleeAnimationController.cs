using UnityEngine;

public class EnemyMeleeAnimationController : MonoBehaviour
{
    private Animator animator;

    private float XMovement;
    private float YMovement;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


}

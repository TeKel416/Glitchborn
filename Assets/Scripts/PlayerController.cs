using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    private Animator animator;

    [Header("Movement")]
    public float speed;

    private Vector2 moveInput;

    [Header("Roll / Dodge")]
    public float rollSpeed;
    public float rollDuration;
    public float rollCooldown;

    private Vector2 rollDirection;
    private Vector2 lastMoveDir;
    private bool isRolling = false;
    private float rollTimer;

    [Header("Actions")]
    public InputActionReference moveAction;
    public InputActionReference rollAction;
    public InputActionReference attackAction;
    public InputActionReference shootAction;
    public InputActionReference hackAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (rollTimer > 0f)
        {
            rollTimer -= Time.deltaTime;

            if (isRolling && rollTimer <= rollCooldown)
            {
                isRolling = false; // saiu do roll, entra no cooldown
            }

            if (rollTimer <= 0f)
            {
                rollTimer = 0f; // corrige rollTimer caso ele fique negativo
            }
        }
    }

    private void FixedUpdate()
    {
        if (isRolling)
        {
            rb.linearVelocity = rollDirection * rollSpeed;
        }
        else
        {
            rb.linearVelocity = moveInput * speed;
        }
    }

    private void OnEnable()
    {
        // mover
        moveAction.action.Enable();
        moveAction.action.performed += OnMovePerformed;
        moveAction.action.canceled += OnMoveCanceled;
        // rolar
        rollAction.action.Enable();
        rollAction.action.performed += OnRollPerformed;
        // ataque melee
        attackAction.action.Enable();
        attackAction.action.performed += OnAttackPerformed;
        // tiro
        shootAction.action.Enable();
        shootAction.action.performed += OnShootPerformed;
        // hack
        hackAction.action.Enable();
        hackAction.action.performed += OnHackPerformed;
    }

    private void OnDisable()
    {
        // mover
        moveAction.action.Disable();
        moveAction.action.performed -= OnMovePerformed;
        moveAction.action.canceled -= OnMoveCanceled;
        // rolar
        rollAction.action.Disable();
        rollAction.action.performed -= OnRollPerformed;
        // ataque melee
        attackAction.action.Disable();
        attackAction.action.performed -= OnAttackPerformed;
        // tiro
        shootAction.action.Disable();
        shootAction.action.performed -= OnShootPerformed;
        // hack
        hackAction.action.Disable();
        hackAction.action.performed -= OnHackPerformed;
    }

    // mover
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        lastMoveDir = moveInput;

        if (moveInput != Vector2.zero)
        {
            animator.SetFloat("XInput", moveInput.x);
            animator.SetFloat("YInput", moveInput.y);
            animator.SetBool("IsWalking", true);
        }        
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
        animator.SetBool("IsWalking", false);
    }

    // rolar
    private void OnRollPerformed(InputAction.CallbackContext context)
    {
        if (rollTimer <= 0f) // s� deixa rolar quando o timer zerar
        {
            // pega dire��o atual ou �ltima dire��o v�lida
            if (moveInput.sqrMagnitude > 0.1f)
            {
                rollDirection = moveInput.normalized;

            }
            else if (lastMoveDir != Vector2.zero)
            {
                rollDirection = lastMoveDir; // fallback caso parado

            }
            else
            {
                rollDirection = Vector2.right; // fallback caso jogo rec�m iniciado
            }

            isRolling = true;
            rollTimer = rollDuration + rollCooldown;
        }
        else
        {
            Debug.Log("[ROLL BLOCKED] ainda em cooldown");
        }
    }

    // ataque melee
    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("ataque melee executado");
    }

    // tiro
    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("tiro executado");
    }

    // hack
    private void OnHackPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("hack executado");
    }
}
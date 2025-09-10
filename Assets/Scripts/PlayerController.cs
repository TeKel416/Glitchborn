using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

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
    private float nextRollTime;

    [Header("Actions")]
    public InputActionReference moveAction;
    public InputActionReference rollAction;
    public InputActionReference attackAction;
    public InputActionReference shootAction;
    public InputActionReference hackAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rollAction.action.triggered)
        {
            Debug.Log("[ROLL TRIGGER DETECTED]");
        }
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
        rollAction.action.performed += OnRollPerformed;
        // ataque melee
        attackAction.action.performed += OnAttackPerformed;
        // tiro
        shootAction.action.performed += OnShootPerformed;
        // hack
        hackAction.action.performed += OnHackPerformed;
    }

    private void OnDisable()
    {
        // mover
        moveAction.action.Disable();
        moveAction.action.performed -= OnMovePerformed;
        moveAction.action.canceled -= OnMoveCanceled;
        // rolar
        rollAction.action.performed -= OnRollPerformed;
        // ataque melee
        attackAction.action.performed -= OnAttackPerformed;
        // tiro
        shootAction.action.performed -= OnShootPerformed;
        // hack
        hackAction.action.performed -= OnHackPerformed;
    }

    // mover
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        lastMoveDir = moveInput;
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    // rolar
    private void OnRollPerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"[ROLL PRESSED] rollTimer={rollTimer}, isRolling={isRolling}");
        if (rollTimer <= 0f) // só deixa rolar quando o timer zerar
        {
            Debug.Log("[ROLL STARTED]");
            // pega direção atual ou última direção válida
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
                rollDirection = Vector2.right; // fallback caso jogo recém iniciado
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
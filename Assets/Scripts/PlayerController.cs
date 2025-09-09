using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Movement")]
    public float speed = 5;

    [Header("Roll / Dodge")]
    public float rollSpeed = 10;
    public float rollDuration = 0.3f;

    [Header("Actions")]
    public InputActionReference moveAction;
    public InputActionReference rollAction;

    private Vector2 moveInput;
    private Vector2 lastMoveDir;
    private Vector2 rollDirection;
    private bool isRolling = false;
    private float rollTimer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        // mover
        moveAction.action.Enable();
        moveAction.action.performed += OnMovePerformed;
        moveAction.action.canceled += OnMoveCanceled;

        // rolar
        rollAction.action.performed += OnRollPerformed;
    }

    private void OnDisable()
    {
        // mover
        moveAction.action.Disable();
        moveAction.action.performed -= OnMovePerformed;
        moveAction.action.canceled -= OnMoveCanceled;

        // rolar
        rollAction.action.performed -= OnRollPerformed;
    }

    private void Update()
    {
        if (isRolling)
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0f)
            {
                isRolling = false;
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
        if (!isRolling)
        {
            // pega direção atual ou última direção válida
            if (moveInput.sqrMagnitude > 0.1f)
            {
                rollDirection = moveInput.normalized;

            } else
            {
                if (lastMoveDir != Vector2.zero)
                {
                    rollDirection = lastMoveDir; // fallback caso parado

                } else
                {
                    rollDirection = Vector2.right; // fallback caso jogo recém iniciado
                }
                
            }

            isRolling = true;
            rollTimer = rollDuration;
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public bool locked = false;

    [Header("Health")]
    public float hp = 8;
    public float getHitDelay = 0.5f;

    [Header("Movement")]
    public float speed;

    private Vector2 moveInput;

    [Header("Roll / Dodge")]
    public float rollDuration;
    public float rollCooldown;

    private Vector2 rollDirection;
    private Vector2 lastMoveDir;
    public bool isRolling = false;
    private float rollTimer;

    [Header("Attack")]
    public float attackRange = 3f;
    public float attackDelay = 1f;
    public Transform attackPoint;
    public float damage = 1;
    public float dealDamageDelay = 0.25f;

    [Header("Interacao")]
    public bool interact = false;

    [Header("Shoot")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 30;
    public float shootDelay = 0.5f;

    [Header("VFXs")]
    public GameObject hitVFX;
    public GameObject rollVFX;

    [Header("Actions")]
    public InputActionReference moveAction;
    public InputActionReference rollAction;
    public InputActionReference attackAction;
    public InputActionReference shootAction;
    public InputActionReference hackAction;
    public InputActionReference speakAction;

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
        } 
        else
        {
            rollTimer = 0f; // corrige rollTimer caso ele fique negativo
        }
    }

    private void FixedUpdate()
    {
        if (isRolling) // rolamento
        {
            rb.linearVelocity = rollDirection * (speed * 3);
        }
        else // andar
        {
            if (!locked)
            {
                rb.linearVelocity = moveInput * speed;
                animator.SetBool("IsWalking", true);
                animator.SetFloat("XInput", lastMoveDir.x);
                animator.SetFloat("YInput", lastMoveDir.y);

                if (rb.linearVelocity == Vector2.zero)
                {
                    animator.SetBool("IsWalking", false);
                }
            }
            
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
        // conversar
        speakAction.action.Enable();
        speakAction.action.performed += OnSpeakPerformed;
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
        // conversar
        speakAction.action.Disable();
        speakAction.action.performed -= OnSpeakPerformed;
    }

    // mover
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        lastMoveDir = moveInput;

        if (moveInput != Vector2.zero && !locked)
        {
            animator.SetFloat("XInput", moveInput.x);
            animator.SetFloat("YInput", moveInput.y);
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    // rolar
    private void OnRollPerformed(InputAction.CallbackContext context)
    {
        if (!locked && rollTimer <= 0f) // so deixa rolar quando o timer zerar
        {
            // pega direcao atual ou ultima direcao valida
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
                rollDirection = Vector2.down; // fallback caso jogo recem iniciado
            }

            locked = true;
            isRolling = true;
            Instantiate(rollVFX, transform.position, transform.rotation);
            rollTimer = rollDuration + rollCooldown;

            CancelInvoke("Unlock");
            Invoke("Unlock", rollDuration);
        }
    }

    // ataque melee
    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (!locked)
        {
            animator.SetBool("IsWalking", false);
            //anim.SetTrigger("Attack");
            locked = true;
            rb.linearVelocity = Vector2.zero;

            Instantiate(hitVFX, attackPoint.position, attackPoint.rotation);

            CancelInvoke("Unlock");
            Invoke("Unlock", attackDelay);

            CancelInvoke("DealDamage");
            Invoke("DealDamage", dealDamageDelay);
        }
    }

    private void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].CompareTag("Enemy"))
            {
                hits[i].GetComponent<EnemyController>().EnterGetHit(damage);
            }
            else if (hits[i].CompareTag("BreakableBox"))
            {
                hits[i].GetComponent<BreakableBox>().Break();
            }
        }
    }

    // tomar dano
    public void EnterGetHit(float dealtDamage)
    {
        Debug.Log("player ai");
        //tocar animacao de hit
        locked = true;
        rb.linearVelocity = Vector2.zero;
        hp -= dealtDamage;

        if (hp <= 0)
        {
            Destroy(gameObject);
            SceneLoader.LoadScene("SampleScene");
        }

        GetComponent<HealthManager>().TakeDamage(dealtDamage);

        CancelInvoke("Unlock");
        Invoke("Unlock", getHitDelay);
    }

    public void Heal(float heal)
    {
        if (hp < 8)
        {
            hp += heal;
        }
        
        if (hp > 8)
        {
            hp = 8;
        }

        GetComponent<HealthManager>().Heal(heal);
    }

    // tiro
    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        if (!locked)
        {
            animator.SetBool("IsWalking", false);
            //anim.SetTrigger("Shoot");
            locked = true;
            rb.linearVelocity = Vector2.zero;

            GameObject b = Instantiate(bulletPrefab, attackPoint.position, attackPoint.rotation);
            b.GetComponent<Rigidbody2D>().linearVelocity = lastMoveDir.normalized * bulletSpeed;
            b.GetComponent<PlayerBullet>().damage = damage;

            CancelInvoke("Unlock");
            Invoke("Unlock", shootDelay);
        }
    }

    // hack
    private void OnHackPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("hack executado");
    }

    // conversar
    private void OnSpeakPerformed(InputAction.CallbackContext context)
    {
        interact = true;
        CancelInvoke("EndInteraction");
        Invoke("EndInteraction", 0.2f);
    }

    private void EndInteraction()
    {
        interact = false;
    }

    void Unlock() => locked = false;
}
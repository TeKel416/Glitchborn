using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;

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
    public Image rollCooldownIndicator;

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

    [Header("Shoot")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 30;
    public float shootDelay = 0.5f;

    [Header("Hack")]
    public bool hack = false;

    [Header("Interacao")]
    public bool interact = false;

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
            //rollCooldownIndicator.fillAmount = rollTimer / 100f;
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
        hackAction.action.canceled += OnHackCanceled;
        // conversar
        speakAction.action.Enable();
        speakAction.action.performed += OnSpeakPerformed;
        speakAction.action.canceled += OnSpeakCanceled;
    }

    private void OnDisable()
    {
        // mover
        moveAction.action.Disable();
        moveAction.action.performed -= OnMovePerformed;
        moveAction.action.canceled -= OnMoveCanceled;
        // ataque melee
        attackAction.action.Disable();
        attackAction.action.performed -= OnAttackPerformed;
        // hack
        hackAction.action.Disable();
        hackAction.action.performed -= OnHackPerformed;
        hackAction.action.performed -= OnHackCanceled;
        // conversar
        speakAction.action.Disable();
        speakAction.action.performed -= OnSpeakPerformed;
        speakAction.action.canceled -= OnMoveCanceled;
    }

    // mover
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        // Arredonda para o inteiro mais próximo (-1, 0, 1) para o Blend Tree
        Vector2 animationInput = new Vector2(
            Mathf.Round(moveInput.x),
            Mathf.Round(moveInput.y)
        );

        // Se o input não for zero, armazena a versão arredondada para a animação
        if (animationInput.sqrMagnitude > 0f)
        {
            lastMoveDir = animationInput; // lastMoveDir agora armazena (-1, 0, 1)
        }

        if (moveInput != Vector2.zero && !locked)
        {
            // Envia os valores arredondados para o Animator
            animator.SetFloat("XInput", lastMoveDir.x);
            animator.SetFloat("YInput", lastMoveDir.y);
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
        GetComponent<HealthManager>().TakeDamage(hp);
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
        
        if(onConveyorBelt) return;
        
        CancelInvoke("Unlock");
        Invoke("Unlock", getHitDelay);
    }

    // curar
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

        GetComponent<HealthManager>().Heal(hp);
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

    // esteira
    private bool onConveyorBelt = false;
    public IEnumerator EnterConveyorBelt(Direction direction, Vector3 center, Vector3 end, float conveyorSpeed)
    {
        onConveyorBelt = true;
        locked = true;
        CancelInvoke("Unlock");
        CancelInvoke("DealDamage");

        Vector3 targetPosition = transform.position;
        switch(direction)
        {
            case Direction.Up:
                while(transform.position.y < end.y)
                {
                    if(transform.position.x > center.x + 0.05f) targetPosition.x -= Time.deltaTime * conveyorSpeed;
                    else if(transform.position.x < center.x - 0.05f) targetPosition.x += Time.deltaTime * conveyorSpeed;

                    targetPosition.y += Time.deltaTime * conveyorSpeed;
                    transform.position = targetPosition;
                    yield return null;
                }
            break;
            case Direction.Down:
                while(transform.position.y > end.y)
                {            
                    if(transform.position.x > center.x + 0.05f) targetPosition.x -= Time.deltaTime * conveyorSpeed;
                    else if(transform.position.x < center.x - 0.05f) targetPosition.x += Time.deltaTime * conveyorSpeed;

                    targetPosition.y -= Time.deltaTime * conveyorSpeed;
                    transform.position = targetPosition;
                    yield return null;
                }
            break;
            case Direction.Right:
                while(transform.position.x < end.x)
                {
                    if(transform.position.y > center.y + 0.05f) targetPosition.y -= Time.deltaTime * conveyorSpeed;
                    else if(transform.position.y < center.y - 0.05f) targetPosition.y += Time.deltaTime * conveyorSpeed;

                    targetPosition.x += Time.deltaTime * conveyorSpeed;
                    transform.position = targetPosition;
                    yield return null;
                }
            break;
            case Direction.Left:
                while(transform.position.x > end.x)
                {
                    if(transform.position.y > center.y + 0.05f) targetPosition.y -= Time.deltaTime * conveyorSpeed;
                    else if(transform.position.y < center.y - 0.05f) targetPosition.y += Time.deltaTime * conveyorSpeed;

                    targetPosition.x -= Time.deltaTime * conveyorSpeed;
                    transform.position = targetPosition;
                    yield return null;
                }
            break;
        }

        onConveyorBelt = false;
        Unlock();
    }

    // hack
    private void OnHackPerformed(InputAction.CallbackContext context)
    {
        hack = true;
    }

    private void OnHackCanceled(InputAction.CallbackContext context)
    {
        hack = false;
    }

    // conversar
    private void OnSpeakPerformed(InputAction.CallbackContext context)
    {
        interact = true;
    }

    private void OnSpeakCanceled(InputAction.CallbackContext context)
    {
        interact = false;
    }

    void Unlock() => locked = false;
}
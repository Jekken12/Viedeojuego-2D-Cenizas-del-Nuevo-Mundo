using UnityEngine;
using UnityEngine.InputSystem;

public class JekkenMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 10f;

    [Header("Ataque")]
    public Transform attackPoint;
    public Vector2 attackSize = new Vector2(1.2f, 1f);
    public LayerMask enemyLayer;
    public int damage = 1;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float movimiento;
    private bool estaEnSuelo = true;
    private float attackPointX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackPointX = Mathf.Abs(attackPoint.localPosition.x);
    }

    void Update()
    {
        // MOVIMIENTO HORIZONTAL
        movimiento = 0f;

        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
        {
            movimiento = -1f;
        }

        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            movimiento = 1f;
        }

        // ANIMATOR
        animator.SetFloat("speed", Mathf.Abs(movimiento));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetBool("isGrounded", estaEnSuelo);

        // GIRAR PERSONAJE
        // GIRAR PERSONAJE Y ATTACK POINT
        if (movimiento > 0)
        {
            spriteRenderer.flipX = false;

            Vector3 pos = attackPoint.localPosition;
            pos.x = attackPointX;
            attackPoint.localPosition = pos;
        }
        else if (movimiento < 0)
        {
            spriteRenderer.flipX = true;

            Vector3 pos = attackPoint.localPosition;
            pos.x = -attackPointX;
            attackPoint.localPosition = pos;
        }

        // SALTO
        if (Keyboard.current.spaceKey.wasPressedThisFrame && estaEnSuelo)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaSalto
            );

            estaEnSuelo = false;

            animator.SetBool("isGrounded", false);
            animator.SetBool("isJumping", true);
        }

        // ATAQUE BÁSICO
        if (Keyboard.current.jKey.wasPressedThisFrame && estaEnSuelo)
        {
            // El daño lo aplica el Animation Event de Jekken_Attack1.anim.
            // No llamar Atacar() aquí: causaría un golpe doble.
            animator.SetTrigger("attack1");
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            movimiento * velocidad,
            rb.linearVelocity.y
        );
    }

    void Atacar()
    {
        Collider2D[] enemigos = Physics2D.OverlapBoxAll(
            attackPoint.position,
            attackSize,
            0f,
            enemyLayer
        );

        Debug.Log("Enemigos detectados: " + enemigos.Length);

        foreach (Collider2D enemigo in enemigos)
        {
            Debug.Log("Golpe detectado contra: " + enemigo.name);

            EnemyHealth vidaEnemigo = enemigo.GetComponent<EnemyHealth>();

            if (vidaEnemigo != null)
            {
                vidaEnemigo.RecibirDanio(damage);
            }
            else
            {
                Debug.Log("El objeto golpeado NO tiene EnemyHealth");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            estaEnSuelo = true;

            animator.SetBool("isGrounded", true);
            animator.SetBool("isJumping", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            estaEnSuelo = false;
            animator.SetBool("isGrounded", false);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireCube(
            attackPoint.position,
            attackSize
        );
    }
}
using UnityEngine;

public class MutantePesadoAI : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform objetivo;

    [Header("Movimiento")]
    public float velocidad = 2f;
    public float distanciaDeteccion = 8f;
    public float distanciaAtaque = 1.8f;

    [Header("Ataque")]
    public Transform attackPoint;
    public Vector2 attackSize = new Vector2(2f, 2f);
    public LayerMask playerLayer;
    public int damage = 1;
    public float tiempoEntreAtaques = 2f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float tiempoProximoAtaque;
    private bool muerto = false;

    private float attackPointX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (attackPoint != null)
            attackPointX = Mathf.Abs(attackPoint.localPosition.x);
    }

    void Update()
    {
        if (muerto || objetivo == null)
            return;

        float distancia = Mathf.Abs(
            objetivo.position.x - transform.position.x
        );

        // Fuera del rango de detección
        if (distancia > distanciaDeteccion)
        {
            Detenerse();
            return;
        }

        // Dentro del rango de ataque
        if (distancia <= distanciaAtaque)
        {
            Detenerse();
            MirarObjetivo();

            if (Time.time >= tiempoProximoAtaque)
            {
                Atacar();

                tiempoProximoAtaque =
                    Time.time + tiempoEntreAtaques;
            }

            return;
        }

        // Perseguir a Jekken
        Perseguir();
    }

    void Perseguir()
    {
        float direccion = Mathf.Sign(
            objetivo.position.x - transform.position.x
        );

        rb.linearVelocity = new Vector2(
            direccion * velocidad,
            rb.linearVelocity.y
        );

        animator.SetBool("isRunning", true);

        MirarObjetivo();
    }

    void MirarObjetivo()
    {
        if (objetivo.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false;

            Vector3 pos = attackPoint.localPosition;
            pos.x = attackPointX;
            attackPoint.localPosition = pos;
        }
        else
        {
            spriteRenderer.flipX = true;

            Vector3 pos = attackPoint.localPosition;
            pos.x = -attackPointX;
            attackPoint.localPosition = pos;
        }
    }

    void Detenerse()
    {
        rb.linearVelocity = new Vector2(
            0f,
            rb.linearVelocity.y
        );

        animator.SetBool("isRunning", false);
    }

    // SOLO INICIA LA ANIMACIÓN
    void Atacar()
    {
        animator.SetTrigger("attack");
    }

    // ESTE MÉTODO LO LLAMA EL ANIMATION EVENT
    public void AplicarDanio()
    {
        if (muerto || attackPoint == null)
            return;

        Collider2D[] jugadores = Physics2D.OverlapBoxAll(
            attackPoint.position,
            attackSize,
            0f,
            playerLayer
        );

        foreach (Collider2D jugador in jugadores)
        {
            JekkenHealth vida =
                jugador.GetComponent<JekkenHealth>();

            if (vida != null)
            {
                vida.RecibirDanio(damage);
            }
        }
    }

    public void Morir()
    {
        muerto = true;

        Detenerse();

        enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            distanciaDeteccion
        );

        if (attackPoint != null)
        {
            Gizmos.DrawWireCube(
                attackPoint.position,
                attackSize
            );
        }
    }
}
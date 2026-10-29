using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 3;

    private int vidaActual;
    private bool muerto = false;

    private Animator animator;
    private Collider2D col;
    private Rigidbody2D rb;

    void Awake()
    {
        vidaActual = vidaMaxima;

        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void RecibirDanio(int cantidad)
    {
        if (muerto)
            return;

        vidaActual -= cantidad;

        Debug.Log(name + " recibió daño. Vida: " + vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
        else
        {
            animator.SetTrigger("hurt");
        }
    }

    void Morir()
    {
        muerto = true;

        // Detener inmediatamente la IA para que no persiga ni ataque
        // durante la animación de muerte.
        MutantePesadoAI iaPesado = GetComponent<MutantePesadoAI>();
        if (iaPesado != null)
            iaPesado.Morir();

        animator.SetTrigger("death");

        // Evita que siga chocando con Jekken
        if (col != null)
            col.enabled = false;

        // Detiene movimiento
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // Destruir después de que termine la animación
        Destroy(gameObject, 1.5f);
    }
}
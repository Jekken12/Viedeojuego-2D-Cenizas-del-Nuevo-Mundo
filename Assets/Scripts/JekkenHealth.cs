using UnityEngine;

public class JekkenHealth : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 5;

    private int vidaActual;
    public int VidaActual => vidaActual;
    private bool muerto = false;

    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D col;
    private JekkenMovement movimiento;

    void Awake()
    {
        vidaActual = vidaMaxima;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        movimiento = GetComponent<JekkenMovement>();
    }

    public void RecibirDanio(int cantidad)
    {
        if (muerto)
            return;

        vidaActual -= cantidad;

        Debug.Log("Jekken recibió daño. Vida: " + vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
        else
        {
            animator.SetTrigger("hurt");
        }
    }


    public bool RecuperarVida(int cantidad)
    {
        if (muerto || vidaActual >= vidaMaxima || cantidad <= 0)
            return false;

        vidaActual = Mathf.Min(vidaActual + cantidad, vidaMaxima);

        Debug.Log("Jekken recuperó vida. Vida actual: " + vidaActual);

        return true;
    }


    void Morir()
    {
        muerto = true;

        animator.SetTrigger("death");

        if (movimiento != null)
            movimiento.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        if (col != null)
            col.enabled = false;

        Debug.Log("Jekken murió");
    }
}
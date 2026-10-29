using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Fase 2: Ataque 2 (K), Ulti (L) y carga de Nucleos.
/// Se agrega junto a JekkenMovement, sin sustituirlo.
/// </summary>
[RequireComponent(typeof(Animator))]
public class JekkenHabilidades : MonoBehaviour
{
    [Header("Ataque 2 - tecla K")]
    public Transform attackPoint; // Arrastrar el mismo AttackPoint de JekkenMovement
    public Vector2 attack2Size = new Vector2(2.2f, 1.6f);
    public LayerMask enemyLayer;
    [Min(1)] public int damageAtaque2 = 2;
    [Min(0.1f)] public float cooldownAtaque2 = 0.7f;

    [Header("Ulti - tecla L")]
    public Vector2 offsetCentroUlti = new Vector2(0f, 1f);
    [Min(0.1f)] public float radioUlti = 3.5f;
    [Min(1)] public int damageUlti = 4;
    [Min(1)] public int nucleosMaximos = 1;
    [SerializeField, Min(0)] private int nucleosActuales = 0;
    [Min(0.1f)] public float cooldownUlti = 1f;

    public int NucleosActuales => nucleosActuales;
    public int NucleosMaximos => nucleosMaximos;

    private Animator animator;
    private JekkenMovement movimiento;
    private float siguienteAtaque2;
    private float siguienteUlti;
    private bool ataque2Pendiente;
    private bool ultiPendiente;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movimiento = GetComponent<JekkenMovement>();
        nucleosActuales = Mathf.Clamp(nucleosActuales, 0, nucleosMaximos);
    }

    private void Update()
    {
        // JekkenHealth desactiva JekkenMovement al morir.
        if (Keyboard.current == null || movimiento == null || !movimiento.enabled)
            return;

        // Por ahora solo permitimos atacar desde el suelo, como Ataque 1.
        bool enSuelo = animator.GetBool("isGrounded");
        if (!enSuelo)
            return;

        if (Keyboard.current.kKey.wasPressedThisFrame &&
            Time.time >= siguienteAtaque2 && !ataque2Pendiente && !ultiPendiente)
        {
            ataque2Pendiente = true;
            siguienteAtaque2 = Time.time + cooldownAtaque2;
            animator.SetTrigger("attack2");
        }

        if (Keyboard.current.lKey.wasPressedThisFrame &&
            nucleosActuales > 0 &&
            Time.time >= siguienteUlti && !ultiPendiente && !ataque2Pendiente)
        {
            ultiPendiente = true;
            siguienteUlti = Time.time + cooldownUlti;
            animator.SetTrigger("ulti");
        }

        // Evita bloquear nuevas habilidades si faltó el evento en una animación.
        if (ataque2Pendiente && Time.time > siguienteAtaque2 + 0.2f)
            ataque2Pendiente = false;
        if (ultiPendiente && Time.time > siguienteUlti + 0.2f)
            ultiPendiente = false;
    }

    /// <summary>Llamar UNA VEZ mediante Animation Event de Ataque 2.</summary>
    public void AplicarAtaque2()
    {
        if (!ataque2Pendiente || !PuedeAtacar() || attackPoint == null)
            return;

        ataque2Pendiente = false;
        Collider2D[] encontrados = Physics2D.OverlapBoxAll(
            attackPoint.position, attack2Size, 0f, enemyLayer);
        DanarUnicos(encontrados, damageAtaque2);
    }

    /// <summary>Llamar UNA VEZ mediante Animation Event de Ulti.</summary>
    public void AplicarUlti()
    {
        if (!ultiPendiente || !PuedeAtacar() || nucleosActuales <= 0)
            return;

        ultiPendiente = false;
        nucleosActuales--;
        Vector2 centro = (Vector2)transform.position + offsetCentroUlti;
        Collider2D[] encontrados = Physics2D.OverlapCircleAll(
            centro, radioUlti, enemyLayer);
        DanarUnicos(encontrados, damageUlti);
    }

    /// <summary>Se llama desde NucleoRecogible cuando Jekken recoge un Núcleo.</summary>
    public bool RecogerNucleo()
    {
        if (!PuedeAtacar() || nucleosActuales >= nucleosMaximos)
            return false;

        nucleosActuales++;
        Debug.Log("Núcleo recogido: " + nucleosActuales + "/" + nucleosMaximos);
        return true;
    }

    private bool PuedeAtacar()
    {
        return movimiento != null && movimiento.enabled && enabled;
    }

    private static void DanarUnicos(Collider2D[] colliders, int cantidad)
    {
        var afectados = new HashSet<EnemyHealth>();
        foreach (Collider2D col in colliders)
        {
            if (col == null)
                continue;

            EnemyHealth vida = col.GetComponentInParent<EnemyHealth>();
            if (vida != null && afectados.Add(vida))
                vida.RecibirDanio(cantidad);
        }
    }

    private void OnDisable()
    {
        ataque2Pendiente = false;
        ultiPendiente = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(attackPoint.position, attack2Size);
        }
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere((Vector2)transform.position + offsetCentroUlti, radioUlti);
    }
}

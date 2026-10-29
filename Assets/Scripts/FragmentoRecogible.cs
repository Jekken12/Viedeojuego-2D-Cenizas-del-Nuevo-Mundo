
using UnityEngine;

public class FragmentoRecogible : MonoBehaviour
{
    private SistemaFragmentos sistemaFragmentos;
    private bool recogido = false;

    void Awake()
    {
        // Buscar automáticamente el sistema en la escena
        sistemaFragmentos =
            FindFirstObjectByType<SistemaFragmentos>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (recogido)
            return;

        JekkenHealth jekken =
            other.GetComponentInParent<JekkenHealth>();

        if (jekken == null)
            return;

        // Volver a buscar si no se encontró al iniciar
        if (sistemaFragmentos == null)
        {
            sistemaFragmentos =
                FindFirstObjectByType<SistemaFragmentos>();
        }

        if (sistemaFragmentos == null)
        {
            Debug.LogWarning(
                "No se encontró SistemaFragmentos en la escena"
            );
            return;
        }

        if (sistemaFragmentos.RecogerFragmento())
        {
            recogido = true;
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

/// <summary>Agrega este script al prefab del Núcleo, junto con Collider2D Is Trigger.</summary>
[RequireComponent(typeof(Collider2D))]
public class NucleoRecogible : MonoBehaviour
{
    public AudioClip sonidoRecoger; // Opcional, asignar AUDIO 10 si se desea.

    private void OnTriggerEnter2D(Collider2D otro)
    {
        JekkenHabilidades habilidades = otro.GetComponentInParent<JekkenHabilidades>();
        if (habilidades == null || !habilidades.RecogerNucleo())
            return;

        if (sonidoRecoger != null)
            AudioSource.PlayClipAtPoint(sonidoRecoger, transform.position);

        Destroy(gameObject);
    }
}

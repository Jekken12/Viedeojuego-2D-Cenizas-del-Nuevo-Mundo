
using UnityEngine;

public class VidaRecogible : MonoBehaviour
{
    [Header("Recuperación")]
    public int cantidadVida = 5;

    [Header("Audio")]
    public AudioClip sonidoRecoger;

    private bool recogido = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (recogido)
            return;

        JekkenHealth jekken =
            other.GetComponentInParent<JekkenHealth>();

        if (jekken == null)
            return;

        if (jekken.RecuperarVida(cantidadVida))
        {
            recogido = true;

            if (sonidoRecoger != null)
            {
                Vector3 posicionAudio =
                    Camera.main != null
                    ? Camera.main.transform.position
                    : transform.position;

                AudioSource.PlayClipAtPoint(
                    sonidoRecoger,
                    posicionAudio,
                    0.6f
                );
            }

            Destroy(gameObject);
        }
    }
}

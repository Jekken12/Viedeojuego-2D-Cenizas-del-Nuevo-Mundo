
using UnityEngine;
using UnityEngine.UI;

public class HUDVida : MonoBehaviour
{
    [Header("Referencias")]
    public JekkenHealth jekken;
    public Image rellenoVida;

    void Update()
    {
        if (jekken == null || rellenoVida == null)
            return;

        float porcentaje = (float)jekken.VidaActual /
                           Mathf.Max(1, jekken.vidaMaxima);

        rellenoVida.fillAmount = Mathf.Clamp01(porcentaje);
    }
}

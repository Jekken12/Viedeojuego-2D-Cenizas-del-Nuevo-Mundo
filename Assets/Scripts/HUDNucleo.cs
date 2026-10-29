
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDNucleo : MonoBehaviour
{
    [Header("Referencias")]
    public JekkenHabilidades habilidades;
    public Image rellenoNucleo;
    public TMP_Text textoNucleo;

    void Update()
    {
        if (habilidades == null)
            return;

        int actuales = habilidades.NucleosActuales;
        int maximos = Mathf.Max(1, habilidades.NucleosMaximos);

        if (rellenoNucleo != null)
        {
            rellenoNucleo.fillAmount =
                Mathf.Clamp01((float)actuales / maximos);
        }

        if (textoNucleo != null)
        {
            textoNucleo.text = actuales + "/" + maximos;
        }
    }
}

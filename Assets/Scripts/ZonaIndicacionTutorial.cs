
using UnityEngine;
using TMPro;

public class ZonaIndicacionTutorial : MonoBehaviour
{
    [TextArea(2, 4)]
    public string mensaje;

    public GameObject panelIndicaciones;
    public TMP_Text textoIndicacion;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<JekkenHealth>() == null)
            return;

        if (panelIndicaciones == null || textoIndicacion == null)
            return;

        textoIndicacion.text = mensaje;
        panelIndicaciones.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponentInParent<JekkenHealth>() == null)
            return;

        if (panelIndicaciones != null &&
            textoIndicacion != null &&
            textoIndicacion.text == mensaje)
        {
            panelIndicaciones.SetActive(false);
        }
    }
}

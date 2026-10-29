
using UnityEngine;
using TMPro;

public class SistemaFragmentos : MonoBehaviour
{
    [Header("Fragmentos")]
    public int totalFragmentos = 5;

    [Header("HUD")]
    public TMP_Text textoFragmentos;
    public TMP_Text textoMision;

    private int fragmentosActuales = 0;

    public int FragmentosActuales => fragmentosActuales;

    void Start()
    {
        ActualizarHUD();
    }

    public bool RecogerFragmento()
    {
        if (fragmentosActuales >= totalFragmentos)
            return false;

        fragmentosActuales++;
        ActualizarHUD();

        Debug.Log("Fragmentos: " +
            fragmentosActuales + "/" + totalFragmentos);

        return true;
    }

    void ActualizarHUD()
    {
        if (textoFragmentos != null)
            textoFragmentos.text =
                fragmentosActuales + "/" + totalFragmentos;

        if (textoMision != null)
        {
            if (fragmentosActuales >= totalFragmentos)
                textoMision.text =
                    "Regresa con Tara y Shuthing";
            else
                textoMision.text =
                    "Recoge los 5 fragmentos del mapa";
        }
    }
}


using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class FinalTutorial : MonoBehaviour
{
    public TMP_Text textoInteraccion;
    public string siguienteEscena = "Nivel_Zivil_01";

    private bool jugadorCerca = false;
    private int pasoDialogo = 0;

    void Update()
    {
        if (!jugadorCerca || Keyboard.current == null)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            pasoDialogo++;

            switch (pasoDialogo)
            {
                case 1:
                    MostrarTexto(
                        "Tara: Veo que ya puedes defenderte. " +
                        "Necesitamos tu ayuda.\n" +
                        "[E] Continuar"
                    );
                    break;

                case 2:
                    MostrarTexto(
                        "Shuthing: Hay cinco fragmentos de " +
                        "un mapa dispersos por Zivil. " +
                        "Encuéntralos y regresa con nosotros.\n" +
                        "[E] Comenzar Misión 1"
                    );
                    break;

                case 3:
                    SceneManager.LoadScene(siguienteEscena);
                    break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<JekkenHealth>() == null)
            return;

        jugadorCerca = true;
        pasoDialogo = 0;

        MostrarTexto("[E] Hablar con Tara y Shuthing");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponentInParent<JekkenHealth>() == null)
            return;

        jugadorCerca = false;
        pasoDialogo = 0;

        if (textoInteraccion != null)
            textoInteraccion.gameObject.SetActive(false);
    }

    void MostrarTexto(string mensaje)
    {
        if (textoInteraccion == null)
            return;

        textoInteraccion.gameObject.SetActive(true);
        textoInteraccion.text = mensaje;
    }
}

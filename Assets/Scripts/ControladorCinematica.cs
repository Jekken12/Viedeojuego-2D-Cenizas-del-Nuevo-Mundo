
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ControladorCinematica : MonoBehaviour
{
    [Header("Imagen de la cinemática")]
    public Image imagenCinematica;

    [Header("Hojas de la historia en orden")]
    public Sprite[] paginas;

    [Header("Escena al finalizar")]
    public string escenaSiguiente = "Nivel_Zivil_01";

    private int paginaActual = 0;
    private float tiempoInicio;

    void Start()
    {
        if (imagenCinematica == null ||
            paginas == null || paginas.Length == 0)
        {
            Debug.LogError("Faltan imágenes de la cinemática.");
            enabled = false;
            return;
        }

        paginaActual = 0;
        imagenCinematica.sprite = paginas[paginaActual];
        imagenCinematica.preserveAspect = true;

        tiempoInicio = Time.unscaledTime;
    }

    void Update()
    {
        // Evita que el clic del menú salte la primera hoja.
        if (Time.unscaledTime - tiempoInicio < 0.3f)
            return;

        bool espacio = Keyboard.current != null &&
                       Keyboard.current.spaceKey.wasPressedThisFrame;

        bool clic = Mouse.current != null &&
                    Mouse.current.leftButton.wasPressedThisFrame;

        if (espacio || clic)
            AvanzarPagina();
    }

    public void AvanzarPagina()
    {
        paginaActual++;

        if (paginaActual < paginas.Length)
        {
            imagenCinematica.sprite = paginas[paginaActual];
        }
        else
        {
            SceneManager.LoadScene(escenaSiguiente);
        }
    }
}

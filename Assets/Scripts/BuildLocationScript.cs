using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Este script maneja la lógica para interactuar con una ubicación de construcción en un juego,
// permitiendo al jugador abrir un menú de construcción al hacer clic en el objeto.
public class BuildLocationScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    // Variable estática para almacenar el canvas que está abierto actualmente.
    [HideInInspector]
    public static GameObject OpenCanvas;

    // Referencia al canvas asociado con este objeto.
    private GameObject canvas;

    // Referencia a la torreta que se puede construir (sin uso en este código).
    private GameObject turret;

    // Bandera para determinar si el objeto está presionado.
    private bool pressed = false;

    // Método Start se llama antes del primer frame.
    void Start()
    {
        // Encuentra el canvas hijo del objeto y lo desactiva.
        canvas = transform.Find("Canvas").gameObject;
        canvas.SetActive(false);
    }

    // Método llamado cuando se detecta un clic en el objeto.
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        // Si ya está presionado, no hacer nada.
        if (pressed) return;

        // Si hay un canvas abierto, cerrarlo.
        if (OpenCanvas != null) OpenCanvas.SetActive(false);

        // Mueve el objeto hacia abajo para indicar que está presionado.
        gameObject.transform.Translate(0, -3f, 0);
        pressed = true;
    }

    // Método llamado cuando el puntero sale del objeto.
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        // Si no está presionado, no hacer nada.
        if (!pressed) return;

        // Mueve el objeto hacia arriba para revertir la acción de presionar.
        gameObject.transform.Translate(0, 3f, 0);
        pressed = false;
    }

    // Método llamado cuando se suelta el clic en el objeto.
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        // Si no está presionado, no hacer nada.
        if (!pressed) return;

        // Cambiar el estado de presionado a falso y mover el objeto hacia arriba.
        pressed = false;
        gameObject.transform.Translate(0, 3f, 0);

        // Activar el canvas asociado con el objeto.
        canvas.SetActive(true);

        // Actualizar la referencia al canvas abierto.
        OpenCanvas = canvas;
    }
}

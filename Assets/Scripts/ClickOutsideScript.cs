using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Este script maneja la lógica para cerrar un canvas abierto al hacer clic fuera de él.
public class ClickOutsideScript : MonoBehaviour, IPointerDownHandler
{
    // Método llamado cuando se detecta un clic en el objeto que implementa esta interfaz.
    public void OnPointerDown(PointerEventData eventData)
    {
        // Verifica si hay algún canvas abierto y lo desactiva.
        if (BuildLocationScript.OpenCanvas != null)
        {
            BuildLocationScript.OpenCanvas.SetActive(false);
        }
    }
}

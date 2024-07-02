using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScript : MonoBehaviour
{
    public Vector2 ShadowOffset;  // Desplazamiento del sombreado relativo al avión

    private Transform shadow;     // Referencia al objeto de sombra
    private Quaternion prevRotation = Quaternion.identity;  // Rotación previa del avión

    void Start()
    {
        shadow = transform.Find("Shadow");  // Encuentra el objeto sombra bajo la jerarquía del avión
    }

    void FixedUpdate()
    {
        shadow.position = transform.position + (Vector3)ShadowOffset;  // Establece la posición de la sombra relativa al avión
        shadow.rotation = transform.rotation;  // Alinea la rotación de la sombra con la rotación del avión

        prevRotation = transform.rotation;  // Actualiza la rotación previa con la rotación actual del avión
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingRotator : MonoBehaviour
{

    public float Amount;  // Amplitud de la rotación
    public float Speed;   // Velocidad de la rotación

    private float t;      // Parámetro de tiempo

    void FixedUpdate()
    {
        t += Time.deltaTime;  // Incrementa el parámetro de tiempo basado en el tiempo transcurrido
        transform.Rotate(0, 0, Mathf.Sin(t * Speed) * Amount);  // Aplica una rotación oscilante en el eje Z usando una función sinusoidal
    }
}

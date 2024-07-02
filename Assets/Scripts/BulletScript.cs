using UnityEngine;

using UnityEngine;
using Assets.Scripts;

// Este script maneja la lógica para las balas en el juego, controlando su movimiento y desactivación.
public class BulletScript : FlyingShotScript
{
    // Distancia recorrida por la bala.
    private float distance;

    // Método Start se llama antes del primer frame.
    void Start()
    {
        // Inicialización de la bala.
    }

    // Método llamado cuando el objeto se habilita.
    void OnEnable()
    {
        // Resetea la distancia recorrida.
        distance = 0.0f;

        // Activa el efecto de sonido del rifle.
        Pool.Instance.ActivateObject("rifleSoundEffect").SetActive(true);
    }

    // Método FixedUpdate se llama a intervalos fijos y es utilizado para actualizar física.
    void FixedUpdate()
    {
        // Calcula la distancia que la bala debe recorrer en este frame.
        var diff = Time.deltaTime * Speed;

        // Acumula la distancia recorrida.
        distance += diff;

        // Mueve la bala en la dirección especificada.
        transform.position += (Vector3)Direction * diff;

        // Si la bala ha recorrido más distancia de la permitida, desactívala.
        if (distance > Range)
        {
            Pool.Instance.DeactivateObject(gameObject);
        }
    }
}

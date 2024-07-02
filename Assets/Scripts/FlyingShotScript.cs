using System.Collections.Generic;
using UnityEngine;

// Clase base abstracta para proyectiles en movimiento.
public abstract class FlyingShotScript : MonoBehaviour
{
    [System.NonSerialized] public float Speed;        // Velocidad del proyectil.
    [System.NonSerialized] public Vector2 Direction;  // Dirección hacia la que se mueve el proyectil.
    [System.NonSerialized] public GameObject Target;  // Objetivo del proyectil.
    [System.NonSerialized] public float Range;        // Alcance máximo del proyectil.
    [System.NonSerialized] public float Damage;       // Daño infligido por el proyectil.
    [System.NonSerialized] public List<string> EnemyTags; // Etiquetas de enemigos que puede afectar el proyectil.
    [System.NonSerialized] public Transform Turret;   // Torreta que disparó el proyectil.

    // Método virtual para ejecutar la acción de "explotar" el proyectil.
    public virtual void BlowUp()
    {
        Pool.Instance.DeactivateObject(gameObject); // Desactiva el objeto del pool.
    }
}

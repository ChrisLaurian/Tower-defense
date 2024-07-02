using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Este script gestiona y controla los enemigos en el juego.
public class EnemyManagerScript : MonoBehaviour
{
    // Instancia estática del EnemyManagerScript.
    private static EnemyManagerScript enemyManagerInstance;

    // Propiedad para obtener la instancia única del EnemyManagerScript.
    public static EnemyManagerScript Instance
    {
        get
        {
            if (enemyManagerInstance == null)
                enemyManagerInstance = FindObjectOfType<EnemyManagerScript>();

            return enemyManagerInstance;
        }
    }

    // Clase privada que representa un par de enemigo y distancia.
    private class EnemyDistancePair
    {
        public GameObject Enemy; // Objeto del enemigo.
        public float Distance;   // Distancia del enemigo desde algún punto.

        // Constructor que inicializa el par de enemigo y distancia.
        public EnemyDistancePair(GameObject enemy, float distance)
        {
            Enemy = enemy;
            Distance = distance;
        }
    }

    // Diccionario que mapea cada enemigo a su respectivo par de enemigo y distancia.
    private Dictionary<GameObject, EnemyDistancePair> enemies = new Dictionary<GameObject, EnemyDistancePair>();

    // Método para registrar un nuevo enemigo.
    public void RegisterEnemy(GameObject enemy)
    {
        enemies.Add(enemy, new EnemyDistancePair(enemy, 0));
    }

    // Método para eliminar un enemigo registrado.
    public void DeleteEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    // Método para actualizar la distancia de un enemigo.
    public void UpdateEnemy(GameObject enemy, float distance)
    {
        enemies[enemy].Distance = distance;
    }

    // Método para obtener un enemigo dentro de un rango específico y con etiquetas especificadas.
    public GameObject GetEnemyInRange(Vector2 position, float range, IEnumerable<string> enemyTags)
    {
        return enemies.Values
            .Where(e => ((Vector2)e.Enemy.transform.position - position).sqrMagnitude < range * range && enemyTags.Any(t => e.Enemy.CompareTag(t)))
            .OrderBy(e => e.Distance)
            .Select(e => e.Enemy)
            .FirstOrDefault();
    }

    // Método para obtener el enemigo más cercano dentro de un rango específico y con etiquetas especificadas.
    public GameObject GetClosestEnemyInRange(Vector2 position, float range, IEnumerable<string> enemyTags)
    {
        return enemies.Values
            .Where(e => ((Vector2)e.Enemy.transform.position - position).sqrMagnitude < range * range && enemyTags.Any(t => e.Enemy.CompareTag(t)))
            .OrderBy(e => ((Vector2)e.Enemy.transform.position - position).sqrMagnitude)
            .Select(e => e.Enemy)
            .FirstOrDefault();
    }
}

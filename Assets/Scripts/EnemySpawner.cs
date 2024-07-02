using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script maneja la generación de oleadas de enemigos.
public class EnemySpawner : MonoBehaviour
{
    // Clase Serializable que define una oleada de enemigos.
    [System.Serializable]
    public class Wave
    {
        public int Amount;       // Cantidad de enemigos en esta oleada.
        public GameObject Enemy; // Prefab del enemigo a generar.
        public float SpawnTime;  // Tiempo entre la generación de cada enemigo.
        public float RestTime;   // Tiempo de descanso antes de la siguiente oleada.
    }

    public List<Wave> Waves;    // Lista de oleadas de enemigos configuradas.
    private int waveIndex = 0;  // Índice de la oleada actual.
    private Wave currentWave;   // Referencia a la oleada actual.
    private float spawnTime = 2.0f; // Tiempo entre generaciones de enemigos.

    // Método llamado cuando el objeto se activa.
    void OnEnable()
    {
        currentWave = Waves[0]; // Inicializa la primera oleada.
    }

    // Método llamado en cada frame.
    void Update()
    {
        // Si ya se alcanzó el número máximo de oleadas, termina la función.
        if (waveIndex >= Waves.Count) return;

        // Si la oleada actual tiene tiempo de descanso, reduce el tiempo restante.
        if (currentWave.RestTime >= 0)
        {
            currentWave.RestTime -= Time.deltaTime;
            return;
        }

        // Si la cantidad de enemigos en la oleada actual es cero, reduce el tiempo de descanso.
        if (currentWave.Amount <= 0)
        {
            currentWave.RestTime -= Time.deltaTime;
            return;
        }

        // Si el tiempo entre generaciones de enemigos ha pasado, genera un nuevo enemigo.
        if (spawnTime < 0)
        {
            Spawn(currentWave.Enemy); // Genera un enemigo.
            spawnTime = currentWave.SpawnTime; // Reinicia el tiempo de generación.
            currentWave.Amount--; // Reduce la cantidad de enemigos restantes en la oleada.
            return;
        }

        spawnTime -= Time.deltaTime; // Reduce el tiempo entre generaciones de enemigos.
    }

    // Método para generar un enemigo en la escena.
    private void Spawn(GameObject prototype)
    {
        var spawnedEnemy = Pool.Instance.ActivateObject(prototype.tag); // Activa un objeto del pool.
        spawnedEnemy.SetActive(true); // Activa el objeto en la escena.

        EnemyManagerScript.Instance.RegisterEnemy(spawnedEnemy); // Registra el enemigo en el EnemyManager.
    }
}


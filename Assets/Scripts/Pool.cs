using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    private static Pool poolInstance;
    public static Pool Instance
    {
        get
        {
            if (poolInstance == null)
                poolInstance = FindObjectOfType<Pool>();  // Encuentra la instancia única del Pool en la escena

            return poolInstance;
        }
    }

    private class SinglePool
    {
        public HashSet<GameObject> Active;  // Conjunto de objetos activos
        public Queue<GameObject> Available;  // Cola de objetos disponibles

        public SinglePool()
        {
            Available = new Queue<GameObject>();  // Inicializa la cola de objetos disponibles
        }
    }

    public int StartCapacity = 10;  // Capacidad inicial para cada pool
    public GameObject[] PooledObjects;  // Arreglo de GameObjects que pueden ser poolizados

    private Dictionary<string, GameObject> pooledObjects = new Dictionary<string, GameObject>();  // Diccionario de GameObjects poolizados
    private Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();  // Diccionario que contiene las colas de objetos poolizados

    private void Start()
    {
        foreach (var item in PooledObjects)
        {
            RegisterObject(item);  // Registra cada objeto especificado en PooledObjects al iniciar
        }
    }

    public void RegisterObject(GameObject prototype)
    {
        if (pooledObjects.ContainsKey(prototype.tag)) return;  // Evita registrar el mismo tipo de objeto más de una vez

        var singlePool = new Queue<GameObject>();  // Crea una nueva cola para el pool de este tipo de objeto
        for (var i = 0; i < StartCapacity; i++)
        {
            var newItem = Instantiate(prototype, transform);  // Instancia un nuevo GameObject
            newItem.SetActive(false);  // Lo desactiva inicialmente
            singlePool.Enqueue(newItem);  // Lo añade a la cola de objetos disponibles
        }

        pooledObjects.Add(prototype.tag, prototype);  // Añade el prototipo al diccionario de prototipos
        pool.Add(prototype.tag, singlePool);  // Añade la cola al diccionario de pools
    }

    public GameObject ActivateObject(string tag)
    {
        if (!pooledObjects.ContainsKey(tag))
            throw new KeyNotFoundException();  // Lanza una excepción si la etiqueta del objeto no está registrada en el pool

        var singlePool = pool[tag];  // Obtiene la cola de objetos asociada a la etiqueta

        if (singlePool.Count == 0)
        {
            var newItem = Instantiate(pooledObjects[tag], transform);  // Si no hay objetos disponibles, instancia uno nuevo
            return newItem;
        }

        var item = singlePool.Dequeue();  // Obtiene el primer objeto disponible de la cola

        return item;
    }

    public void DeactivateObject(GameObject item)
    {
        if (!pooledObjects.ContainsKey(item.tag))
            throw new KeyNotFoundException();  // Lanza una excepción si la etiqueta del objeto no está registrada en el pool

        var singlePool = pool[item.tag];  // Obtiene la cola de objetos asociada a la etiqueta

        item.SetActive(false);  // Desactiva el objeto
        singlePool.Enqueue(item);  // Lo devuelve a la cola de objetos disponibles
    }
}

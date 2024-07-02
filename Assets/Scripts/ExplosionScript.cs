using UnityEngine;

// Este script controla el comportamiento de una explosión mediante un sistema de partículas.
public class ExplosionScript : MonoBehaviour
{
    private ParticleSystem particleSystem; // Referencia al componente ParticleSystem.

    // Método llamado al despertar del objeto.
    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>(); // Obtiene el componente ParticleSystem.
    }

    // Método llamado cuando el objeto se activa.
    void OnEnable()
    {
        particleSystem.Play(); // Inicia la reproducción del sistema de partículas.
    }

    // Método llamado en cada frame.
    void Update()
    {
        // Si las partículas han dejado de reproducirse, desactiva el objeto del pool.
        if (!particleSystem.isPlaying)
        {
            Pool.Instance.DeactivateObject(gameObject);
        }
    }
}

using UnityEngine;

// Este script desactiva automáticamente el objeto después de un tiempo especificado.
public class AutoDisableScript : MonoBehaviour
{
    // Tiempo en segundos antes de que el objeto se desactive.
    public float TimeOut;

    // Tiempo restante antes de desactivar el objeto.
    private float remainingTime;

    // Método que se llama cuando el objeto se habilita.
    void OnEnable()
    {
        // Inicializa el tiempo restante con el valor de TimeOut.
        remainingTime = TimeOut;
    }

    // Método que se llama una vez por frame.
    void Update()
    {
        // Si el tiempo restante es menor a 0, desactiva el objeto.
        if (remainingTime < 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            // Decrementa el tiempo restante con el tiempo transcurrido desde el último frame.
            remainingTime -= Time.deltaTime;
        }
    }
}

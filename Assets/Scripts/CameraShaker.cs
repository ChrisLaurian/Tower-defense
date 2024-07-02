using UnityEngine;

// Este script maneja la lógica de sacudida de la cámara en el juego.
public class CameraShaker : MonoBehaviour
{
    // Velocidad de la sacudida.
    public float Speed;

    // Intensidad de la sacudida.
    public float Amount;

    // Factor de decaimiento de la sacudida.
    public float Decay;

    // Fuerza actual de la sacudida.
    private float strength;

    // Posición original de la cámara.
    private Vector3 originalPosition;

    // Instancia estática del CameraShaker.
    private static CameraShaker cameraShakerInstance;

    // Propiedad para obtener la instancia única de CameraShaker.
    public static CameraShaker Instance
    {
        get
        {
            if (cameraShakerInstance == null)
                cameraShakerInstance = FindObjectOfType<CameraShaker>();

            return cameraShakerInstance;
        }
    }

    // Método llamado cuando el objeto se habilita.
    void OnEnable()
    {
        // Guarda la posición original de la cámara.
        originalPosition = transform.position;
    }

    // Método Update se llama una vez por frame.
    void Update()
    {
        // Si la fuerza de la sacudida es muy baja, restaura la posición original de la cámara y termina.
        if (strength < 0.001)
        {
            transform.position = originalPosition;
            return;
        }

        // Calcula nuevas posiciones x e y basadas en funciones sinusoidales para crear el efecto de sacudida.
        var a = Mathf.Sin(Speed * 11 * Time.realtimeSinceStartup + 1);
        var b = Mathf.Sin(Speed * 17 * Time.realtimeSinceStartup + 2);
        var c = Mathf.Sin(Speed * 13 * Time.realtimeSinceStartup + 3);
        var d = Mathf.Sin(Speed * 19 * Time.realtimeSinceStartup + 5);

        var x = originalPosition.x + strength * (a + b);
        var y = originalPosition.y + strength * (c + d);

        // Actualiza la posición de la cámara.
        transform.position = new Vector3(x, y, originalPosition.z);

        // Reduce la fuerza de la sacudida basándose en el factor de decaimiento.
        strength *= Decay;
    }

    // Método público para iniciar la sacudida de la cámara.
    public void Shake()
    {
        strength = Amount;
    }
}

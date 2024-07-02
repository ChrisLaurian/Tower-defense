using UnityEngine;

public class SoundEffectScript : MonoBehaviour
{
    private AudioSource audio;  // Referencia al componente AudioSource para reproducir sonidos

    void Awake()
    {
        audio = GetComponent<AudioSource>();  // Obtiene el componente AudioSource del objeto al que está adjunto
    }

    void OnEnable()
    {
        audio.Play();  // Cuando el objeto se activa, reproduce el sonido asociado
    }

    void Update()
    {
        if (!audio.isPlaying)  // Verifica si el sonido ha dejado de reproducirse
        {
            Pool.Instance.DeactivateObject(gameObject);  // Desactiva el objeto actual usando el Pool
        }
    }
}

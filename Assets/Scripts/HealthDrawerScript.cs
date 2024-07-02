using UnityEngine;
using UnityEngine.UI;

public class HealthDrawerScript : MonoBehaviour
{
    public Sprite FullHeart;         // Sprite para el corazón lleno.
    public Sprite EmptyHeart;        // Sprite para el corazón vacío.

    public GameObject[] HeartObjects;    // Arreglo de objetos GameObject que representan los corazones.
    private Image[] Hearts;               // Arreglo de componentes Image para los corazones.

    // Método para dibujar los corazones según el número de vidas.
    public void Draw(int lives)
    {
        // Dibuja los corazones llenos hasta el número de vidas actual.
        for (int i = 0; i < lives; i++)
        {
            Hearts[i].sprite = FullHeart;
        }

        // Dibuja los corazones restantes como vacíos.
        for (int i = lives; i < Hearts.Length; i++)
        {
            Hearts[i].sprite = EmptyHeart;
        }
    }

    // Método llamado al activar el script.
    void OnEnable()
    {
        Hearts = new Image[HeartObjects.Length]; // Inicializa el arreglo Hearts con el tamaño de HeartObjects.

        // Asigna los componentes Image de los GameObjects HeartObjects a Hearts.
        for (int i = 0; i < HeartObjects.Length; i++)
        {
            Hearts[i] = HeartObjects[i].GetComponent<Image>();
        }

        // Dibuja inicialmente todos los corazones llenos.
        Draw(Hearts.Length);
    }
}

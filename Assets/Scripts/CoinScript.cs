using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script maneja el comportamiento de una moneda en el juego.
public class CoinScript : MonoBehaviour
{
    // Valor constante de la moneda.
    public const int Value = 10;

    // Número aleatorio utilizado para variar la escala de la moneda.
    private float randomNumber;

    // Método Start se llama antes del primer frame.
    void Start()
    {
        // Inicializa el número aleatorio para variar la escala de la moneda.
        randomNumber = Random.value * 5;
    }

    // Método Update se llama una vez por frame.
    void Update()
    {
        // Calcula la escala basada en una función sinusoidal para dar efecto de escalado pulsante.
        var scale = 1.0f + 0.2f * Mathf.Sin(5 * Time.realtimeSinceStartup + randomNumber);
        transform.localScale = new Vector3(scale, scale, 1.0f);
    }

    // Método llamado cuando el mouse pasa sobre la moneda.
    void OnMouseOver()
    {
        // Informa al GameManager que la moneda ha sido recolectada.
        GameManager.Instance.CoinCollected(gameObject);

        // Desactiva la moneda utilizando el Pooling.
        Pool.Instance.DeactivateObject(gameObject);

        // Activa el efecto de sonido de la moneda.
        Pool.Instance.ActivateObject("coinSoundEffect").SetActive(true);
    }
}

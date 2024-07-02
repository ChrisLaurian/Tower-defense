using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Este script maneja el comportamiento de los enemigos en el juego.
public class EnemyScript : MonoBehaviour
{
    // Salud máxima del enemigo.
    public float MaxHealth;

    // Dinero que el enemigo otorga al ser derrotado.
    public int Money;

    // Objeto de la moneda que el enemigo suelta al ser derrotado.
    public GameObject Coin;

    // Media y desviación estándar para la generación aleatoria de monedas.
    public float SpawnedCoinMean;
    public float SpawnedCoinStd;

    // Referencia al canvas del enemigo para mostrar la barra de salud.
    private Transform canvas;

    // Referencia a la barra de salud del enemigo.
    private Slider healthBar;

    // Salud actual del enemigo.
    private float health;

    // Método llamado cuando el objeto se activa.
    private void OnEnable()
    {
        // Encuentra y asigna el canvas y la barra de salud.
        canvas = transform.Find("Canvas");
        healthBar = canvas.Find("HealthBar").GetComponent<Slider>();
        canvas.gameObject.SetActive(false);

        // Inicializa la salud del enemigo.
        health = MaxHealth;
        healthBar.maxValue = MaxHealth;
        healthBar.value = health;
    }

    // Método llamado en cada frame.
    private void Update()
    {
        // Ajusta la rotación y escala del canvas para mantenerlo constante.
        canvas.rotation = Quaternion.identity;
        canvas.localScale = Vector3.one * 0.5f;
    }

    // Método para generar monedas cuando el enemigo es derrotado.
    private void SpawnCoins()
    {
        // Calcula el número de monedas a generar con una distribución gaussiana.
        var num = (int)(MathHelpers.NextGaussianDouble() * SpawnedCoinStd + SpawnedCoinMean + 0.5f);

        // Genera las monedas en posiciones aleatorias alrededor del enemigo.
        for (int i = 0; i < num; i++)
        {
            var x = MathHelpers.NextGaussianDouble() * Mathf.Log(i + 1) * 4.0f;
            var y = MathHelpers.NextGaussianDouble() * Mathf.Log(i + 1) * 4.0f;

            var coin = Pool.Instance.ActivateObject(Coin.tag);
            coin.transform.position = transform.position + new Vector3(x, y, 0);
            coin.SetActive(true);
        }

        // Añade el dinero al GameManager.
        GameManager.Instance.AddMoney(Money);
    }

    // Método llamado cuando el enemigo colisiona con otro collider.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeSelf) return;

        // Si el enemigo llega al final (finish), se escapa.
        if (collision.CompareTag("finish"))
        {
            GameManager.Instance.EnemyEscaped(gameObject);
        }
        // Si el enemigo es golpeado por una bala o un misil, recibe daño.
        else if ((collision.CompareTag("bullet") && !CompareTag("plane")) || (collision.CompareTag("rocket") && !CompareTag("soldier")))
        {
            var flyingShot = collision.gameObject.GetComponent<FlyingShotScript>();
            var damage = flyingShot.Damage;
            health -= damage;
            healthBar.value = health;
            canvas.gameObject.SetActive(true);
            flyingShot.BlowUp();

            // Si la salud del enemigo llega a cero, es derrotado.
            if (health <= 0)
            {
                // Reproduce efectos de sonido y partículas según el tipo de enemigo.
                if (CompareTag("plane") || CompareTag("tank"))
                {
                    Pool.Instance.ActivateObject("bigExplosionSoundEffect").SetActive(true);
                    var explosion = Pool.Instance.ActivateObject("explosionParticle");
                    explosion.transform.position = transform.position;
                    explosion.SetActive(true);
                }

                // Genera monedas y notifica al GameManager que el enemigo ha sido derrotado.
                SpawnCoins();
                GameManager.Instance.EnemyKilled(gameObject);

                // Desactiva el objeto del enemigo y lo elimina del EnemyManager.
                Pool.Instance.DeactivateObject(gameObject);
                EnemyManagerScript.Instance.DeleteEnemy(gameObject);
            }
        }
    }

    // Método llamado cuando el enemigo sale de otro collider.
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Si el enemigo sale del final, se considera que escapó.
        if (collision.tag == "finish")
        {
            EnemyManagerScript.Instance.DeleteEnemy(gameObject);
            Pool.Instance.DeactivateObject(gameObject);
        }
    }
}

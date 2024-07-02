using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Clase encargada de gestionar el flujo y la lógica del juego.
public class GameManager : MonoBehaviour
{
    private static GameManager gameManagerInstance;
    public static GameManager Instance
    {
        get
        {
            if (gameManagerInstance == null)
                gameManagerInstance = FindObjectOfType<GameManager>();

            return gameManagerInstance;
        }
    }

    public const int MaxLives = 5;             // Número máximo de vidas del jugador.
    public int InitialMoney;                   // Dinero inicial del jugador.

    public int Level;                          // Nivel actual del juego.
    public GameObject VictoryText;             // Objeto de texto para mostrar victoria.
    public GameObject GameOverText;            // Objeto de texto para mostrar game over.

    public int InitialTurretPrice;             // Precio inicial de la torreta.
    public int InitialRocketPrice;             // Precio inicial del cohete.
    public int TurretPriceAddition;            // Incremento de precio de la torreta.
    public int RocketPriceAddition;            // Incremento de precio del cohete.

    private int turretPrice;                   // Precio actual de la torreta.
    private int rocketPrice;                   // Precio actual del cohete.

    public static int Lives;                   // Vidas restantes del jugador.
    private int money;                         // Dinero actual del jugador.
    private HealthDrawerScript healthDrawer;    // Script para dibujar la barra de salud.
    private MoneyDrawer moneyDrawer;           // Script para dibujar el dinero.

    private int remainingEnemies;              // Enemigos restantes por derrotar.

    // Método llamado al inicio del juego.
    void Start()
    {
        money = InitialMoney;

        turretPrice = InitialTurretPrice;
        rocketPrice = InitialRocketPrice;

        healthDrawer = GetComponent<HealthDrawerScript>();
        moneyDrawer = GetComponent<MoneyDrawer>();

        moneyDrawer.Draw(InitialMoney); // Dibuja la cantidad inicial de dinero.

        // Calcula la cantidad total de enemigos que deben ser derrotados en la oleada actual.
        remainingEnemies = GetComponent<EnemySpawner>().Waves.Sum(w => w.Amount);
    }

    // Método llamado cuando un enemigo escapa del mapa.
    public void EnemyEscaped(GameObject enemy)
    {
        Lives--; // Reduce las vidas del jugador.
        CameraShaker.Instance.Shake(); // Activa la sacudida de cámara.
        healthDrawer.Draw(Lives); // Actualiza la barra de salud en pantalla.

        // Verifica si las vidas llegaron a cero.
        if (Lives <= 0)
        {
            GameOver(); // Llama al método de game over.
        }

        remainingEnemies--; // Reduce el contador de enemigos restantes.

        // Verifica si no quedan más enemigos por derrotar.
        if (remainingEnemies == 0)
        {
            Victory(); // Llama al método de victoria.
        }
    }

    // Método llamado cuando un enemigo es derrotado.
    public void EnemyKilled(GameObject enemy)
    {
        remainingEnemies--; // Reduce el contador de enemigos restantes.

        // Verifica si no quedan más enemigos por derrotar.
        if (remainingEnemies == 0)
        {
            Victory(); // Llama al método de victoria.
        }
    }

    // Obtiene la cantidad actual de dinero del jugador.
    public int GetMoney()
    {
        return money;
    }

    // Añade una cantidad específica de dinero al total actual.
    public void AddMoney(int value)
    {
        money += value; // Incrementa el dinero.
        moneyDrawer.Draw(money); // Actualiza el dinero mostrado en pantalla.
    }

    // Método llamado cuando se construye una torreta.
    public void TurretBuilt(GameObject turret)
    {
        if (turret.CompareTag("turretTower"))
        {
            money -= turretPrice; // Reduce el dinero según el precio de la torreta actual.
            turretPrice += TurretPriceAddition; // Aumenta el precio de la torreta para la próxima compra.
        }
        else
        {
            money -= rocketPrice; // Reduce el dinero según el precio del cohete actual.
            rocketPrice += RocketPriceAddition; // Aumenta el precio del cohete para la próxima compra.
        }

        moneyDrawer.Draw(money); // Actualiza el dinero mostrado en pantalla.
    }

    // Método llamado cuando se recoge una moneda.
    public void CoinCollected(GameObject coin)
    {
        money += CoinScript.Value; // Añade el valor de la moneda al dinero del jugador.
        moneyDrawer.Draw(money); // Actualiza el dinero mostrado en pantalla.
    }

    // Verifica si hay suficiente dinero para construir una torreta específica.
    public bool EnoughMoneyForTurret(string tag)
    {
        if (tag == "turretTower")
            return money >= turretPrice; // Retorna verdadero si el dinero es suficiente para una torreta.

        return money >= rocketPrice; // Retorna verdadero si el dinero es suficiente para un cohete.
    }

    // Retorna el precio de una torreta o cohete específico.
    public int MoneyForTurret(string tag)
    {
        return tag == "turretTower" ? turretPrice : rocketPrice; // Retorna el precio actual de la torreta o cohete.
    }

    // Método llamado al alcanzar la condición de victoria.
    public void Victory()
    {
        VictoryText.SetActive(true); // Activa el texto de victoria en pantalla.
        Invoke("NextLevel", 5.0f); // Llama al siguiente nivel después de 5 segundos.
    }

    // Método llamado al alcanzar la condición de game over.
    public void GameOver()
    {
        GameOverText.SetActive(true); // Activa el texto de game over en pantalla.
        Invoke("BackToMainMenu", 5.0f); // Regresa al menú principal después de 5 segundos.
    }

    // Carga el siguiente nivel del juego.
    public void NextLevel()
    {
        if (Level <= 2)
        {
            SceneManager.LoadScene("Level_0" + (Level + 1)); // Carga el siguiente nivel si está disponible.
        }
        else
        {
            SceneManager.LoadScene("Menu_screen"); // Regresa al menú principal si no hay más niveles.
        }
    }

    // Regresa al menú principal del juego.
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu_screen"); // Carga la escena del menú principal.
    }
}

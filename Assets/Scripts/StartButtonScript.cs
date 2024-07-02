using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonScript : MonoBehaviour
{
    public GameObject LoadingScreen;       // Referencia al objeto de pantalla de carga
    public GameObject MainMenuScreen;      // Referencia al objeto de pantalla principal
    public GameObject SelectLevelScreen;   // Referencia al objeto de pantalla de selección de nivel
    public int Level;                      // Número de nivel a iniciar

    private void Start()
    {
        LoadingScreen.SetActive(false);   // Al inicio, desactiva la pantalla de carga
    }

    // Método llamado al presionar el botón "Start Game"
    public void StartGame()
    {
        LoadingScreen.SetActive(true);    // Activa la pantalla de carga
        GameManager.Lives = GameManager.MaxLives;  // Reinicia las vidas del jugador desde el GameManager
        SceneManager.LoadScene("Level_01");  // Carga el primer nivel del juego
    }

    // Método llamado al presionar el botón "Select Level"
    public void SelectLevel()
    {
        MainMenuScreen.SetActive(false);  // Oculta la pantalla principal
        GameManager.Lives = GameManager.MaxLives;  // Reinicia las vidas del jugador desde el GameManager
        SelectLevelScreen.SetActive(true);  // Muestra la pantalla de selección de nivel
    }

    // Método llamado al presionar el botón "Back to Main Menu" desde la pantalla de selección de nivel
    public void BackToMainMenu()
    {
        SelectLevelScreen.SetActive(false);  // Oculta la pantalla de selección de nivel
        MainMenuScreen.SetActive(true);      // Muestra la pantalla principal
    }

    // Método llamado al presionar el botón "Exit" para salir del juego
    public void Exit()
    {
        Application.Quit();  // Cierra la aplicación (solo funciona en compilaciones standalone)
    }

    // Método llamado al presionar el botón "Start Level" desde la pantalla de selección de nivel
    public void StartLevel()
    {
        LoadingScreen.SetActive(true);    // Activa la pantalla de carga
        SceneManager.LoadScene("Level_0" + Level);  // Carga el nivel seleccionado por el jugador
    }
}

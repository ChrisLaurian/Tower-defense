using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButtonScript : MonoBehaviour
{
    public GameObject PauseButton;  // Referencia al botón que activa la pausa
    public GameObject PausePanel;   // Referencia al panel que se muestra cuando el juego está en pausa

    // Método llamado cuando se hace clic en el botón de Pausa
    public void PauseGame()
    {
        Time.timeScale = 0.0f;  // Pausa el juego estableciendo la escala de tiempo a 0

        PauseButton.SetActive(false);  // Oculta el botón de Pausa
        PausePanel.SetActive(true);    // Muestra el panel de Pausa
    }

    // Método llamado cuando se hace clic en el botón de Resumir (dentro de PausePanel)
    public void ResumeGame()
    {
        Time.timeScale = 1.0f;  // Reanuda la velocidad normal del juego estableciendo la escala de tiempo a 1

        PausePanel.SetActive(false);   // Oculta el panel de Pausa
        PauseButton.SetActive(true);   // Muestra el botón de Pausa
    }

    // Método llamado cuando se hace clic en el botón de Volver al Menú Principal (dentro de PausePanel)
    public void BackToMainMenu()
    {
        Time.timeScale = 1.0f;  // Asegura que la escala de tiempo esté configurada en 1 para reanudar el flujo normal del tiempo

        SceneManager.LoadScene("Menu_screen");  // Carga la escena del menú principal
    }
}

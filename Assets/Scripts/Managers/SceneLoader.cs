using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused) UnpauseGame(); else PauseGame();
        }
    }

    // carrega uma Scene
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // fecha o jogo
    public static void QuitGame()
    {
        Application.Quit();
    }

    // pausa e despausa o jogo
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        ShowMouseCursor(true);
    }

    public void UnpauseGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        ShowMouseCursor(false);
    }

    public static void ShowMouseCursor(bool show)
    {
        if (show) Cursor.lockState = CursorLockMode.None; else Cursor.lockState = CursorLockMode.Locked;
    }
}
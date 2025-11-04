using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                pauseMenu.SetActive(false);
                FreezeTime(false);
            }
            else
            {
                pauseMenu.SetActive(true);
                FreezeTime(true);
            }
        }
    }

    // carrega uma Scene
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // fecha o jogo
    public void QuitGame()
    {
        Application.Quit();
    }

    // pausa e despausa o jogo
    public void FreezeTime(bool isFrozen)
    {
        if (isFrozen)
        {
            isPaused = true;
            Time.timeScale = 0;
        }
        else
        {
            isPaused = false;
            Time.timeScale = 1;
        }
    }
}
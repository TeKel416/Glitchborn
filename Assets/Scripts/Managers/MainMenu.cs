using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        MusicManager.instance.PlayMusic("Main Menu");
    }

    public void Play()
    {
        SceneLoader.LoadScene("Game");
    }

    public void Quit()
    {
        SceneLoader.QuitGame();
    }
}

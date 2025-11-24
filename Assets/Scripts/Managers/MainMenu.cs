using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        SceneLoader.ShowMouseCursor(true);
        MusicManager.instance.PlayMusic("Main Menu");
    }

    public void Play()
    {
        SceneLoader.ShowMouseCursor(false);
        SceneLoader.LoadScene("Game");
    }

    public void Quit()
    {
        SceneLoader.QuitGame();
    }
}

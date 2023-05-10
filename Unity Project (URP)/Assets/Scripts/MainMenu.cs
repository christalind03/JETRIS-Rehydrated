using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioManager _audioManager;

    void Start()
    {
        _audioManager = AudioManager.instance;
        _audioManager.Play("Main Menu");
    }

    public void PlayGame()
    {
        _audioManager.Stop("Main Menu");
        SceneManager.LoadScene("Game");
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

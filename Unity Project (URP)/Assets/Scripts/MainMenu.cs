using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioManager _audioManager;

    [SerializeField]
    private GameObject _mainMenu;
    
    [SerializeField]
    private GameObject _settingsMenu;

    void Start()
    {
        _audioManager = AudioManager.instance;
        _audioManager.PlayMusic("Main Menu");
    }

    public void PlayGame()
    {
        _audioManager.StopMusic("Main Menu");
        SceneManager.LoadScene("Game");
    }

    public void OpenSettings()
    {
        _mainMenu.SetActive(false);
        _settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        _mainMenu.SetActive(true);
        _settingsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

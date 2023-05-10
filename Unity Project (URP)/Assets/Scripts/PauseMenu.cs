using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    private AudioManager _audioManager;

    private GameObject _backdrop;
    private GameObject _pauseMenu;
    private GameObject _warningPrompt;

    private Button _warningYes;
    private Button _warningNo;

    private int _warningResponse;
    private bool _isPaused = false;

    void Awake()
    {
        _audioManager = FindObjectOfType<AudioManager>();

        _backdrop = this.transform.GetChild(0).gameObject;
        _pauseMenu = this.transform.GetChild(1).gameObject;
        _warningPrompt = this.transform.GetChild(2).gameObject;

        _warningYes = _warningPrompt.transform.GetChild(2).gameObject.GetComponent<Button>();
        _warningNo = _warningPrompt.transform.GetChild(3).gameObject.GetComponent<Button>();
    }

    public void SetState(bool newState)
    {
        _isPaused = newState;
    }

    public bool GetState()
    {
        return _isPaused;
    }

    public void PauseGame()
    {
        // Reset warning prompt
        _backdrop.SetActive(true);
        _pauseMenu.SetActive(true);
        _warningPrompt.SetActive(false);

        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void ResumeGame()
    {
        _backdrop.SetActive(false);
        _pauseMenu.SetActive(false);
        _warningPrompt.SetActive(false);

        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void MainMenu()
    {
        StartCoroutine(PromptWarning("Main Menu"));
    }

    public void QuitGame()
    {
        StartCoroutine(PromptWarning("Quit"));
    }

    private IEnumerator PromptWarning(string pauseOption)
    {
        _warningResponse = -1;

        _pauseMenu.SetActive(false);
        _warningPrompt.SetActive(true);

        SetupWarningOptions();

        while(_warningResponse == -1)
        {
            yield return null;
        }

        if(_warningResponse == 1)
        {
            if(pauseOption == "Main Menu")
            {
                _audioManager.Stop("Game");
                SceneManager.LoadScene("Main Menu");
            }
            else if (pauseOption == "Quit")
            {
                Debug.Log("Quitting application...");
                Application.Quit();
            }
            else
            {
                Debug.LogError($"Invalid pause option \" {pauseOption} \" entered as a parameter");
            }
        }
        else
        {
            PauseGame();
        }
    }

    private void SetupWarningOptions()
    {
        _warningYes.onClick.RemoveAllListeners();
        _warningNo.onClick.RemoveAllListeners();

        _warningYes.onClick.AddListener(delegate {_warningResponse = 1;});
        _warningNo.onClick.AddListener(delegate {_warningResponse = 0;});
    }
}

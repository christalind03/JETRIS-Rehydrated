using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game : MonoBehaviour
{
    private AudioManager _audioManager;
    
    [SerializeField]
    private TextMeshProUGUI _countdownText;

    private bool _isCountdownDone = false;
    private bool _isPaused = false;

    void Start()
    {
        _audioManager = AudioManager.instance;
        _audioManager.Play("Game");

        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        Time.timeScale = 0f;
        _isPaused = true;

        _audioManager.Play("Countdown");
        _countdownText.text = "3";
        yield return new WaitForSecondsRealtime(1f);
        _countdownText.text = "2";
        yield return new WaitForSecondsRealtime(1f);
        _countdownText.text = "1";
        yield return new WaitForSecondsRealtime(1f);
        _countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(1f);
        _countdownText.text = "";

        Time.timeScale = 1f;
        _isPaused = false;

        _isCountdownDone = true;
    }

    public bool GetState()
    {
        return _isPaused;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        _isPaused = false;
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private AudioManager _audioManager;

    [SerializeField]
    private TextMeshProUGUI _scoreText;

    [SerializeField]
    private TextMeshProUGUI _highScoreText;

    [SerializeField]
    private GameObject _newHighscoreTag;

    void Awake()
    {
        _audioManager = AudioManager.instance;

        // Update the scores on screen
        int score = PlayerPrefs.GetInt("Score");    
        int highscore = PlayerPrefs.GetInt("Highscore");

        _scoreText.text = score.ToString();
        _highScoreText.text = highscore.ToString();

        if((score == highscore) && (highscore != 0))
        {
            _newHighscoreTag.SetActive(true);
        }
    }

    public void PlayAgain()
    {
        _audioManager.StopMusic("Game");
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        _audioManager.StopMusic("Game");
        SceneManager.LoadScene("Main Menu");
    }
}

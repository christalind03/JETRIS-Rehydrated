using TMPro;
using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _rowTypeText;

    [SerializeField]
    private TextMeshProUGUI _comboText;
    
    [SerializeField]
    private TextMeshProUGUI _scoreText;

    private int _currentScore = 0;
    private int _highScore = 0;

    private int _prevNumRowsCleared = 0;
    private int _numBlocksDropped = 0;
    private int _comboMultiplier = 0;

    void Awake()
    {
        _rowTypeText.text = "";
        _comboText.text = "";
        _scoreText.text = _currentScore.ToString();

        // Setup highscore if not already
        if(PlayerPrefs.HasKey("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", 0);
        }

        _highScore = PlayerPrefs.GetInt("Highscore");
    }

    public void UpdateBlocksDropped()
    {
        _numBlocksDropped++;
    }

    public void UpdateScore(int currentLevel, int rowsCleared)
    {
        switch(rowsCleared)
        {
            case 1:
                _rowTypeText.text = "SINGLE";
                _currentScore += 40 * (currentLevel + 1);
                break;
            
            case 2:
                _rowTypeText.text = "DOUBLE";
                _currentScore += 100 * (currentLevel + 1);
                break;

            case 3:
                _rowTypeText.text = "TRIPLE";
                _currentScore += 300 * (currentLevel + 1);
                break;

            case 4:
                _rowTypeText.text = "TETRIS";
                _currentScore += 1200 * (currentLevel + 1);
                break;
            
            default:
                return;
        }

        if(_rowTypeText.text != "")
        {
            StartCoroutine(ClearRowTypeText());
        }

        if(rowsCleared == _prevNumRowsCleared && _numBlocksDropped <= 1)
        {
            _prevNumRowsCleared = rowsCleared;
            _numBlocksDropped = 0;

            _comboMultiplier++;
            _comboText.text = $"COMBO x{_comboMultiplier}";
            _currentScore += _comboMultiplier * currentLevel * 50;
            StartCoroutine(ClearComboText());
        }
        else
        {
            _prevNumRowsCleared = rowsCleared;
            _numBlocksDropped = 0;
            _comboMultiplier = 0;
        }

        _scoreText.text = _currentScore.ToString();

        // Update highscore if the current score is greater than the old highscore
        if(_currentScore > _highScore)
        {
            PlayerPrefs.SetInt("Highscore", _currentScore);
            _highScore = _currentScore;
        }
    }

    public void SaveCurrentScore()
    {
        PlayerPrefs.SetInt("Score", _currentScore);
    }

    private IEnumerator ClearRowTypeText()
    {
        yield return new WaitForSeconds(1.75f);
        _rowTypeText.text = "";
    }

    private IEnumerator ClearComboText()
    {
        yield return new WaitForSeconds(1.75f);
        _comboText.text = "";
    }
}
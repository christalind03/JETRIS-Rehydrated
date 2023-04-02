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

    private int _prevNumRowsCleared = -1;
    private int _comboMultiplier = 0;

    void Awake()
    {
        _rowTypeText.text = "";
        _comboText.text = "";
        _scoreText.text = _currentScore.ToString();
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
        }

        if(_rowTypeText.text != "")
        {
            StartCoroutine(ClearRowTypeText());
        }

        if(rowsCleared > 0 && rowsCleared == _prevNumRowsCleared)
        {
            _comboMultiplier++;
            _comboText.text = $"COMBO x{_comboMultiplier}";
            StartCoroutine(ClearComboText());

            _currentScore += _comboMultiplier * currentLevel * 50;
            _prevNumRowsCleared = rowsCleared;
        }
        else if(rowsCleared > 0 && rowsCleared != _prevNumRowsCleared)
        {
            _comboMultiplier = 0;
            _prevNumRowsCleared = rowsCleared;
        }

        _scoreText.text = _currentScore.ToString();
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
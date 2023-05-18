using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _currentLevelText;

    private int _currentLevel = 0;
    private int _linesCleared = 0;
    private float _dropSpeed = 1.5f;

    void Awake()
    {
        _currentLevelText.text = _currentLevel.ToString();
    }

    public int GetCurrentLevel()
    {
        return _currentLevel;
    }

    public float GetDropSpeed()
    {
        return _dropSpeed;
    }

    public void UpdateLinesCleared()
    {
        _linesCleared++;

        if(_linesCleared == (_currentLevel * 1) + 1)
        {
            UpdateCurrentLevel();
        }
    }

    private void UpdateCurrentLevel()
    { 
        _currentLevel++;
        _linesCleared = 0;
        _dropSpeed /= 1.25f;

        _currentLevelText.text = _currentLevel.ToString();
    }
}

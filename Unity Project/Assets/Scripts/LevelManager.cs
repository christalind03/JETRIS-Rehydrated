using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _currentLevelText;

    private int _currentLevel = 0;
    private int _linesCleared = 0;
    private float _dropSpeed = 1.5f;

    void Start()
    {
        _currentLevelText.text = _currentLevel.ToString();
    }

    void Update()
    {
        if(_linesCleared == (_currentLevel * 1) + 1)
        {
            _currentLevel++;
            _linesCleared = 0;
            _dropSpeed /= 1.25f;

            _currentLevelText.text = _currentLevel.ToString();
        }
    }

    public float GetDropSpeed()
    {
        return _dropSpeed;
    }

    public void UpdateLinesCleared()
    {
        _linesCleared++;
    }
}

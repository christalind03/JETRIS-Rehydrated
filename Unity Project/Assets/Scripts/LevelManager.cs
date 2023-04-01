using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentLevelText;

    private int currentLevel = 0;
    private int linesCleared = 0;

    void Start()
    {
        currentLevelText.text = currentLevel.ToString();
    }

    void Update()
    {
        if(linesCleared == (currentLevel * 1) + 1)
        {
            currentLevel++;
            linesCleared = 0;

            currentLevelText.text = currentLevel.ToString();
        }
    }

    public void UpdateLinesCleared()
    {
        linesCleared++;
    }
}

using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private void OnEnable()
    {
        ScoreManager.OnScoreChanged += UpdateScoreDisplay;
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreChanged -= UpdateScoreDisplay;
    }

    private void UpdateScoreDisplay(int score)
    {
        scoreText.text = "Score: " + score;
    }
}
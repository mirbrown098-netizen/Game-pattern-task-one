
using TMPro;
using UnityEngine;

public class ScoreHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private void Awake()
    {
        if (scoreText == null)
        {
            scoreText = GetComponent<TMP_Text>();
        }
    }

    private void OnEnable()
    {
        ScoreManager.OnScoreChanged += UpdateScore;
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreChanged -= UpdateScore;
    }

    private void Start()
    {
        UpdateScore(0);
    }

    private void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Zombies Killed: " + score;
        }
    }
}

using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{// Observer event notifies other scripts whenever the score changes.
    public static event Action<int> OnScoreChanged;

    private int score = 0;

    private void Start()
    {
        OnScoreChanged?.Invoke(score);
    }

    public void AddScore(int amount)
    {
        score += amount;
        OnScoreChanged?.Invoke(score);
    }
}

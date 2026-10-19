
using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Observer event for HUD updates.
    public static event Action<int> OnScoreChanged;

    [Header("Score")]
    [SerializeField] private int score = 0;

    public int CurrentScore => score;

    private void OnEnable()
    {
        // Listen for defeated zombies.
        EnemyHealth.OnEnemyDied += HandleEnemyDied;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyDied -= HandleEnemyDied;
    }

    private void Start()
    {
        // Tell the HUD the starting score.
        OnScoreChanged?.Invoke(score);
    }

    private void HandleEnemyDied()
    {
        // One kill = one point.
        AddScore(1);
    }

    public void AddScore(int amount)
    {
        score += amount;

        Debug.Log("Zombies Killed: " + score);

        // Notify HUD listeners.
        OnScoreChanged?.Invoke(score);
    }

    public void ResetScore()
    {
        score = 0;
        OnScoreChanged?.Invoke(score);
    }
}

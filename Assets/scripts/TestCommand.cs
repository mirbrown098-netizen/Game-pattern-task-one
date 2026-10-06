using UnityEngine;

public class TestCommand : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private ScoreManager scoreManager;

    public void TestDamage()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);
        }
    }

    public void TestScore()
    {
        if (scoreManager != null)
        {
            scoreManager.AddScore(10);
        }
    }
}
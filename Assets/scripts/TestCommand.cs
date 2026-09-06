using UnityEngine;
using UnityEngine.SceneManagement;

public class TestCommand : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private ScoreManager scoreManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerHealth.TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            scoreManager.AddScore(10);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
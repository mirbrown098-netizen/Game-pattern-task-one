using UnityEngine;
using TMPro;

public class HealthUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;

    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateHealth;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHealth;
    }

    private void UpdateHealth(int currentHealth)
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth + " / 100";
        }
    }
}
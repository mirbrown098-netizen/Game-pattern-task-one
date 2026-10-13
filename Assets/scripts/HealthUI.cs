using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;

    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateHealthDisplay;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHealthDisplay;
    }

    private void UpdateHealthDisplay(int health)
    {
        healthText.text = "Health: " + health;
    }
}
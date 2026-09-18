using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{ // Observer event notifies other scripts whenever the player's health changes.
    public static event Action<int> OnHealthChanged;

    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        OnHealthChanged?.Invoke(currentHealth);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("Damage");
        }
    }
}

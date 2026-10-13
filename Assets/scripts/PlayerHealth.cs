
using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action<int> OnHealthChanged;
    public static event Action OnPlayerDied;
    public static event Action OnPlayerDamaged;

    [Header("Player Health")]
    [SerializeField] private int maxHealth = 100;

    [Header("Current Health")]
    [SerializeField] private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void Start()
    {
        RestoreHealth();
    }

    private void HandleGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Playing)
        {
            RestoreHealth();
        }
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0 || amount <= 0)
            return;

        if (GameManager.Instance == null ||
            GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;

        currentHealth = Mathf.Max(0, currentHealth - amount);

        Debug.Log("Player Health: " + currentHealth);

        OnHealthChanged?.Invoke(currentHealth);
        OnPlayerDamaged?.Invoke();

        if (currentHealth == 0)
        {
            Debug.Log("Player has died!");
            OnPlayerDied?.Invoke();
        }
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);

        Debug.Log("Player health restored to " + currentHealth);
    }
}

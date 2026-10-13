
using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static event Action OnEnemyDied;
    public static event Action OnEnemyDamaged;

    [Header("Enemy Health")]
    [SerializeField] private int maxHealth = 50;

    [SerializeField] private int currentHealth;

    public int CurrentHealth => currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0 || damage <= 0)
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        Debug.Log("Zombie Health: " + currentHealth);

        OnEnemyDamaged?.Invoke();

        if (currentHealth == 0)
        {
            Debug.Log("Zombie defeated!");

            OnEnemyDied?.Invoke();

            gameObject.SetActive(false);
        }
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
    }
}

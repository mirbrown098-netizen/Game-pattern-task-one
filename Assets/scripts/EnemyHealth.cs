
using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static event Action OnEnemyDied;
    public static event Action OnEnemyDamaged;

    [Header("Enemy Health")]
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private int currentHealth;

    [Header("Romero Animation")]
    [SerializeField] private Animator romeroAnimator;
    [SerializeField] private string deathTrigger = "Die";
    [SerializeField] private float deathAnimationDuration = 2.5f;

    public int CurrentHealth => currentHealth;

    private EnemyStateMachine enemyAI;
    private Coroutine deathCoroutine;
    private bool isDead;

    private void Awake()
    {
        enemyAI = GetComponent<EnemyStateMachine>();

        FindRomeroAnimator();

        currentHealth = maxHealth;
    }

    private void FindRomeroAnimator()
    {
        // Use the Animator explicitly assigned in the Inspector.
        if (romeroAnimator != null)
            return;

        // Find only active child Animators.
        // This ignores the disabled StylizedZombie.
        Animator[] animators =
            GetComponentsInChildren<Animator>(false);

        foreach (Animator animator in animators)
        {
            if (animator.gameObject.name == "Romero")
            {
                romeroAnimator = animator;
                break;
            }
        }

        if (romeroAnimator == null)
        {
            Debug.LogWarning(
                "EnemyHealth: Romero Animator was not found.",
                this
            );
        }
    }

    private void OnEnable()
    {
        RestoreHealth();

        if (enemyAI != null)
        {
            enemyAI.enabled = true;
        }

        if (romeroAnimator == null)
        {
            FindRomeroAnimator();
        }

        // Do not call Animator.Update or Rebind here.
        // These caused warnings when pooled zombies
        // were being reactivated.
        if (romeroAnimator != null &&
            romeroAnimator.isActiveAndEnabled)
        {
            romeroAnimator.ResetTrigger(deathTrigger);
            romeroAnimator.ResetTrigger("Attack");
            romeroAnimator.Play("Walk_N", 0, 0f);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead || currentHealth <= 0 || damage <= 0)
            return;

        currentHealth = Mathf.Max(
            0,
            currentHealth - damage
        );

        Debug.Log("Zombie Health: " + currentHealth);

        OnEnemyDamaged?.Invoke();

        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("Zombie defeated!");

        // Notify the existing score and Observer systems.
        OnEnemyDied?.Invoke();

        // Stop the zombie AI during its death animation.
        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }

        // Ensure we are using Romero, not StylizedZombie.
        FindRomeroAnimator();

        if (romeroAnimator != null &&
            romeroAnimator.isActiveAndEnabled &&
            romeroAnimator.runtimeAnimatorController != null)
        {
            romeroAnimator.ResetTrigger("Attack");
            romeroAnimator.SetTrigger(deathTrigger);

            Debug.Log(
                "Romero death animation triggered!",
                romeroAnimator
            );
        }
        else
        {
            Debug.LogWarning(
                "Romero Animator is missing or inactive!",
                this
            );
        }

        deathCoroutine =
            StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(
            Mathf.Max(0f, deathAnimationDuration)
        );

        deathCoroutine = null;

        // Return to the existing object pool.
        gameObject.SetActive(false);
    }

    public void RestoreHealth()
    {
        if (deathCoroutine != null)
        {
            StopCoroutine(deathCoroutine);
            deathCoroutine = null;
        }

        currentHealth = maxHealth;
        isDead = false;
    }

    private void OnDisable()
    {
        if (deathCoroutine != null)
        {
            StopCoroutine(deathCoroutine);
            deathCoroutine = null;
        }
    }
}

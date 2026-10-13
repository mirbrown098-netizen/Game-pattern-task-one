
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private SwordData swordData;

    [Header("Attack Position")]
    [SerializeField] private Transform attackOrigin;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private float nextAttackTime;

    public void Attack()
    {
        if (swordData == null)
        {
            Debug.LogWarning("SwordData is not assigned!");
            return;
        }

        if (GameManager.Instance == null ||
            GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;

        if (Time.time < nextAttackTime)
            return;

        nextAttackTime = Time.time + swordData.attackCooldown;

        // Play the sword swing animation
        if (animator != null)
        {
            animator.SetTrigger("SwordAttack");
        }

        Transform origin = attackOrigin != null
            ? attackOrigin
            : transform;

        Vector3 attackCenter =
            origin.position + origin.forward * (swordData.attackRange * 0.5f);

        Collider[] hits = Physics.OverlapSphere(
            attackCenter,
            swordData.attackRange * 0.5f
        );

        foreach (Collider hit in hits)
        {
            EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                // Damage the zombie
                enemy.TakeDamage(swordData.damage);

                Debug.Log(
                    "Sword hit zombie for " + swordData.damage + " damage!"
                );

                // Shake the camera when a zombie is hit
                if (CameraShake.Instance != null)
                {
                    CameraShake.Instance.Shake();
                }

                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (swordData == null)
            return;

        Transform origin = attackOrigin != null
            ? attackOrigin
            : transform;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            origin.position + origin.forward * (swordData.attackRange * 0.5f),
            swordData.attackRange * 0.5f
        );
    }
}

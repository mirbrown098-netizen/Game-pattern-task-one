
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewSwordData",
    menuName = "Game/Weapons/Sword Data"
)]
public class SwordData : ScriptableObject
{
    [Header("Sword Settings")]
    public string weaponName = "Survivor Sword";

    public int damage = 25;

    public float attackRange = 2f;

    public float attackCooldown = 0.75f;

    [Header("Hit Effects")]
    public GameObject hitEffectPrefab;
}

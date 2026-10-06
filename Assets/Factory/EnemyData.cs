using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Factory/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Enemy Information")]
    public string enemyName = "Zombie";

    [Header("Prefab")]
    public GameObject prefab;

    [Header("Stats")]
    public int health = 100;
    public float speed = 2f;
}
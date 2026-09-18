using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Factory/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public GameObject prefab;
    public int health = 100;
    public float speed = 2f;
}
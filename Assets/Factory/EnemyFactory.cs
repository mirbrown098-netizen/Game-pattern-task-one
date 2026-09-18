using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private EnemyData[] enemyTypes;

    private List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[enemyTypes.Length];

        for (int i = 0; i < enemyTypes.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Spawn(int typeIndex, Vector3 position)
    {
        if (typeIndex < 0 || typeIndex >= enemyTypes.Length)
        {
            Debug.LogWarning("Invalid enemy type index.");
            return null;
        }

        EnemyData data = enemyTypes[typeIndex];
        GameObject enemy = null;

        // Look for an inactive zombie that can be reused.
        foreach (GameObject pooledEnemy in pools[typeIndex])
        {
            if (!pooledEnemy.activeInHierarchy)
            {
                enemy = pooledEnemy;
                enemy.transform.position = position;
                enemy.SetActive(true);

                Debug.Log("Reused from pool: " + data.enemyName);
                break;
            }
        }

        // If there isn't one available, create a new zombie.
        if (enemy == null)
        {
            enemy = Instantiate(
                data.prefab,
                position,
                Quaternion.identity
            );

            pools[typeIndex].Add(enemy);

            PooledEnemy pooled = enemy.GetComponent<PooledEnemy>();

            if (pooled != null)
            {
                pooled.SetupPool(this, typeIndex);
            }

            Debug.Log("Created new: " + data.enemyName);
        }

        enemy.name = data.enemyName;

        EnemyStats stats = enemy.GetComponent<EnemyStats>();

        if (stats != null)
        {
            stats.Configure(data);
        }

        return enemy;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        Debug.Log("Returned zombie to pool.");
    }
}
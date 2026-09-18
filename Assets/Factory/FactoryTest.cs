using UnityEngine;

public class FactoryTest : MonoBehaviour
{
    [SerializeField] private EnemyFactory factory;

    private GameObject lastSpawnedEnemy;

    private void Update()
    {
        // 4 = Spawn Walker Zombie
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            lastSpawnedEnemy = factory.Spawn(
                0,
                new Vector3(4f, 2f, 0f)
            );
        }

        // 5 = Spawn Runner Zombie
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            lastSpawnedEnemy = factory.Spawn(
                1,
                new Vector3(4f, -2f, 0f)
            );
        }

        // 6 = Return the last spawned zombie to the pool
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (lastSpawnedEnemy != null &&
                lastSpawnedEnemy.activeInHierarchy)
            {
                factory.ReturnEnemy(lastSpawnedEnemy);
            }
        }
    }
}
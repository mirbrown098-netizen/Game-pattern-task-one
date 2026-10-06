using UnityEngine;

public class FactoryTest : MonoBehaviour
{
    [SerializeField] private EnemyFactory factory;

    [Header("3D Spawn Positions")]
    [SerializeField]
    private Vector3 walkerSpawnPosition =
        new Vector3(4f, 0f, 4f);

    [SerializeField]
    private Vector3 runnerSpawnPosition =
        new Vector3(-4f, 0f, 4f);

    private GameObject lastSpawnedEnemy;

    public void SpawnWalker()
    {
        if (factory == null)
        {
            Debug.LogWarning("FactoryTest: EnemyFactory is not assigned.");
            return;
        }

        lastSpawnedEnemy = factory.Spawn(
            0,
            walkerSpawnPosition
        );
    }

    public void SpawnRunner()
    {
        if (factory == null)
        {
            Debug.LogWarning("FactoryTest: EnemyFactory is not assigned.");
            return;
        }

        lastSpawnedEnemy = factory.Spawn(
            1,
            runnerSpawnPosition
        );
    }

    public void ReturnLastSpawnedEnemy()
    {
        if (factory == null)
        {
            Debug.LogWarning("FactoryTest: EnemyFactory is not assigned.");
            return;
        }

        if (lastSpawnedEnemy != null &&
            lastSpawnedEnemy.activeInHierarchy)
        {
            factory.ReturnEnemy(lastSpawnedEnemy);
            lastSpawnedEnemy = null;
        }
    }
}
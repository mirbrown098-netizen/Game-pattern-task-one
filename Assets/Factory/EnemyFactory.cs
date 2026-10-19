
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [Header("Enemy Types")]
    [SerializeField] private EnemyData[] enemyTypes;

    private List<GameObject>[] pools;

    [Header("Automatic Zombie Spawning")]
    [SerializeField] private bool automaticSpawning = true;
    [SerializeField] private int spawnTypeIndex = 0;
    [SerializeField] private float spawnInterval = 8f;
    [SerializeField] private float minimumSpawnDistance = 12f;
    [SerializeField] private float maximumSpawnDistance = 20f;
    [SerializeField] private int maximumActiveEnemies = 8;

    [Header("Spawn Ground Detection")]
    [SerializeField] private float raycastStartHeight = 30f;
    [SerializeField] private float raycastDistance = 100f;
    [SerializeField] private float spawnHeightOffset = 1f;
    [SerializeField] private LayerMask groundLayers = ~0;

    private Transform player;
    private float nextSpawnTime;

    private void Awake()
    {
        pools = new List<GameObject>[enemyTypes.Length];

        for (int i = 0; i < enemyTypes.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    private void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning(
                "EnemyFactory could not find the Player."
            );
        }

        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (!automaticSpawning || player == null)
            return;

        if (Time.time < nextSpawnTime)
            return;

        nextSpawnTime =
            Time.time + Mathf.Max(0.5f, spawnInterval);

        if (CountActiveEnemies() >= maximumActiveEnemies)
            return;

        TrySpawnAutomatically();
    }

    private void TrySpawnAutomatically()
    {
        if (spawnTypeIndex < 0 ||
            spawnTypeIndex >= enemyTypes.Length)
        {
            Debug.LogWarning(
                "Automatic spawn type index is invalid."
            );
            return;
        }

        float minDistance =
            Mathf.Max(1f, minimumSpawnDistance);

        float maxDistance =
            Mathf.Max(minDistance, maximumSpawnDistance);

        // Try several locations to find suitable ground.
        for (int attempt = 0; attempt < 12; attempt++)
        {
            Vector2 randomDirection =
                Random.insideUnitCircle.normalized;

            if (randomDirection.sqrMagnitude < 0.01f)
                continue;

            float distance =
                Random.Range(minDistance, maxDistance);

            Vector3 candidatePosition =
                player.position +
                new Vector3(
                    randomDirection.x * distance,
                    0f,
                    randomDirection.y * distance
                );

            Vector3 rayOrigin =
                candidatePosition +
                Vector3.up * raycastStartHeight;

            RaycastHit[] hits = Physics.RaycastAll(
                rayOrigin,
                Vector3.down,
                raycastDistance,
                groundLayers,
                QueryTriggerInteraction.Ignore
            );

            float highestGround =
                float.NegativeInfinity;

            bool foundGround = false;

            foreach (RaycastHit hit in hits)
            {
                // Don't spawn on the player or another zombie.
                if (hit.collider.transform.IsChildOf(player))
                    continue;

                if (hit.collider.GetComponentInParent<
                    EnemyStateMachine>() != null)
                    continue;

                if (hit.point.y > highestGround)
                {
                    highestGround = hit.point.y;
                    foundGround = true;
                }
            }

            if (!foundGround)
                continue;

            Vector3 spawnPosition =
                new Vector3(
                    candidatePosition.x,
                    highestGround + spawnHeightOffset,
                    candidatePosition.z
                );

            GameObject spawnedEnemy =
                Spawn(spawnTypeIndex, spawnPosition);

            if (spawnedEnemy != null)
            {
                Debug.Log(
                    "Automatically spawned zombie."
                );
            }

            return;
        }

        Debug.LogWarning(
            "EnemyFactory could not find a spawn position."
        );
    }

    private int CountActiveEnemies()
    {
        int count = 0;

        foreach (List<GameObject> pool in pools)
        {
            foreach (GameObject enemy in pool)
            {
                if (enemy != null && enemy.activeInHierarchy)
                {
                    count++;
                }
            }
        }

        return count;
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

        // Reuse an inactive zombie from the pool.
        foreach (GameObject pooledEnemy in pools[typeIndex])
        {
            if (pooledEnemy != null &&
                !pooledEnemy.activeInHierarchy)
            {
                enemy = pooledEnemy;
                enemy.transform.position = position;
                enemy.SetActive(true);

                Debug.Log(
                    "Reused from pool: " + data.enemyName
                );

                break;
            }
        }

        // Create a zombie if the pool is empty.
        if (enemy == null)
        {
            enemy = Instantiate(
                data.prefab,
                position,
                Quaternion.identity
            );

            pools[typeIndex].Add(enemy);

            PooledEnemy pooled =
                enemy.GetComponent<PooledEnemy>();

            if (pooled != null)
            {
                pooled.SetupPool(this, typeIndex);
            }

            Debug.Log(
                "Created new: " + data.enemyName
            );
        }

        enemy.name = data.enemyName;

        EnemyStats stats =
            enemy.GetComponent<EnemyStats>();

        if (stats != null)
        {
            stats.Configure(data);
        }

        return enemy;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        if (enemy == null)
            return;

        enemy.SetActive(false);

        Debug.Log("Returned zombie to pool.");
    }
}

using UnityEngine;

public class PooledEnemy : MonoBehaviour
{
    public int TypeIndex { get; private set; }
    public EnemyFactory Factory { get; private set; }

    public void SetupPool(EnemyFactory factory, int typeIndex)
    {
        Factory = factory;
        TypeIndex = typeIndex;
    }
}
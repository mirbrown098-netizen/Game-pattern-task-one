using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int Health { get; private set; }
    public float Speed { get; private set; }

    public void Configure(EnemyData data)
    {
        Health = data.health;
        Speed = data.speed;

        EnemyStateMachine stateMachine = GetComponent<EnemyStateMachine>();

        if (stateMachine != null)
        {
            stateMachine.speed = Speed;
        }

        Debug.Log(data.enemyName +
                  " configured | Health: " + Health +
                  " | Speed: " + Speed);
    }
}
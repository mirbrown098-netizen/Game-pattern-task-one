using UnityEngine;

public class StateObserver : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerStateMachine.OnPlayerStateChanged += HandlePlayerStateChanged;
        EnemyStateMachine.OnEnemyStateChanged += HandleEnemyStateChanged;
    }

    private void OnDisable()
    {
        PlayerStateMachine.OnPlayerStateChanged -= HandlePlayerStateChanged;
        EnemyStateMachine.OnEnemyStateChanged -= HandleEnemyStateChanged;
    }

    private void HandlePlayerStateChanged(string stateName)
    {
        Debug.Log("OBSERVER - Player changed to: " + stateName);
    }

    private void HandleEnemyStateChanged(string stateName)
    {
        Debug.Log("OBSERVER - Enemy changed to: " + stateName);
    }
}
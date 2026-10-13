
using UnityEngine;

public class ExitGoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (GameManager.Instance == null)
            return;

        if (GameManager.Instance.CurrentState !=
            GameManager.GameState.Playing)
            return;

        Debug.Log("Player reached the exit!");

        GameManager.Instance.WinGame();
    }
}

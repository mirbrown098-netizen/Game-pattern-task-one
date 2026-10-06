using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        MainMenu,
        Playing,
        GameOver
    }

    public static event Action<GameState> OnGameStateChanged;

    [SerializeField] private GameState currentState = GameState.MainMenu;

    public GameState CurrentState => currentState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ChangeState(GameState.MainMenu);
    }

    public void ChangeState(GameState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;

        Debug.Log("GAME STATE CHANGED TO: " + currentState);

        OnGameStateChanged?.Invoke(currentState);
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
    }

    public void EndGame()
    {
        ChangeState(GameState.GameOver);
    }

    public void ReturnToMainMenu()
    {
        ChangeState(GameState.MainMenu);
    }
}
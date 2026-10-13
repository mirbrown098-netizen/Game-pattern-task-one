
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
        GameOver,
        Victory
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

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDied += HandlePlayerDied;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= HandlePlayerDied;
    }

    private void Start()
    {
        // Notify UI even if the starting state is already MainMenu.
        currentState = GameState.MainMenu;
        OnGameStateChanged?.Invoke(currentState);
    }

    private void HandlePlayerDied()
    {
        if (currentState == GameState.Playing)
        {
            EndGame();
        }
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

    public void WinGame()
    {
        if (currentState == GameState.Playing)
        {
            ChangeState(GameState.Victory);
        }
    }

    public void ReturnToMainMenu()
    {
        ChangeState(GameState.MainMenu);
    }
}

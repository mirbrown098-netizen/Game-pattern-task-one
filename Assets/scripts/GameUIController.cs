using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameOverPanel;

    private GameManager.GameState currentState;

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            HandleGameStateChanged(GameManager.Instance.CurrentState);
        }
    }

    private void Update()
    {
        // Keep the cursor unlocked while menus are open.
        if (currentState == GameManager.GameState.MainMenu ||
            currentState == GameManager.GameState.GameOver)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void HandleGameStateChanged(GameManager.GameState newState)
    {
        currentState = newState;

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(
                newState == GameManager.GameState.MainMenu
            );
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(
                newState == GameManager.GameState.GameOver
            );
        }

        if (newState == GameManager.GameState.Playing)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
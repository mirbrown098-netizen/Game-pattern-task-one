using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCommandInput : MonoBehaviour
{
    [SerializeField] private CommandHandler commandHandler;
    [SerializeField] private PlayerStateMachine playerStateMachine;
    [SerializeField] private FactoryTest factoryTest;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float attackStateDuration = 0.35f;

    private float attackTimer;

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        // 7 = MAIN MENU
        if (Keyboard.current.digit7Key.wasPressedThisFrame)
        {
            ICommand command = new GameStateCommand(
                gameManager,
                GameManager.GameState.MainMenu
            );

            commandHandler.ExecuteCommand(command);
        }

        // 8 = PLAYING
        if (Keyboard.current.digit8Key.wasPressedThisFrame)
        {
            ICommand command = new GameStateCommand(
                gameManager,
                GameManager.GameState.Playing
            );

            commandHandler.ExecuteCommand(command);
        }

        // 9 = GAME OVER
        if (Keyboard.current.digit9Key.wasPressedThisFrame)
        {
            ICommand command = new GameStateCommand(
                gameManager,
                GameManager.GameState.GameOver
            );

            commandHandler.ExecuteCommand(command);
        }

        // 4 = SPAWN WALKER
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            ICommand command =
                new SpawnEnemyCommand(factoryTest, 0);

            commandHandler.ExecuteCommand(command);
        }

        // 5 = SPAWN RUNNER
        if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            ICommand command =
                new SpawnEnemyCommand(factoryTest, 1);

            commandHandler.ExecuteCommand(command);
        }

        // 6 = RECYCLE LAST ENEMY
        if (Keyboard.current.digit6Key.wasPressedThisFrame)
        {
            ICommand command =
                new RecycleEnemyCommand(factoryTest);

            commandHandler.ExecuteCommand(command);
        }

        // LEFT MOUSE = ATTACK
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            ICommand command =
                new AttackCommand(playerStateMachine);

            commandHandler.ExecuteCommand(command);

            attackTimer = attackStateDuration;
        }

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
            return;
        }

        Vector3 direction = Vector3.zero;

        // WASD MOVEMENT
        if (Keyboard.current.wKey.isPressed)
            direction += Vector3.up;

        if (Keyboard.current.sKey.isPressed)
            direction += Vector3.down;

        if (Keyboard.current.aKey.isPressed)
            direction += Vector3.left;

        if (Keyboard.current.dKey.isPressed)
            direction += Vector3.right;

        if (direction != Vector3.zero)
        {
            direction.Normalize();

            ICommand command =
                new MoveCommand(transform, direction, moveSpeed);

            commandHandler.ExecuteCommand(command);

            playerStateMachine.ChangeToMoving();
        }
        else
        {
            playerStateMachine.ChangeToIdle();
        }
    }
}
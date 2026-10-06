using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using StarterAssets;

public class StarterAssetsCommandBridge : MonoBehaviour
{
    [SerializeField] private CommandHandler commandHandler;
    [SerializeField] private StarterAssetsInputs starterInputs;
    [SerializeField] private PlayerStateMachine playerStateMachine;

    [Header("Factory")]
    [SerializeField] private FactoryTest factoryTest;

    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;

    [Header("Attack")]
    [SerializeField] private float attackStateDuration = 0.35f;

    private float attackTimer;

    private void Update()
    {
        // LEFT MOUSE = ATTACK
        // Do not attack when clicking a UI element.
        bool pointerOverUI =
            EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject();

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame &&
            !pointerOverUI)
        {
            ICommand command =
                new AttackCommand(playerStateMachine);

            commandHandler.ExecuteCommand(command);

            attackTimer = attackStateDuration;
        }

        // 4 = SPAWN 3D ZOMBIE
        if (Keyboard.current != null &&
            Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            if (factoryTest != null)
            {
                ICommand command =
                    new SpawnEnemyCommand(factoryTest, 0);

                commandHandler.ExecuteCommand(command);
            }
        }

        // 6 = RECYCLE LAST SPAWNED ZOMBIE
        if (Keyboard.current != null &&
            Keyboard.current.digit6Key.wasPressedThisFrame)
        {
            if (factoryTest != null)
            {
                ICommand command =
                    new RecycleEnemyCommand(factoryTest);

                commandHandler.ExecuteCommand(command);
            }
        }

        // 7 = MAIN MENU
        if (Keyboard.current != null &&
            Keyboard.current.digit7Key.wasPressedThisFrame)
        {
            if (gameManager != null)
            {
                ICommand command =
                    new GameStateCommand(
                        gameManager,
                        GameManager.GameState.MainMenu
                    );

                commandHandler.ExecuteCommand(command);
            }
        }

        // 8 = PLAYING
        if (Keyboard.current != null &&
            Keyboard.current.digit8Key.wasPressedThisFrame)
        {
            if (gameManager != null)
            {
                ICommand command =
                    new GameStateCommand(
                        gameManager,
                        GameManager.GameState.Playing
                    );

                commandHandler.ExecuteCommand(command);
            }
        }

        // 9 = GAME OVER
        if (Keyboard.current != null &&
            Keyboard.current.digit9Key.wasPressedThisFrame)
        {
            if (gameManager != null)
            {
                ICommand command =
                    new GameStateCommand(
                        gameManager,
                        GameManager.GameState.GameOver
                    );

                commandHandler.ExecuteCommand(command);
            }
        }

        // RETURN FROM ATTACKING STATE
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                if (starterInputs.move != Vector2.zero)
                {
                    playerStateMachine.ChangeToMoving();
                }
                else
                {
                    playerStateMachine.ChangeToIdle();
                }
            }
        }
    }

    // MOVEMENT THROUGH COMMANDHANDLER
    public void OnMove(InputValue value)
    {
        Vector2 direction = value.Get<Vector2>();

        ICommand command =
            new StarterAssetsMoveCommand(
                starterInputs,
                direction
            );

        commandHandler.ExecuteCommand(command);

        if (attackTimer > 0f)
            return;

        if (direction != Vector2.zero)
        {
            playerStateMachine.ChangeToMoving();
        }
        else
        {
            playerStateMachine.ChangeToIdle();
        }
    }

    // CAMERA LOOK THROUGH COMMANDHANDLER
    public void OnLook(InputValue value)
    {
        Vector2 lookDirection = value.Get<Vector2>();

        ICommand command =
            new LookCommand(
                starterInputs,
                lookDirection
            );

        commandHandler.ExecuteCommand(command);
    }

    // JUMP THROUGH COMMANDHANDLER
    public void OnJump(InputValue value)
    {
        bool jumpState = value.isPressed;

        ICommand command =
            new JumpCommand(
                starterInputs,
                jumpState
            );

        commandHandler.ExecuteCommand(command);
    }

    // SPRINT THROUGH COMMANDHANDLER
    public void OnSprint(InputValue value)
    {
        bool sprintState = value.isPressed;

        ICommand command =
            new SprintCommand(
                starterInputs,
                sprintState
            );

        commandHandler.ExecuteCommand(command);
    }
}
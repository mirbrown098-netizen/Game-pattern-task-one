public class GameStateCommand : ICommand
{
    private GameManager gameManager;
    private GameManager.GameState targetState;

    public GameStateCommand(
        GameManager gameManager,
        GameManager.GameState targetState)
    {
        this.gameManager = gameManager;
        this.targetState = targetState;
    }

    public void Execute()
    {
        gameManager.ChangeState(targetState);
    }
}
public class AttackCommand : ICommand
{
    private PlayerStateMachine playerStateMachine;

    public AttackCommand(PlayerStateMachine playerStateMachine)
    {
        this.playerStateMachine = playerStateMachine;
    }

    public void Execute()
    {
        playerStateMachine.ChangeToAttacking();
    }
}
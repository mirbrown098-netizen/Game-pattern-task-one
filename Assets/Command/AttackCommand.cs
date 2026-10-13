
public class AttackCommand : ICommand
{
    private PlayerStateMachine playerStateMachine;
    private SwordAttack swordAttack;

    public AttackCommand(
        PlayerStateMachine playerStateMachine,
        SwordAttack swordAttack)
    {
        this.playerStateMachine = playerStateMachine;
        this.swordAttack = swordAttack;
    }

    public void Execute()
    {
        if (playerStateMachine != null)
        {
            playerStateMachine.ChangeToAttacking();
        }

        if (swordAttack != null)
        {
            swordAttack.Attack();
        }
    }
}

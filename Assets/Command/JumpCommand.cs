using StarterAssets;

public class JumpCommand : ICommand
{
    private StarterAssetsInputs starterInputs;
    private bool jumpPressed;

    public JumpCommand(
        StarterAssetsInputs starterInputs,
        bool jumpPressed)
    {
        this.starterInputs = starterInputs;
        this.jumpPressed = jumpPressed;
    }

    public void Execute()
    {
        if (starterInputs != null)
        {
            starterInputs.JumpInput(jumpPressed);
        }
    }
}
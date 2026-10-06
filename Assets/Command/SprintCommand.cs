using StarterAssets;

public class SprintCommand : ICommand
{
    private StarterAssetsInputs starterInputs;
    private bool sprintState;

    public SprintCommand(
        StarterAssetsInputs starterInputs,
        bool sprintState)
    {
        this.starterInputs = starterInputs;
        this.sprintState = sprintState;
    }

    public void Execute()
    {
        if (starterInputs != null)
        {
            starterInputs.SprintInput(sprintState);
        }
    }
}
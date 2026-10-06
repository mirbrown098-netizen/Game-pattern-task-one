using UnityEngine;
using StarterAssets;

public class LookCommand : ICommand
{
    private StarterAssetsInputs starterInputs;
    private Vector2 lookDirection;

    public LookCommand(
        StarterAssetsInputs starterInputs,
        Vector2 lookDirection)
    {
        this.starterInputs = starterInputs;
        this.lookDirection = lookDirection;
    }

    public void Execute()
    {
        if (starterInputs != null)
        {
            starterInputs.LookInput(lookDirection);
        }
    }
}
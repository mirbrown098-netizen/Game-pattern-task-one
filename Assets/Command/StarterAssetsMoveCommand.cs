using UnityEngine;
using StarterAssets;

public class StarterAssetsMoveCommand : ICommand
{
    private StarterAssetsInputs starterInputs;
    private Vector2 moveDirection;

    public StarterAssetsMoveCommand(
        StarterAssetsInputs starterInputs,
        Vector2 moveDirection)
    {
        this.starterInputs = starterInputs;
        this.moveDirection = moveDirection;
    }

    public void Execute()
    {
        if (starterInputs != null)
        {
            starterInputs.MoveInput(moveDirection);
        }
    }
}
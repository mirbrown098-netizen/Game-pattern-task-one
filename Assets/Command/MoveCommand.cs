using UnityEngine;

public class MoveCommand : ICommand
{
    private Transform player;
    private Vector3 direction;
    private float speed;

    public MoveCommand(Transform player, Vector3 direction, float speed)
    {
        this.player = player;
        this.direction = direction;
        this.speed = speed;
    }

    public void Execute()
    {
        player.position += direction * speed * Time.deltaTime;
    }
}
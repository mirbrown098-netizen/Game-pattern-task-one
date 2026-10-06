using UnityEngine;

public class CommandHandler : MonoBehaviour
{
    public void ExecuteCommand(ICommand command)
    {
        if (command != null)
        {
            command.Execute();
        }
    }
}
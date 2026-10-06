using System;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public static event Action<string> OnPlayerStateChanged;

    private StateMachine stateMachine;
    private IState currentState;

    private IState idleState;
    private IState movingState;
    private IState attackingState;

    [SerializeField] private string currentStateName = "None";

    private void Start()
    {
        stateMachine = new StateMachine();

        idleState = new PlayerIdleState(this);
        movingState = new PlayerMovingState(this);
        attackingState = new PlayerAttackingState(this);

        ChangeToIdle();
    }

    private void Update()
    {
        stateMachine.Tick();
    }

    public void ChangeToIdle()
    {
        ChangeState(idleState);
    }

    public void ChangeToMoving()
    {
        ChangeState(movingState);
    }

    public void ChangeToAttacking()
    {
        ChangeState(attackingState);
    }

    private void ChangeState(IState newState)
    {
        if (currentState == newState)
            return;

        stateMachine.ChangeState(newState);
        currentState = newState;
    }

    public void NotifyStateChanged(string stateName)
    {
        currentStateName = stateName;

        OnPlayerStateChanged?.Invoke(stateName);
        Debug.Log("Player State: " + stateName);
    }
}

public class PlayerIdleState : IState
{
    private PlayerStateMachine player;

    public PlayerIdleState(PlayerStateMachine player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.NotifyStateChanged("Idle");
    }

    public void Tick()
    {
    }

    public void Exit()
    {
    }
}

public class PlayerMovingState : IState
{
    private PlayerStateMachine player;

    public PlayerMovingState(PlayerStateMachine player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.NotifyStateChanged("Moving");
    }

    public void Tick()
    {
    }

    public void Exit()
    {
    }
}

public class PlayerAttackingState : IState
{
    private PlayerStateMachine player;

    public PlayerAttackingState(PlayerStateMachine player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.NotifyStateChanged("Attacking");
    }

    public void Tick()
    {
    }

    public void Exit()
    {
    }
}
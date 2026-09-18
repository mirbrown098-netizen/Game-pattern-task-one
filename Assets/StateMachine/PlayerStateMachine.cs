using System;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public static event Action<string> OnPlayerStateChanged;

    private StateMachine stateMachine;

    private IState idleState;
    private IState movingState;
    private IState attackingState;

    private void Start()
    {
        stateMachine = new StateMachine();

        idleState = new PlayerIdleState(this);
        movingState = new PlayerMovingState(this);
        attackingState = new PlayerAttackingState(this);

        ChangeState(idleState);
    }

    private void Update()
    {
        stateMachine.Tick();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeState(idleState);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeState(movingState);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeState(attackingState);
        }
    }

    public void ChangeState(IState newState)
    {
        stateMachine.ChangeState(newState);
    }

    public void NotifyStateChanged(string stateName)
    {
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

    public void Tick() { }

    public void Exit() { }
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

    public void Tick() { }

    public void Exit() { }
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

    public void Tick() { }

    public void Exit() { }
}
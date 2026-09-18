using System;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public static event Action<string> OnEnemyStateChanged;

    private StateMachine stateMachine;

    private IState idleState;
    private IState chaseState;
    private IState attackState;

    public Transform Player { get; private set; }

    public float speed = 2f;
    public float chaseDistance = 6f;
    public float attackDistance = 1.5f;

    private void Start()
    {
        GameObject playerObject = GameObject.Find("Player");

        if (playerObject != null)
        {
            Player = playerObject.transform;
        }

        stateMachine = new StateMachine();

        idleState = new EnemyIdleState(this);
        chaseState = new EnemyChaseState(this);
        attackState = new EnemyAttackState(this);

        ChangeState(idleState, "Idle");
    }

    private void Update()
    {
        stateMachine.Tick();
    }

    public void ChangeState(IState newState, string stateName)
    {
        stateMachine.ChangeState(newState);

        Debug.Log("Enemy State: " + stateName);
        OnEnemyStateChanged?.Invoke(stateName);
    }

    public float DistanceToPlayer()
    {
        if (Player == null)
            return Mathf.Infinity;

        return Vector3.Distance(transform.position, Player.position);
    }

    public IState IdleState => idleState;
    public IState ChaseState => chaseState;
    public IState AttackState => attackState;
}

public class EnemyIdleState : IState
{
    private EnemyStateMachine enemy;

    public EnemyIdleState(EnemyStateMachine enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        Debug.Log("Enemy entered Idle");
    }

    public void Tick()
    {
        if (enemy.DistanceToPlayer() <= enemy.chaseDistance)
        {
            enemy.ChangeState(enemy.ChaseState, "Chase");
        }
    }

    public void Exit() { }
}

public class EnemyChaseState : IState
{
    private EnemyStateMachine enemy;

    public EnemyChaseState(EnemyStateMachine enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        Debug.Log("Enemy entered Chase");
    }

    public void Tick()
    {
        if (enemy.Player == null)
            return;

        float distance = enemy.DistanceToPlayer();

        if (distance <= enemy.attackDistance)
        {
            enemy.ChangeState(enemy.AttackState, "Attack");
            return;
        }

        if (distance > enemy.chaseDistance)
        {
            enemy.ChangeState(enemy.IdleState, "Idle");
            return;
        }

        enemy.transform.position = Vector3.MoveTowards(
            enemy.transform.position,
            enemy.Player.position,
            enemy.speed * Time.deltaTime
        );
    }

    public void Exit() { }
}

public class EnemyAttackState : IState
{
    private EnemyStateMachine enemy;

    public EnemyAttackState(EnemyStateMachine enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        Debug.Log("Enemy entered Attack");
    }

    public void Tick()
    {
        if (enemy.DistanceToPlayer() > enemy.attackDistance)
        {
            enemy.ChangeState(enemy.ChaseState, "Chase");
        }
    }

    public void Exit() { }
}
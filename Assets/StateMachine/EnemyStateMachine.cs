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

    [Header("Movement")]
    public float speed = 2f;
    public float rotationSpeed = 8f;
    public float chaseDistance = 100f;
    public float attackDistance = 1.5f;

    [Header("Ground Following")]
    public float groundCheckHeight = 3f;
    public float groundOffset = 1f;

    [Header("Current State")]
    [SerializeField] private string currentStateName = "None";

    private void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

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

        currentStateName = stateName;

        Debug.Log("Enemy State: " + stateName);
        OnEnemyStateChanged?.Invoke(stateName);
    }

    public float DistanceToPlayer()
    {
        if (Player == null)
            return Mathf.Infinity;

        Vector3 enemyPosition = transform.position;
        Vector3 playerPosition = Player.position;

        enemyPosition.y = 0f;
        playerPosition.y = 0f;

        return Vector3.Distance(enemyPosition, playerPosition);
    }

    public void MoveTowardPlayer()
    {
        if (Player == null)
            return;

        Vector3 currentPosition = transform.position;

        Vector3 targetPosition = new Vector3(
            Player.position.x,
            currentPosition.y,
            Player.position.z
        );

        Vector3 direction = targetPosition - currentPosition;
        direction.y = 0f;

        // Face the player while chasing.
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction.normalized);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Required movement for the assignment.
        Vector3 nextPosition = Vector3.MoveTowards(
            currentPosition,
            targetPosition,
            speed * Time.deltaTime
        );

        FollowGround(ref nextPosition);

        transform.position = nextPosition;
    }

    private void FollowGround(ref Vector3 nextPosition)
    {
        Vector3 rayStart =
            nextPosition + Vector3.up * groundCheckHeight;

        RaycastHit[] hits = Physics.RaycastAll(
            rayStart,
            Vector3.down,
            groundCheckHeight * 2f
        );

        float highestGround = float.NegativeInfinity;
        bool foundGround = false;

        foreach (RaycastHit hit in hits)
        {
            // Ignore the zombie's own collider and children.
            if (hit.collider.transform.IsChildOf(transform) ||
                hit.collider.transform == transform)
            {
                continue;
            }

            if (hit.point.y > highestGround)
            {
                highestGround = hit.point.y;
                foundGround = true;
            }
        }

        if (foundGround)
        {
            nextPosition.y = highestGround + groundOffset;
        }
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

        enemy.MoveTowardPlayer();
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
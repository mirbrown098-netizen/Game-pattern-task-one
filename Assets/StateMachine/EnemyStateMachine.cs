
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

    private Animator zombieAnimator;

    [Header("Movement")]
    public float speed = 2f;
    public float rotationSpeed = 8f;
    public float chaseDistance = 100f;
    public float attackDistance = 1.5f;

    [Header("Attack")]
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;

    [Header("Attack Line of Sight")]
    public float attackCheckHeight = 1f;

    [Header("Ground Following")]
    public float groundCheckHeight = 3f;
    public float groundOffset = 1f;

    [Header("Wall Collision Fix")]
    public float wallCheckHeight = 0.5f;
    public float wallCheckRadius = 0.3f;
    public float maximumStepHeight = 0.35f;
    public float minimumGroundNormal = 0.65f;

    [Header("Current State")]
    [SerializeField]
    private string currentStateName = "None";

    private void Start()
    {
        zombieAnimator = GetComponentInChildren<Animator>();

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
        if (stateMachine != null)
        {
            stateMachine.Tick();
        }
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

        return Vector3.Distance(
            enemyPosition,
            playerPosition
        );
    }

    // NEW: Check for walls between Romero and the player.
    public bool HasClearAttackPath()
    {
        if (Player == null)
            return false;

        Vector3 origin =
            transform.position +
            Vector3.up * attackCheckHeight;

        Vector3 target =
            Player.position +
            Vector3.up * attackCheckHeight;

        Vector3 direction = target - origin;
        float distance = direction.magnitude;

        if (distance <= 0.01f)
            return true;

        RaycastHit[] hits = Physics.RaycastAll(
            origin,
            direction.normalized,
            distance,
            ~0,
            QueryTriggerInteraction.Ignore
        );

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider == null)
                continue;

            Transform hitTransform =
                hit.collider.transform;

            // Ignore Romero's own colliders.
            if (hitTransform == transform ||
                hitTransform.IsChildOf(transform))
            {
                continue;
            }

            // Ignore the player's colliders.
            if (hitTransform == Player ||
                hitTransform.IsChildOf(Player))
            {
                continue;
            }

            // Ignore other zombies.
            if (hit.collider.GetComponentInParent<
                EnemyStateMachine>() != null)
            {
                continue;
            }

            // Something solid blocks the attack.
            return false;
        }

        return true;
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

        Vector3 direction =
            targetPosition - currentPosition;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
            return;

        Vector3 moveDirection = direction.normalized;

        Quaternion targetRotation =
            Quaternion.LookRotation(moveDirection);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        float moveDistance =
            Mathf.Min(
                speed * Time.deltaTime,
                direction.magnitude
            );

        if (IsWallBlocking(moveDirection, moveDistance))
        {
            return;
        }

        Vector3 nextPosition =
            currentPosition +
            moveDirection * moveDistance;

        FollowGround(ref nextPosition);

        transform.position = nextPosition;
    }

    private bool IsWallBlocking(
        Vector3 direction,
        float moveDistance
    )
    {
        Vector3 rayStart =
            transform.position +
            Vector3.up * wallCheckHeight;

        RaycastHit[] hits = Physics.SphereCastAll(
            rayStart,
            wallCheckRadius,
            direction,
            moveDistance + 0.05f,
            ~0,
            QueryTriggerInteraction.Ignore
        );

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider == null)
                continue;

            Transform hitTransform =
                hit.collider.transform;

            if (hitTransform == transform ||
                hitTransform.IsChildOf(transform))
            {
                continue;
            }

            if (Player != null &&
                (hitTransform == Player ||
                 hitTransform.IsChildOf(Player)))
            {
                continue;
            }

            if (hit.collider.GetComponentInParent<
                EnemyStateMachine>() != null)
            {
                continue;
            }

            if (hit.normal.y >= minimumGroundNormal)
            {
                continue;
            }

            return true;
        }

        return false;
    }

    private void FollowGround(ref Vector3 nextPosition)
    {
        Vector3 rayStart =
            nextPosition +
            Vector3.up * groundCheckHeight;

        RaycastHit[] hits = Physics.RaycastAll(
            rayStart,
            Vector3.down,
            groundCheckHeight * 2f,
            ~0,
            QueryTriggerInteraction.Ignore
        );

        float bestGroundHeight =
            float.NegativeInfinity;

        bool foundGround = false;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider == null)
                continue;

            Transform hitTransform =
                hit.collider.transform;

            if (hitTransform == transform ||
                hitTransform.IsChildOf(transform))
            {
                continue;
            }

            if (Player != null &&
                (hitTransform == Player ||
                 hitTransform.IsChildOf(Player)))
            {
                continue;
            }

            if (hit.collider.GetComponentInParent<
                EnemyStateMachine>() != null)
            {
                continue;
            }

            if (hit.normal.y < minimumGroundNormal)
            {
                continue;
            }

            float targetHeight =
                hit.point.y + groundOffset;

            if (targetHeight >
                transform.position.y + maximumStepHeight)
            {
                continue;
            }

            if (hit.point.y > bestGroundHeight)
            {
                bestGroundHeight = hit.point.y;
                foundGround = true;
            }
        }

        if (foundGround)
        {
            nextPosition.y =
                bestGroundHeight + groundOffset;
        }
        else
        {
            nextPosition.y = transform.position.y;
        }
    }

    public void DealDamageToPlayer()
    {
        if (Player == null)
            return;

        // NEW: No damage through walls.
        if (!HasClearAttackPath())
            return;

        if (zombieAnimator != null)
        {
            zombieAnimator.SetTrigger("Attack");
        }

        PlayerHealth playerHealth =
            Player.GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            playerHealth =
                Player.GetComponentInChildren<PlayerHealth>();
        }

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);

            Debug.Log(
                "Zombie dealt " +
                attackDamage +
                " damage!"
            );
        }
        else
        {
            Debug.LogWarning(
                "Zombie could not find PlayerHealth."
            );
        }
    }

    public IState IdleState => idleState;
    public IState ChaseState => chaseState;
    public IState AttackState => attackState;
}


// ========================================
// IDLE STATE
// ========================================

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
            enemy.ChangeState(
                enemy.ChaseState,
                "Chase"
            );
        }
    }

    public void Exit() { }
}


// ========================================
// CHASE STATE
// ========================================

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

        float distance =
            enemy.DistanceToPlayer();

        if (distance <= enemy.attackDistance)
        {
            enemy.ChangeState(
                enemy.AttackState,
                "Attack"
            );
            return;
        }

        if (distance > enemy.chaseDistance)
        {
            enemy.ChangeState(
                enemy.IdleState,
                "Idle"
            );
            return;
        }

        enemy.MoveTowardPlayer();
    }

    public void Exit() { }
}


// ========================================
// ATTACK STATE
// ========================================

public class EnemyAttackState : IState
{
    private EnemyStateMachine enemy;
    private float nextAttackTime;

    public EnemyAttackState(EnemyStateMachine enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        Debug.Log("Enemy entered Attack");
        nextAttackTime = Time.time;
    }

    public void Tick()
    {
        if (enemy.Player == null)
            return;

        if (enemy.DistanceToPlayer() >
            enemy.attackDistance)
        {
            enemy.ChangeState(
                enemy.ChaseState,
                "Chase"
            );
            return;
        }

        // NEW: Only attack when the path is clear.
        if (!enemy.HasClearAttackPath())
        {
            return;
        }

        if (Time.time >= nextAttackTime)
        {
            enemy.DealDamageToPlayer();

            nextAttackTime =
                Time.time + enemy.attackCooldown;
        }
    }

    public void Exit() { }
}

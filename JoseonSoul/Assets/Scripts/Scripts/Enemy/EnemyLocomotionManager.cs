using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using UnityEngine.AI;

public class EnemyLocomotionManager : MonoBehaviour
{
    [SerializeField] private Transform player;            // player transform
    [SerializeField] private float followRange = 10f;     // following range
    [SerializeField] private float returnRange = 15f;     // stop following range
    [SerializeField] private float homeLimit = 20f;       // max distance from initial position
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 120f;

    private Vector3 homePosition;                           // initial position
    private bool isReturning = false;

    private EnemyController enemyController;
    private NavMeshAgent agent;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        homePosition = transform.position;
        agent.speed = moveSpeed;
        agent.angularSpeed = rotationSpeed;
    }

    // Update is called once per frame
    public void HandleMovement()
    {
        if (agent.isStopped) return;

        float playerDistance = Vector3.Distance(transform.position, player.position); // distance from enemy to player
        float homeDistance = Vector3.Distance(transform.position, homePosition);      // distance from enemy to initial position

        if (playerDistance <= followRange && homeDistance <= homeLimit)
        {
            FollowPlayer();
            isReturning = false;
        }
        else if (playerDistance > returnRange || homeDistance > homeLimit)
        {
            ReturnToHome();
        }
        else
        {
            Idle();
        }
    }

    void FollowPlayer()
    {
        agent.SetDestination(player.position);
        enemyController.SetEnemyState((int)EnemyState.Moving);
    }

    void ReturnToHome()
    {
        if (Vector3.Distance(transform.position, homePosition) < 0.1f)
        {
            isReturning = false;
            agent.ResetPath();
            enemyController.SetEnemyState((int)EnemyState.Idle);
            return;
        }

        agent.SetDestination(homePosition);
        enemyController.SetEnemyState((int)EnemyState.Moving);
        isReturning = true;
    }

    void Idle()
    {
        agent.ResetPath();
        enemyController.SetEnemyState((int)EnemyState.Idle);
    }

    public void StopMoving()
    {
        agent.isStopped = true;
    }

    public void ResumeMoving()
    {
        agent.isStopped = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, followRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, returnRange);
    }
}
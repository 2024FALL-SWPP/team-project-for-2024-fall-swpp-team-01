using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Boss;
using Enemy;

public class BossLocomotionManager : MonoBehaviour
{
    private Transform player;            // player transform

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 120f;

    private BossController bossController;
    private NavMeshAgent agent;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerController.Instance != null)
        {
            player = PlayerController.Instance.transform;
        }
        else
        {
            Debug.LogError("PlayerController.Instance is null. Ensure PlayerController exists in the scene.");
        }

        bossController = GetComponent<BossController>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = moveSpeed;
        agent.angularSpeed = rotationSpeed;
    }

    // Update is called once per frame
    public void HandleMovement()
    {
        // if (bossController.IsAttacking()) return; // if attacking, stop moving

        float playerDistance = Vector3.Distance(transform.position, player.position); // distance from enemy to player

        FollowPlayer();
    }

    void FollowPlayer()
    {
        agent.SetDestination(player.position);
        bossController.SetBossState((int)BossState.Moving);
    }

    public void StopMoving()
    {
        agent.isStopped = true;
    }

    public void ResumeMoving()
    {
        agent.isStopped = false;
    }
}

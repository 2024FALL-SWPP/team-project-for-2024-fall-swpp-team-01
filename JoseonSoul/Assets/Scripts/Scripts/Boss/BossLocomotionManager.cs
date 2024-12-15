using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Boss;

public class BossLocomotionManager : MonoBehaviour
{
    public NavMeshAgent agent;
    private BossController bossController;
    private BossAttackManager attackManager;
    private Transform player;
    private Animator animator;

    [SerializeField] private float walkTimer = 0f;
    [SerializeField] private const float runThreshold = 3f;

    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 8f;

    [SerializeField] private float desiredDistance = 3f;
    private void Start()
    {
        if (PlayerController.Instance != null)
        {
            player = PlayerController.Instance.transform;
        }
        else
        {
            Debug.LogError("PlayerController.Instance is null. Ensure PlayerController exists in the scene.");
        }
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        bossController = GetComponent<BossController>();
        attackManager = GetComponent<BossAttackManager>();

        agent.speed = walkSpeed;
    }

    public void HandleMovement()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Vector3 targetPosition = player.position - directionToPlayer * desiredDistance;
        agent.SetDestination(targetPosition);
        float remainingDistance = Vector3.Distance(transform.position, player.position);
        if (remainingDistance <= desiredDistance)
        {
            agent.updateRotation = false;
            LookAtPlayer();
        }
        else
        {
            agent.updateRotation = true;
        }

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (attackManager.isAttacking) 
        {
            walkTimer = 0f;
            return;
        }
  
        else
        {
            if (walkTimer >= runThreshold)
            {
                bossController.SetBossState((int)BossState.Running);
                agent.speed = runSpeed;
            }
            else
            {
                walkTimer += Time.deltaTime;
                bossController.SetBossState((int)BossState.Walking);
                agent.speed = walkSpeed;
            }
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 7f);
    }
}

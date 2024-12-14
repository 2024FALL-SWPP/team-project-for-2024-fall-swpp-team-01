using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss;
using System.Data.Common;
using UnityEngine.AI;
using System.Security.Cryptography;
using JetBrains.Annotations;

public class BossAttackManager : MonoBehaviour
{
    private Transform player;
    private BossController bossController;
    private BossLocomotionManager locomotionManager;
    private Animator animator;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attack_2_Range_MIN = 10f;
    [SerializeField] private float TeleportThershold = 10f;

    [SerializeField] private float walkingWhileAttack = 1.5f;

    [SerializeField] private bool hasHitPlayer = false;
    [SerializeField] private int attackNumber = 1;

    public bool isAttacking = false;

    public int attackDamage = 40;


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

        animator = GetComponent<Animator>();
        bossController = GetComponent<BossController>();
        locomotionManager = GetComponent<BossLocomotionManager>();
    }

    public void HandleAttack()
    {
        if (isAttacking) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (attackNumber)
        {
            case 1:
                Attack1(distanceToPlayer);
                break;
            case 2:
                Attack2(distanceToPlayer);
                break;
            case 3:
                Attack3(distanceToPlayer);
                break;
            case 4:
                Attack4(distanceToPlayer);
                break;
            case 5:
                Attack5(distanceToPlayer);
                break;
            case 6:
                Attack6(distanceToPlayer);
                break;
            case 7:
                SummonBat(distanceToPlayer);
                break;
            case 8:
                SummonSword(distanceToPlayer);
                break;
            case 9:
                TeleportStart(distanceToPlayer);
                break;
        }

        
    }

    public void Attack1(float distanceToPlayer)
    {
        if (!isAttacking && distanceToPlayer <= attackRange)
        {
            isAttacking = true;
            hasHitPlayer = false;
            bossController.SetBossState((int)BossState.Idle);
            Debug.Log("attack1 start");
            animator.SetTrigger("Attack1_1");
            locomotionManager.agent.speed = walkingWhileAttack;
        }
        
    }

    public void Attack2(float distanceToPlayer)
    {
        /*
        // if we want attack2 only after running, use this code.

        if (distanceToPlayer <= attack_2_Range_MIN)
        {
            Debug.Log("Too close to attack2");
            ChooseAttack();
            return;
        }
        */

        if (!isAttacking && distanceToPlayer <= attackRange)
        {
            isAttacking = true;
            hasHitPlayer = false;
            bossController.SetBossState((int)BossState.Idle);
            
            Debug.Log("attack2 start");
            animator.SetTrigger("Attack2_1");
            locomotionManager.agent.speed = 10f;
        }
            
    }

    public void Attack3(float distanceToPlayer)
    {
        if (!isAttacking && distanceToPlayer <= attackRange)
        {
            isAttacking = true;
            hasHitPlayer = false;
            bossController.SetBossState((int)BossState.Idle);
            Debug.Log("attack3 start");

            
            animator.SetTrigger("Attack3_1");
            locomotionManager.agent.speed = walkingWhileAttack;
        }
    }

    public void Attack4(float distanceToPlayer)
    {
        if (!isAttacking && distanceToPlayer <= attackRange)
        {
            isAttacking = true;
            hasHitPlayer = false;
            bossController.SetBossState((int)BossState.Idle);
            Debug.Log("attack4 start");

            
            animator.SetTrigger("Attack4_1");
            locomotionManager.agent.speed = walkingWhileAttack;
        }
    }

    public void Attack5(float distanceToPlayer)
    {
        if (!isAttacking && distanceToPlayer <= attackRange)
        {
            isAttacking = true;
            hasHitPlayer = false;
            bossController.SetBossState((int)BossState.Idle);
            Debug.Log("attack5 start");

            locomotionManager.agent.speed = walkingWhileAttack;
            animator.SetTrigger("Attack5");
        }
    }

    public void Attack6(float distanceToPlayer)
    {
        if (!isAttacking && distanceToPlayer <= attackRange)
        {
            isAttacking = true;
            hasHitPlayer = false;
            bossController.SetBossState((int)BossState.Idle);
            Debug.Log("attack6 start");

            locomotionManager.agent.speed = walkingWhileAttack;
            animator.SetTrigger("Attack6");
        }
    }

    public void SummonBat(float distanceToPlayer)
    {
        if (distanceToPlayer <= attack_2_Range_MIN)
        {
            Debug.Log("Too close to summon bats");
            ChooseAttack();
            return;
        }

        if (!isAttacking && distanceToPlayer >= attack_2_Range_MIN)
        {
            isAttacking = true;
            hasHitPlayer = false;
            bossController.SetBossState((int)BossState.Idle);

            Debug.Log("summon bat start");
            animator.SetTrigger("SummonBat");
            locomotionManager.agent.speed = 0f;
        }
    }

    public void SummonSword(float distanceToPlayer)
    {
        if (distanceToPlayer <= attack_2_Range_MIN)
        {
            Debug.Log("Too close to summon swords");
            ChooseAttack();
            return;
        }

        if (!isAttacking && distanceToPlayer >= attack_2_Range_MIN)
        {
            isAttacking = true;
            hasHitPlayer = false;
            bossController.SetBossState((int)BossState.Idle);

            Debug.Log("summon sword start");
            animator.SetTrigger("SummonSword");
            locomotionManager.agent.speed = 0f;
        }
    }

    public void TeleportStart(float distanceToPlayer)
    {
        if (distanceToPlayer <= TeleportThershold)
        {
            Debug.Log("Too close to teleport");
            ChooseAttack();
            return;
        }

        if (!isAttacking)
        {

            isAttacking = true;
            hasHitPlayer = false;
            bossController.SetBossState((int)BossState.Idle);

            Debug.Log("teleport start");
            animator.SetTrigger("Teleport");
            locomotionManager.agent.speed = 0f;
        }
    }

    public void TeleportToPlayer()
    {
        float desiredDistance = 5f;
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Vector3 targetPosition = player.position - directionToPlayer * desiredDistance;
        transform.position = targetPosition;
    }

    public void AttackEnd(string parameters)
    {
        string[] args = parameters.Split(',');
        string attackName = args[0];
        float moveSpeed = float.Parse(args[1]);

        locomotionManager.agent.speed = moveSpeed;
        animator.SetTrigger(attackName);
        hasHitPlayer = false;
    }

    public void AttackAllFinished()
    {
        ChooseAttack();
        isAttacking = false;
        hasHitPlayer = false;
        animator.SetTrigger("Idle_trigger");
    }

    public void SummonAllFinished()
    {
        animator.SetTrigger("Recovery");

    }

    public void Recover()
    {
        isAttacking = false;
        hasHitPlayer = false;
        ChooseAttack();
        animator.SetTrigger("Idle_trigger");
    }
    public void ChooseAttack()
    {
        attackNumber = Random.Range(6, 10);
    }
}

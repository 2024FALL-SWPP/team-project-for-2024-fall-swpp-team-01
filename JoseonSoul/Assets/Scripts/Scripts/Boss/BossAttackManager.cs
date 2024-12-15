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

    public GameObject batPrefab;
    public GameObject swordPrefab;

    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attack_2_Range_MIN = 7f;
    [SerializeField] private float TeleportThershold = 10f;

    [SerializeField] private float walkingWhileAttack = 1.5f;

    [SerializeField] private float batSpawnHeight = 15f;
    [SerializeField] private float gridSpacing1 = 5f;
    [SerializeField] private float gridSpacing2 = 8f;

    [SerializeField] private bool hasHitPlayer = false; //to avoid double attack. if collision detected, please set hasHitPlayer = true.
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

        if (!isAttacking && distanceToPlayer >= attack_2_Range_MIN)
        {
            isAttacking = true;
            hasHitPlayer = false;
            bossController.SetBossState((int)BossState.Idle);

            Debug.Log("summon bat start");
            animator.SetTrigger("SummonBat");
            locomotionManager.agent.speed = 0.1f;
            Spawn3x3Bats();
        }
    }
    public void Spawn3x3Bats()
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3 spawnPosition1 = player.position + Vector3.up * batSpawnHeight
                                      + new Vector3(i * gridSpacing1, 0, j * gridSpacing1);

                Instantiate(batPrefab, spawnPosition1, batPrefab.transform.rotation);
            }
        }
    }
    public void SummonSword(float distanceToPlayer)
    {

        if (!isAttacking && distanceToPlayer >= attack_2_Range_MIN)
        {
            isAttacking = true;
            hasHitPlayer = false;
            bossController.SetBossState((int)BossState.Idle);

            Debug.Log("summon sword start");
            animator.SetTrigger("SummonSword");
            locomotionManager.agent.speed = 0.1f;

            Spawn3x3Swords();
        }
    }

    public void Spawn3x3Swords()
    {
        Quaternion spawnRotation = transform.rotation * Quaternion.Euler(-90, 0, 0);
        for (int i = 0; i <= 2; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3 spawnPosition = transform.position + transform.forward * 3f
                                    + transform.up * i * gridSpacing2
                                    + transform.right * j * gridSpacing2;

                Instantiate(swordPrefab, spawnPosition, spawnRotation);
            }
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
            locomotionManager.agent.speed = 0.1f;
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
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer >= TeleportThershold)
        {
            int[] values = { 2, 7, 8, 9 };
            attackNumber = values[Random.Range(0, values.Length)];
        }
        else if (distanceToPlayer <= attack_2_Range_MIN)
        {
            int[] values = { 1, 2, 3, 4, 5, 6 };
            attackNumber = values[Random.Range(0, values.Length)];
        }
        else
        {
            attackNumber = Random.Range(1, 10);
        }
        
    }
}
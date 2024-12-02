using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class EnemyAttackManager : MonoBehaviour
{
    private Animator animator;
    private EnemyLocomotionManager locomotionManager;
    private EnemyController enemyController;

    [SerializeField] private Transform player;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.5f;

    private bool isAttacking = false; 


    // Start is called before the first frame update
    void Start()
    {
        locomotionManager = GetComponent<EnemyLocomotionManager>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void HandleAttack()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);

        if (playerDistance <= attackRange && !isAttacking)
        {
            StartAttack();
        }
    }

    void StartAttack()
    {
        isAttacking = true;

        locomotionManager.StopMoving();

        enemyController.SetEnemyState((int)EnemyState.Attacking);

        StartCoroutine(AttackCooldownRoutine());
    }

    IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;

        float playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= attackRange)
        {
            enemyController.SetEnemyState((int)EnemyState.Idle);
        }
        else
        {
            enemyController.SetEnemyState((int)EnemyState.Moving);
            locomotionManager.ResumeMoving();
        }
    }
}

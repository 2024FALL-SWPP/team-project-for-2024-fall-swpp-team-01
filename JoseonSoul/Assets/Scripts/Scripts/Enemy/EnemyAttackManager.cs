using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class EnemyAttackManager : MonoBehaviour
{
    private Transform player;

    private Animator animator;
    private EnemyLocomotionManager locomotionManager;
    private EnemyController enemyController;

    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackActiveDuration = 1.0f;

    [SerializeField] private bool isAttacking = false;
    [SerializeField] private int attackDamage = 10;

    private bool hasDealtDamage = false;

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

        enemyController = GetComponent<EnemyController>();
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
        hasDealtDamage = true;

        locomotionManager.StopMoving();

        enemyController.SetEnemyState((int)EnemyState.Attacking);

        StartCoroutine(AttackCooldownRoutine());
    }

    public void CancelAttack()
    {
        if (isAttacking)
        {
            isAttacking = false;
            StopAllCoroutines();
        }
    }

    IEnumerator AttackCooldownRoutine()
    {

        yield return new WaitForSeconds(attackActiveDuration);
        isAttacking = false;
        yield return new WaitForSeconds(attackCooldown - attackActiveDuration);

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

    public bool IsAttacking()
    {
        return isAttacking;
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isAttacking && !hasDealtDamage)
        {
            Debug.Log("Player entered the trigger zone!");
        }
    }
}

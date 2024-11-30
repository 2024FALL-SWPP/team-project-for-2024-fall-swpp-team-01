using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    private EnemyLocomotionManager locomotionManager;
    private EnemyAttackManager attackManager;
    private Animator animator;

    [SerializeField] private int enemyState = 0;
    [SerializeField] private float stunnedDuration = 1.5f;
    private bool isStunned = false;

    // Start is called before the first frame update
    void Start()
    {
        locomotionManager = GetComponent<EnemyLocomotionManager>();
        attackManager = GetComponent<EnemyAttackManager>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStunned) return;
        attackManager.HandleAttack();
        locomotionManager.HandleMovement();
    }

    public void TakeDamage()
    {
        isStunned = true;

        locomotionManager.StopMoving();
        attackManager.CancelAttack();

        SetEnemyState((int)EnemyState.Stunned);

        StartCoroutine(RecoverFromStun());
    }

    IEnumerator RecoverFromStun()
    {
        yield return new WaitForSeconds(stunnedDuration);
        isStunned = false;
        SetEnemyState((int)EnemyState.Idle);
    }

    void SyncAnimationState()
    {
        animator.SetInteger("enemy_state", enemyState);
    }

    public void SetEnemyState(int state)
    {
        enemyState = state;
        SyncAnimationState();
    }
    public bool IsStunnedOrAttacking()
    {
        return isStunned || attackManager.IsAttacking();
    }
}

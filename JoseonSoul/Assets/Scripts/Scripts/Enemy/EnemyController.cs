using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyLocomotionManager locomotionManager;
    private EnemyAttackManager attackManager;
    private Animator animator;

    [SerializeField] private int enemyState = 0;

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
        attackManager.HandleAttack();
        locomotionManager.HandleMovement();
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
}

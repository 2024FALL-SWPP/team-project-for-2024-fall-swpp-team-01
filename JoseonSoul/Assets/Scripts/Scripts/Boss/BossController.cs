using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private BossLocomotionManager locomotionManager;
    private BossAttackManager attackManager;
    private Animator animator;

    [SerializeField] private int bossState = 0;

    // Start is called before the first frame update
    void Start()
    {
        locomotionManager = GetComponent<BossLocomotionManager>();
        attackManager = GetComponent<BossAttackManager>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void SyncAnimationState()
    {
        animator.SetInteger("enemy_state", bossState);
    }

    public void SetBossState(int state)
    {
        bossState = state;
        SyncAnimationState();
    }
}

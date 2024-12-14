using Boss;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private BossLocomotionManager locomotionManager;
    private BossAttackManager attackManager;
    private Animator animator;

    [SerializeField] private int bossState = 0;

    private bool isEntering = true;

    public int maxHealth = 100;

    private void Start()
    {
        locomotionManager = GetComponent<BossLocomotionManager>();
        attackManager = GetComponent<BossAttackManager>();
        animator = GetComponent<Animator>();

        StartCoroutine(BossEntrance());
    }

    private void Update()
    {
        if (isEntering) return;
        locomotionManager.HandleMovement();
        attackManager.HandleAttack();
    }

    public void SetBossState(int state)
    {
        bossState = state;
        animator.SetInteger("boss_state", bossState);
    }

    public int GetBossState()
    {
        return bossState;
    }

    private IEnumerator BossEntrance()
    {
        SetBossState((int)BossState.Idle);
        animator.SetTrigger("Angry_trigger");
        yield return new WaitForSeconds(4.0f);
        animator.SetTrigger("Idle_trigger");
        isEntering = false;
    }
}

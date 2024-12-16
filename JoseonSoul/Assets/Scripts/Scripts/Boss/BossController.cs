using Boss;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private BossLocomotionManager locomotionManager;
    private BossAttackManager attackManager;
    private Animator animator;
    private SoundManager soundManager;

    [SerializeField] private int bossState = 0;

    private bool isEntering = true;

    [SerializeField] private int maxHealth = 500;
    public int currentHealth = 500;

    private void Start()
    {
        locomotionManager = GetComponent<BossLocomotionManager>();
        attackManager = GetComponent<BossAttackManager>();
        animator = GetComponent<Animator>();
        soundManager = SoundManager.Instance;

        Stage_UIManager.Instance.UpdateBossNowHP(maxHealth, true);
        soundManager.SetLongRoarSound();

        StartCoroutine(BossEntrance());
    }

    private void Update()
    {
        if (isEntering) return;

        if (currentHealth <= 0)
        {
            DefeatBoss();
        }
        locomotionManager.HandleMovement();
        attackManager.HandleAttack();
    }


    void DefeatBoss()
    {
        animator.SetTrigger("Idle_trigger");
        animator.SetTrigger("death_trigger");

        Invoke("PlayEndingScene",3.0f);
        isEntering = true;
    }

    void PlayEndingScene()
    {
        GameManager.Instance.NextScene();
    }

    public void DestroyBoss()
    {
        Destroy(gameObject);
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Stage_UIManager.Instance.UpdateBossNowHP(-damage, false);
    }
}

using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player State")]
    [SerializeField] private int playerState = 0;

    private Animator animator;
    
    private PlayerLocomotionManager playerLocomotionManager;
    private PlayerAttackManager playerAttackManager;
    private PlayerJumpManager playerJumpManager;
    private PlayerRollingManager playerRollingManager;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if(animator == null)
            Debug.LogError("Animator Not Detected");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SyncAnimationState(){
        animator.SetInteger("player_state", playerState);
    }

    public void SetPlayerState(int state)
    {
        playerState = state;
        if (playerState == (int)PlayerState.Stunned || playerState == (int)PlayerState.Dead)
        {
            CancelPendingInvokes();
        }
        SyncAnimationState();
    }

    private void CancelPendingInvokes()
    {
        playerJumpManager.CancelInvoke();
        playerAttackManager.CancelAllAttacks(); // Needed to cancel queued attacks.
        // playerLocomotionManager.CancelInvoke(); This just invokes runnable on stamina refill
        playerRollingManager.CancelInvoke(); 
    }

    public int GetPlayerState()
    {
        return playerState;
    }
}

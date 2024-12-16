using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Unity.VisualScripting; // Ensure this namespace contains PlayerController and PlayerState

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class PlayerAttackManager : MonoBehaviour
{
    private PlayerController playerController;
    private SwordColliderManager swordColliderManager;
    private PlayerHealthManager healthManager;
    private SoundManager soundManager;

    private bool isQueued = false; // Indicates if the next attack is queued
    private float openPhaseDuration = 0.2f;   // Duration of the Open phase
    private float closedPhaseDuration = 0.8f; // Duration of the Closed phase
    private float attackOpenEndTime = 0f;     // Tracks when the Open phase ends
    private float staminaConsume = 35.0f;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        swordColliderManager = GetComponentInChildren<SwordColliderManager>();
        healthManager = GetComponent<PlayerHealthManager>();
        soundManager = SoundManager.Instance;

        if (playerController == null)
            Debug.LogError("PlayerController is null");
        if (swordColliderManager == null)
            Debug.LogError("SwordColliderManager is null");
        if(healthManager == null)
            Debug.LogError("Health Manager Not Detected");
    }

    void Update()
    {
        // Detect mouse click
        if (!Input.GetMouseButtonDown(0)) return;

        int state = playerController.GetPlayerState();

        float stamina = healthManager.getCurrentSP();

        // Handle initiating attacks
        if ((state == (int)PlayerState.Idle || state == (int)PlayerState.Moving) && !isQueued && stamina > staminaConsume)
        {
            Debug.Log("Initiating Attack1");
            Attack1();
        }
        // Handle queuing Attack2 during Attack1Open
        else if (state == (int)PlayerState.Attacking1Open && !isQueued && stamina > staminaConsume)
        {
            Debug.Log("Queuing Attack2");
            QueueNextAttack(PlayerState.Attacking2Closed);
        }
        // Handle queuing Attack3 during Attack2Open
        else if (state == (int)PlayerState.Attacking2Open && !isQueued && stamina > staminaConsume)
        {
            Debug.Log("Queuing Attack3");
            QueueNextAttack(PlayerState.Attacking3Closed);
        }
    }

    // ====================
    // Attack1 Sequence
    // ====================
    void Attack1()
    {
        playerController.SetPlayerState((int)PlayerState.Attacking1Closed);
        healthManager.updateCurrentSP(-staminaConsume,false);
        soundManager.SetKnifeSound();
        isQueued = false; // Reset any previous queue

        // Enable the sword collider
        swordColliderManager.EnableCollider();

        // Schedule transition to Attack1Open after 1.8 seconds
        Invoke(nameof(TransitionToAttack1Open), closedPhaseDuration);
    }

    void TransitionToAttack1Open()
    {
        playerController.SetPlayerState((int)PlayerState.Attacking1Open);
        attackOpenEndTime = Time.time + openPhaseDuration;
        // Schedule end of Attack1 after 0.2 seconds
        Invoke(nameof(EndAttack1), openPhaseDuration);
    }

    void EndAttack1()
    {
        playerController.SetPlayerState((int)PlayerState.Idle);
        Debug.Log("Attack1 Ended");

        // Disable the sword collider
        swordColliderManager.DisableCollider();

        isQueued = false; // Reset queued flag
    }

    // ====================
    // Attack2 Sequence
    // ====================
    void Attack2()
    {
        playerController.SetPlayerState((int)PlayerState.Attacking2Closed);
        healthManager.updateCurrentSP(-staminaConsume,false);
        Debug.Log("Attack2 Closed Started");
        soundManager.SetKnifeSound();
        isQueued = false;

        // Enable the sword collider
        swordColliderManager.EnableCollider();

        // Schedule transition to Attack2Open after 1.8 seconds
        Invoke(nameof(TransitionToAttack2Open), closedPhaseDuration);
    }

    void TransitionToAttack2Open()
    {
        playerController.SetPlayerState((int)PlayerState.Attacking2Open);
        Debug.Log("Attack2 Open Started");
        attackOpenEndTime = Time.time + openPhaseDuration;
        // Schedule end of Attack2 after 0.2 seconds
        Invoke(nameof(EndAttack2), openPhaseDuration);
    }

    void EndAttack2()
    {
        playerController.SetPlayerState((int)PlayerState.Idle);
        Debug.Log("Attack2 Ended");

        // Disable the sword collider
        swordColliderManager.DisableCollider();

        isQueued = false;
    }

    // ====================
    // Attack3 Sequence
    // ====================
    void Attack3()
    {
        playerController.SetPlayerState((int)PlayerState.Attacking3Closed);
        healthManager.updateCurrentSP(-staminaConsume,false);
        soundManager.SetKnifeSound();
        isQueued = false;

        // Enable the sword collider
        swordColliderManager.EnableCollider();

        // Schedule transition to Attack3Open after 1.8 seconds
        Invoke(nameof(TransitionToAttack3Open), closedPhaseDuration);
    }

    void TransitionToAttack3Open()
    {
        playerController.SetPlayerState((int)PlayerState.Attacking3Open);
        attackOpenEndTime = Time.time + openPhaseDuration;
        // Schedule end of Attack3 after 0.2 seconds
        Invoke(nameof(EndAttack3), openPhaseDuration);
    }

    void EndAttack3()
    {
        playerController.SetPlayerState((int)PlayerState.Idle);
        Debug.Log("Attack3 Ended");

        // Disable the sword collider
        swordColliderManager.DisableCollider();

        isQueued = false;
    }

    // ====================
    // Queuing Mechanism
    // ====================
    void QueueNextAttack(PlayerState nextState)
    {
        if (isQueued) return; // Prevent multiple queues
        isQueued = true;
        Debug.Log($"Queuing next attack ({nextState})");

        float remainingTime = attackOpenEndTime - Time.time;
        remainingTime = Mathf.Max(0f, remainingTime); // Ensure non-negative

        if (nextState == PlayerState.Attacking2Closed)
        {
            Invoke(nameof(Attack2), remainingTime);
            CancelInvoke(nameof(EndAttack1));
        }
        else if (nextState == PlayerState.Attacking3Closed)
        {
            Invoke(nameof(Attack3), remainingTime);
            CancelInvoke(nameof(EndAttack2));
        }
    }

    // ====================
    // Cancel All Invokes
    // ====================
    public void CancelAllAttacks()
    {
        Debug.Log("Cancelling all pending attacks.");

        // Cancel all Invokes
        CancelInvoke();

        // Disable the sword collider
        swordColliderManager.DisableCollider();

        // Reset attack flags
        isQueued = false;
    }

    // ====================
    // Clean-Up
    // ====================
    void OnDestroy()
    {
        // Cancel any pending Invokes to prevent errors if the object is destroyed
        CancelInvoke();
    }
}
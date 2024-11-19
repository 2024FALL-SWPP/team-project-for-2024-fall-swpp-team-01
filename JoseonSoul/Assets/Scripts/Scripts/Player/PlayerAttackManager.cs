using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player; // Ensure this namespace contains PlayerController and PlayerState

public class PlayerAttack : MonoBehaviour
{
    private PlayerController playerController;

    private bool isQueued = false; // Indicates if the next attack is queued
    private float openPhaseDuration = 0.2f;   // Duration of the Open phase
    private float closedPhaseDuration = 1.8f; // Duration of the Closed phase
    private float attackOpenEndTime = 0f;     // Tracks when the Open phase ends

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
            Debug.LogError("PlayerController is null");
    }

    void Update()
    {
        // Detect mouse click
        if (!Input.GetMouseButtonDown(0)) return;

        int state = playerController.GetPlayerState();

        // Handle initiating attacks
        if ((state == (int)PlayerState.Idle || state == (int)PlayerState.Moving) && !isQueued)
        {
            Debug.Log("Initiating Attack1");
            Attack1();
        }
        // Handle queuing Attack2 during Attack1Open
        else if (state == (int)PlayerState.Attacking1Open && !isQueued)
        {
            Debug.Log("Queuing Attack2");
            QueueNextAttack(PlayerState.Attacking2Closed);
        }
        // Handle queuing Attack3 during Attack2Open
        else if (state == (int)PlayerState.Attacking2Open && !isQueued)
        {
            Debug.Log("Queuing Attack3");
            QueueNextAttack(PlayerState.Attacking3Closed);
        }
        // Extend this pattern for more attacks if needed
    }

    // ====================
    // Attack1 Sequence
    // ====================
    void Attack1()
    {
        playerController.SetPlayerState((int)PlayerState.Attacking1Closed);
        isQueued = false; // Reset any previous queue
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
        isQueued = false; // Reset queued flag

        // If an attack was queued during Attack1Open, it will have been scheduled to execute via Invoke
    }

    // ====================
    // Attack2 Sequence
    // ====================
    void Attack2()
    {
        playerController.SetPlayerState((int)PlayerState.Attacking2Closed);
        isQueued = false;
        // Schedule transition to Attack2Open after 1.8 seconds
        Invoke(nameof(TransitionToAttack2Open), closedPhaseDuration);
    }

    void TransitionToAttack2Open()
    {
        playerController.SetPlayerState((int)PlayerState.Attacking2Open);
        attackOpenEndTime = Time.time + openPhaseDuration;
        // Schedule end of Attack2 after 0.2 seconds
        Invoke(nameof(EndAttack2), openPhaseDuration);
    }

    void EndAttack2()
    {
        playerController.SetPlayerState((int)PlayerState.Idle);
        Debug.Log("Attack2 Ended");
        isQueued = false;
    }

    // ====================
    // Attack3 Sequence
    // ====================
    void Attack3()
    {
        playerController.SetPlayerState((int)PlayerState.Attacking3Closed);
        isQueued = false;
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
        isQueued = false;
        // Further attacks can be queued here
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
        }
        else if (nextState == PlayerState.Attacking3Closed)
        {
            Invoke(nameof(Attack3), remainingTime);
        }
        // Add more conditions here if you have more attacks
    }

    // ====================
    // Cancel All Invokes
    // ====================
    /// <summary>
    /// Cancels all pending Invoke calls related to attack sequences.
    /// Resets the player's state to Idle and clears any queued attacks.
    /// </summary>
    public void CancelAllAttacks()
    {
        Debug.Log("Cancelling all pending attacks.");

        // Cancel all Invokes
        CancelInvoke();

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
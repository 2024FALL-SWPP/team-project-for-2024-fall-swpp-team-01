using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class PlayerAttackedManager : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerHealthManager healthManager;
    private bool hittable = false;
    private bool isBlocking = false;

    public Transform playerTransform; // Reference to the player's transform
    public float shieldBlockAngle = 120f; // Shield blocks attacks within this angle

    void Start()
    {
        // Get PlayerController and ensure it's not null
        playerController = GetComponent<PlayerController>();
        healthManager = GetComponent<PlayerHealthManager>();

        if (playerController == null)
            Debug.LogError("PlayerController not detected");

        if (healthManager == null)
            Debug.LogError("PlayerHealthManager not detected");

        if (playerTransform == null)
            playerTransform = transform; // Assign the player's transform if not provided
    }

    void Update()
    {
        // Update the hittable state based on the current player state
        int state = playerController.GetPlayerState();
        hittable = state != (int)PlayerState.RollingInvincible && state != (int)PlayerState.Dead;

        // Handle shield activation
        if (Input.GetMouseButton(1)) // Right mouse button held
        {
            if (!isBlocking)
            {
                ActivateShield();
            }
        }
        else if (isBlocking) // Right mouse button released
        {
            DeactivateShield();
        }
    }

    /// <summary>
    /// Activates the shield and updates the player state to Guarding.
    /// </summary>
    private void ActivateShield()
    {
        isBlocking = true;
        playerController.SetPlayerState((int)PlayerState.Guarding);
        Debug.Log("Shield activated.");
    }

    /// <summary>
    /// Deactivates the shield and resets the player state to Idle or Moving.
    /// </summary>
    private void DeactivateShield()
    {
        isBlocking = false;
        if (playerController.GetPlayerState() == (int)PlayerState.Guarding)
        {
            playerController.SetPlayerState((int)PlayerState.Idle);
        }
        Debug.Log("Shield deactivated.");
    }

    /// <summary>
    /// Handles collision with enemy attacks.
    /// </summary>
    /// <param name="collision">The collider of the object hitting the player.</param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("EnemyAttack") && hittable)
        {
            Vector3 attackerPosition = collision.transform.position;
            int damage = 0;
            EnemyAttackManager enemyAttackManager = collision.gameObject.GetComponent<EnemyAttackManager>();
            if (enemyAttackManager != null)
            {
                damage = enemyAttackManager.attackDamage;
            }
            if (IsAttackBlocked(attackerPosition))
            {
                Debug.Log("Attack blocked by the shield.");
                // Optional: Add effects or animations for blocking
                return; // Attack is blocked, no further damage logic needed
            }
            
            // If not blocked, handle damage
            HandleDamage(damage); // Adjust damage value as needed
        }
    }

    /// <summary>
    /// Checks if an attack is within the shield's blocking angle.
    /// </summary>
    /// <param name="attackerPosition">The position of the attacker.</param>
    /// <returns>True if the attack is blocked by the shield; false otherwise.</returns>
    private bool IsAttackBlocked(Vector3 attackerPosition)
    {
        if (!isBlocking) return false;

        Vector3 toAttacker = attackerPosition - playerTransform.position; // Vector from player to attacker
        Vector3 forward = playerTransform.forward; // Player's forward direction

        // Calculate the angle between the attacker's position and the player's forward direction
        float angle = Vector3.Angle(forward, toAttacker);

        // If the angle is within the shield block range, the attack is blocked
        return angle <= shieldBlockAngle * 0.5f;
    }

    /// <summary>
    /// Handles applying damage to the player and updating states.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    private void HandleDamage(int damage)
    {
        // Apply damage to the player's health
        healthManager.updateCurrentHP((float)-damage, false);

        // Check if the player's health has dropped to zero or below
        if (healthManager.getCurrentHP() <= 0)
        {
            // Set health to zero and update the player's state to Dead
            healthManager.updateCurrentHP(0f, true);
            playerController.SetPlayerState((int)PlayerState.Dead);

            Debug.Log("Player is dead.");
        }
        else
        {
            // Player is stunned if still alive
            playerController.SetPlayerState((int)PlayerState.Stunned);
            Debug.Log("Player is stunned after taking damage.");

            // Schedule the end of the stunned state
            Invoke(nameof(EndStunned), 1.06f);
        }
    }

    /// <summary>
    /// Ends the stunned state and returns the player to Idle.
    /// </summary>
    private void EndStunned()
    {
        // Only reset the player's state if they are currently stunned
        if (playerController.GetPlayerState() == (int)PlayerState.Stunned)
        {
            playerController.SetPlayerState((int)PlayerState.Idle);
            Debug.Log("Player recovered from stunned state.");
        }
    }
}
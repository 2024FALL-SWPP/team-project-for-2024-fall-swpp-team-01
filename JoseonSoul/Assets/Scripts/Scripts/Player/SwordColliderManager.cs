using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordColliderManager : MonoBehaviour
{
    private Collider swordCollider;

    [SerializeField] private int damage = 40;
    private void Start()
    {
        // Get the collider attached to the sword object
        swordCollider = GetComponent<Collider>();

        if (swordCollider == null)
        {
            Debug.LogError("Sword Collider is not attached!");
            return;
        }

        // Initially disable the collider
        swordCollider.enabled = false;
    }

    /// <summary>
    /// Enable the sword's collider when an attack starts.
    /// </summary>
    public void EnableCollider()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = true;
            Debug.Log("Sword Collider Enabled");
        }
    }

    /// <summary>
    /// Disable the sword's collider when the attack ends or hits an enemy.
    /// </summary>
    public void DisableCollider()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = false;
            Debug.Log("Sword Collider Disabled");
        }
    }

    /// <summary>
    /// Trigger event when the sword hits something.
    /// </summary>
    /// <param name="other">The object that was hit.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider hits an enemy
        if (other.CompareTag("EnemyBody"))
        {
            Debug.Log("Sword hit: " + other.gameObject.name);
    
            other.GetComponent<EnemyController>().TakeDamage(damage);

            // Disable the collider to prevent multiple hits in one attack
            DisableCollider();
        }
    }
}
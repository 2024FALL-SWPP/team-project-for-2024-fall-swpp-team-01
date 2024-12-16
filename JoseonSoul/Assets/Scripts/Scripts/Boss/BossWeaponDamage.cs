using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeaponDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger zone
        if (other.CompareTag("Player"))
        {
            int damage = 25;

            // Call the player's HandleEnemyAttack function
            PlayerAttackedManager playerController = other.gameObject.GetComponent<PlayerAttackedManager>();
            if (playerController != null)
            {
                playerController.HandleEnemyAttack(damage, gameObject); // Pass damage and attacker
            }
        }
    }
}

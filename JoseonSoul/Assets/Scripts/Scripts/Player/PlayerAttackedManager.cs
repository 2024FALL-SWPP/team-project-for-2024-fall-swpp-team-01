using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PlayerAttackedManager : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerController playerController;
    private PlayerHealthManager healthManager;
    private bool hittable = false;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if(playerController == null)
            Debug.LogError("Player Controller Not Detected");    }

    // Update is called once per frame
    void Update()
    {
        int state = playerController.GetPlayerState();
        hittable = state != (int) PlayerState.RollingInvincible && state != (int)PlayerState.Dead;
    }

    void onCollisionEnter(Collider other)
    {
        if (other.tag == "EnemyAttack" && hittable)
        {
            healthManager.updateCurrentHP(-5f, false);              // TODO Value subject to change
            if (healthManager.getCurrentHP() <= 0)
            {
                healthManager.updateCurrentHP(0f, true);
                playerController.SetPlayerState((int) PlayerState.Dead);
            }
            else
            {
                playerController.SetPlayerState((int)PlayerState.Stunned);
                Invoke("EndStunned", 2f); // Order important as SetPlayerState cancels pending invokes. 
            }
            

        }
    }

    void EndStunned()
    {
        playerController.SetPlayerState((int)PlayerState.Idle);
    }
}

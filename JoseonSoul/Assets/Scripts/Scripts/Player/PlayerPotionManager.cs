using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class PlayerPotionManager : MonoBehaviour
{
    public static int maxPotion = 4;
    private int curerntPotion;
    private GameManager gameManager;
    private PlayerController playerController;
    private PlayerHealthManager playerHealthManager;
    private Stage_UIManager UIManager;

    [SerializeField] private float healingDelayTime = 0.5f;
    [SerializeField] private float healingTime = 1.0f;
    [SerializeField] private int healingAmount = 80;
    
    void Start()
    {
        gameManager = GameManager.Instance;
        if(gameManager == null)
            Debug.LogError("Game Manager Not Detected");
        
        playerController = PlayerController.Instance.GetComponent<PlayerController>();
        if(playerController == null)
            Debug.LogError("Player Controller Not Detected");

        playerHealthManager = PlayerController.Instance.GetComponent<PlayerHealthManager>();
        if(playerHealthManager == null)
            Debug.LogError("Health Manager Not Detected");

        UIManager = Stage_UIManager.Instance;
        if(UIManager == null)
            Debug.LogError("UI Manager Not Detected");

        curerntPotion = maxPotion;
    }

    // Update is called once per frame
    void Update()
    {
        int state = playerController.GetPlayerState();
        if( state == (int)PlayerState.Idle || state == (int)PlayerState.Moving )
        {
            if(Input.GetKeyDown(KeyCode.R))
                HandlePotion();
        }
    }

    void HandlePotion()
    {
        playerController.SetPlayerState((int)PlayerState.HealingDelay);
        Invoke("Heal",healingDelayTime);
    }

    void Heal()
    {
        if(curerntPotion > 0)
        {
            curerntPotion -=1 ;
            playerHealthManager.updateCurrentHP(healingAmount, false);
        }
        
        gameManager.SetPlayerStatus(playerHealthManager.getCurrentHP(),curerntPotion);
        UIManager.
        
        Invoke("EndHeal", healingTime);
    }

    void EndHeal()
    {
        playerController.SetPlayerState((int)PlayerState.Idle);
    }

    public int getCurrentPotion()
    {
        return curerntPotion;
    }
}

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

    [SerializeField] private float healingDelayTime = 1.0f;
    [SerializeField] private float healingTime = 1.5f;
    [SerializeField] private int healingAmount = 80;

    private Transform potionTransform;
    private Transform shieldTransform;
    
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

        potionTransform = FindChildByName(PlayerController.Instance.transform, "potion");
        shieldTransform = FindChildByName(PlayerController.Instance.transform, "shield");

        if(potionTransform == null)
            Debug.LogError("Potion Not Detected");
        if(shieldTransform == null)
            Debug.LogError("Shield Not Detected");

        potionTransform.gameObject.SetActive(false);

        curerntPotion = maxPotion;
    }

    Transform FindChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            Transform found = FindChildByName(child, name);
            if (found != null)
                return found;
        }
        return null;
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

        if( state == (int)PlayerState.Stunned )
        {
            potionTransform.gameObject.SetActive(false);
            shieldTransform.gameObject.SetActive(true);
        }
    }

    void HandlePotion()
    {
        potionTransform.gameObject.SetActive(true);
        shieldTransform.gameObject.SetActive(false);
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
        UIManager.PotionSync();
        
        Invoke("EndHeal", healingTime);
    }

    void EndHeal()
    {
        potionTransform.gameObject.SetActive(false);
        shieldTransform.gameObject.SetActive(true);
        playerController.SetPlayerState((int)PlayerState.Idle);
    }

    public int getCurrentPotion()
    {
        return curerntPotion;
    }

    public void updateCurrentPotion(int potion)
    {
        curerntPotion = potion;
        UIManager.PotionSync();
    }
}

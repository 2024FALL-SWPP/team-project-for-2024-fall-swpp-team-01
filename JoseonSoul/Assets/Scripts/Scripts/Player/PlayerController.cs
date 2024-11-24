using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player State")]
    [SerializeField] private int playerState = 0;
    [Header("Player Managers")]
    public PlayerHealthManager playerHealthManager;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if(animator == null)
            Debug.LogError("Animator Not Detected");
        
        playerHealthManager = GetComponent<PlayerHealthManager>();
        if(playerHealthManager == null)
            Debug.LogError("Health Manager Not Detected");
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
        SyncAnimationState();
    }

    public int GetPlayerState()
    {
        return playerState;
    }

    public void InitPlayer(float currnetHP, int potionRemained, Vector3 position)
    {
        transform.position = position;
        playerHealthManager.updateCurrentHP(currnetHP,true);
        //playerPotionmanager.setPotionRemained(potionRemained);
    }
}

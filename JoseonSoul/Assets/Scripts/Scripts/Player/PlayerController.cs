using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player State")]
    [SerializeField] private int playerState = 0;
    public static GameObject Instance { get; private set; }
    
    [Header("Player Managers")]
    public PlayerHealthManager playerHealthManager;
    private Animator animator;
    
    [SerializeField] private PlayerLocomotionManager playerLocomotionManager;
    [SerializeField] private PlayerAttackManager playerAttackManager;
    [SerializeField] private PlayerJumpManager playerJumpManager;
    [SerializeField] private PlayerRollingManager playerRollingManager;
    [SerializeField] private PlayerPotionManager playerPotionManager;
    // Start is called before the first frame update

    void Awake()
    {
        if (Instance == null)
        {
            Instance = gameObject;
            DontDestroyOnLoad(gameObject);  // 씬 전환 시에도 객체 유지
        }
        else
        {
            Destroy(gameObject);  // 이미 인스턴스가 있으면 새로 생성된 오브젝트 삭제
            return;
        }
    }

    void Start()
    {
        animator = Instance.GetComponent<Animator>();
        if(animator == null)
            Debug.LogError("Animator Not Detected");
        
        playerHealthManager = Instance.GetComponent<PlayerHealthManager>();
        if(playerHealthManager == null)
            Debug.LogError("Health Manager Not Detected");

        playerPotionManager = Instance.GetComponent<PlayerPotionManager>();
        if(playerPotionManager == null)
            Debug.LogError("Potion Manager Not Detected");

        playerLocomotionManager = Instance.GetComponent<PlayerLocomotionManager>();
        if(playerLocomotionManager == null)
            Debug.LogError("Locomotion Manager Not Detected");

        playerAttackManager = Instance.GetComponent<PlayerAttackManager>();
        if(playerAttackManager == null)
            Debug.LogError("Attack Manager Not Detected");

        playerJumpManager = Instance.GetComponent<PlayerJumpManager>();
        if(playerJumpManager == null)
            Debug.LogError("Jump Manager Not Detected");

        playerRollingManager = Instance.GetComponent<PlayerRollingManager>();
        if(playerRollingManager == null)
            Debug.LogError("Rolling Manager Not Detected");
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
        playerPotionManager.CancelInvoke();
        
    }


    public int GetPlayerState()
    {
        return playerState;
    }

    public void InitPlayer(float currnetHP, int potionRemained, Vector3 position)
    {
        transform.position = position;
        playerHealthManager.updateCurrentHP(currnetHP,true);
        playerPotionManager.updateCurrentPotion(potionRemained);
        SetPlayerState((int)PlayerState.Idle);
    }
}

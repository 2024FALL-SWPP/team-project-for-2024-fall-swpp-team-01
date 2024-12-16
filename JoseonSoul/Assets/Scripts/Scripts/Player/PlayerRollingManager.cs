using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class PlayerRollingManager : MonoBehaviour
{
    [Header("Rolling Settings")]
    [SerializeField] private float staminaDecrement = 20.0f;
    [SerializeField] private float invincibleTime = 0.9f;
    [SerializeField] private float totalTime = 1.1f;
    [SerializeField] private float rollingSpeed = 7.0f;

    [SerializeField] private Vector3 rollingDirection;

    private PlayerHealthManager healthManager;
    private PlayerController playerController;
    private PlayerLocomotionManager locomotionManager;

    private bool rollable;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if(playerController == null)
            Debug.LogError("Player Controller Not Detected");

        healthManager = GetComponent<PlayerHealthManager>();
        if(healthManager == null)
            Debug.LogError("Health Manager Not Detected");

        locomotionManager = GetComponent<PlayerLocomotionManager>();
        if(locomotionManager == null)
            Debug.LogError("Locomotion Manager Not Detected");
    }

    // Update is called once per frame
    void Update()
    {
        rollable = playerController.GetPlayerState() == 0 || playerController.GetPlayerState() == 1;
        rollable = rollable && healthManager.getCurrentSP() > staminaDecrement;
        if(Input.GetKey(KeyCode.Space) && rollable){
            Roll();
        }

        if(playerController.GetPlayerState() == (int)PlayerState.RollingHittable || playerController.GetPlayerState() == (int)PlayerState.RollingInvincible)
        {
            MovePlayer(rollingDirection);
        }
    }

    void Roll()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        rollingDirection = new Vector3(horizontal, 0f, vertical).normalized;
        if (rollingDirection.magnitude <= 0.1f)
            rollingDirection = transform.forward;
        else
            rollingDirection = locomotionManager.CalculateMoveDirection(horizontal,vertical);
        // **Immediate Rotation to Rolling Direction**
        if (rollingDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rollingDirection);
            transform.rotation = targetRotation;
        }

        healthManager.updateCurrentSP(-staminaDecrement,false);
        InvincibleRoll();
        Invoke("HittableRoll", invincibleTime);
        Invoke("EndRoll", totalTime);
    }

    void InvincibleRoll()
    {
        playerController.SetPlayerState((int)PlayerState.RollingInvincible);
    }

    void HittableRoll()
    {
        playerController.SetPlayerState((int)PlayerState.RollingHittable);
    }
    
    void EndRoll()
    {
        playerController.SetPlayerState((int)PlayerState.Idle);
    }

    void MovePlayer(Vector3 moveDirection)
    {
        Vector3 movement = rollingSpeed * Time.deltaTime * moveDirection;
        transform.position += movement;
    }

    public void CancleRoll() // Call this function when hitted during hittable roll (state 9)
    {
        CancelInvoke("EndRoll");
    }
}

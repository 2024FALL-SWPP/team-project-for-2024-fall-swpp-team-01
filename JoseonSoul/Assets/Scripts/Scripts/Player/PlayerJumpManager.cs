using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PlayerJumpManager : MonoBehaviour
{
    [Header("Jump Setting")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float walkJumpSpeed = 5.0f;
    [SerializeField] private float runJumpSpeed = 10.0f;
    private PlayerController playerController;
    private PlayerLocomotionManager playerLocomotionManager;
    private bool jumpable;
    private float speed;
    private Rigidbody rb;
    private float movingSpeed;
    private Vector3 jumpDirection;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if(playerController == null)
            Debug.LogError("Player Controller Not Detected");

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        if(playerLocomotionManager == null)
            Debug.LogError("Player Locomotion Manager Not Detected");

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        jumpable = playerController.GetPlayerState() == 0 || playerController.GetPlayerState() == 1;
        if(Input.GetKeyDown(KeyCode.F) && jumpable){
            ReadyToJump();
        }

        if(playerController.GetPlayerState() == (int)PlayerState.Jumping)
        {
            MovePlayer(jumpDirection, speed);
        }
    }

    void ReadyToJump()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        jumpDirection = new Vector3(horizontal, 0f, vertical).normalized;
        if (jumpDirection.magnitude <= 0.1f)
            jumpDirection = transform.forward;
        else
            jumpDirection = playerLocomotionManager.CalculateMoveDirection(horizontal,vertical);
        if(playerController.GetPlayerState() == 0)
            speed = 0;
        else
        {
            if(playerLocomotionManager.getIsRunning())
                speed = runJumpSpeed;
            else
                speed = walkJumpSpeed;
        }
        playerController.SetPlayerState((int)PlayerState.JumpingDelay);
        Invoke("Jump",0.4f);
    }

    void Jump()
    {
        playerController.SetPlayerState((int)PlayerState.Jumping);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground") && playerController.GetPlayerState() == (int)PlayerState.Jumping)
        {
            playerController.SetPlayerState((int)PlayerState.Idle);
        }
    }

    void CancelJump(int state) // 점프 도중 타격 당하면 사용, state는 타격당한 시점의 state
    {
        if(state == (int)PlayerState.JumpingDelay)
            CancelInvoke("Jump");
        if(state == (int)PlayerState.Jumping)
        {
            //TODO
        }
    }

    void MovePlayer(Vector3 moveDirection, float speed)
    {
        Vector3 movement = speed * Time.deltaTime * moveDirection;
        transform.position += movement;
    }
}

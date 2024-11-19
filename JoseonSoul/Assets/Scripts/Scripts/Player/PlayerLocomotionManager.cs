using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private bool movable = true;
    [SerializeField] private const float walkingSpeed = 5f;
    [SerializeField] private const float runningSpeed = 10f;
    [SerializeField] private const float staminaDecrementSpeed = 25f;

    private bool runnable = true;
    private bool isRunning = false;
    private float speed = walkingSpeed;                   // Movement speed
    private float rotationSpeed = 720f;         // Degrees per second for rotation
    public TextMeshProUGUI textMesh;
    
    [Header("Camera Reference")]
    public Transform cameraTransform;           // Reference to the camera's transform

    private Animator animator;
    private PlayerHealthManager healthManager;
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if(playerController == null)
            Debug.LogError("Player Controller Not Detected");
        
        // If cameraTransform is not assigned, find the main camera
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
            if (cameraTransform == null)
            {
                Debug.LogError("PlayerLocomotionManager: No camera found. Please assign a Camera to cameraTransform.");
            }
        }

        animator = GetComponent<Animator>();
        if(animator == null)
            Debug.LogError("No Animator Detected");

        healthManager = GetComponent<PlayerHealthManager>();
        if(healthManager == null)
            Debug.LogError("No HealthManager Detected");
    }

    void Update()
    {
        movable = playerController.GetPlayerState() == 0 || playerController.GetPlayerState() == 1;
        if (movable)
        {
            HandleMovement();
        }
        else
        {
            isRunning = false;
        }
        if(healthManager.getCurrentSP() <= 1.0f){
            runnable = false;
            Invoke("enableRunning", 2.0f);
        }
    }

   

    /// <summary>
    /// Handles player movement and rotation based on input and camera orientation.
    /// </summary>
    void HandleMovement()
    {
        // Capture input axes
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right arrows
        float vertical = Input.GetAxisRaw("Vertical");     // W/S or Up/Down arrows
        // Check if there is any movement input
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            // Calculate movement direction relative to the camera
            Vector3 moveDirection = CalculateMoveDirection(horizontal, vertical);

            SetIsRunning();
            SetSpeed();
            playerController.SetPlayerState((int)PlayerState.Moving);
            SetAnimation();
            MovePlayer(moveDirection);
            RotatePlayer(moveDirection);
            if (isRunning)
            {
                DecrementStamina();
            }
        }
        // Optionally, handle idle state when there's no input
        else
        {
            playerController.SetPlayerState((int)PlayerState.Idle);
            //animator.SetInteger("player_state", (int)PlayerState.Idle);
            // Implement idle behavior if needed (e.g., stop movement animations)
        }
    }

    private void SetSpeed()
    {
        speed = isRunning ? runningSpeed : walkingSpeed;
    }

    private void SetAnimation()
    {
        if(isRunning)
        {
            animator.SetBool("run", true);
            animator.SetBool("walk", false);
        }
        else
        {
            animator.SetBool("walk",true);   
            animator.SetBool("run",false);
        }
        return; 
    }

    private void SetIsRunning() // Ran in movable state.
    {
        if (runnable)
            isRunning = Input.GetKey(KeyCode.LeftShift) ? true : false;

        else isRunning = false;
    }

    private void enableRunning()
    {
        runnable = true;
    }

    private void DecrementStamina()
    { 
        healthManager.updateCurrentSP(-staminaDecrementSpeed * Time.deltaTime, false);
    }

    /// <summary>
    /// Calculates the movement direction based on input and camera orientation.
    /// </summary>
    /// <param name="horizontal">Horizontal input axis.</param>
    /// <param name="vertical">Vertical input axis.</param>
    /// <returns>Normalized movement direction vector.</returns>
    public Vector3 CalculateMoveDirection(float horizontal, float vertical)
    {
        // Get the camera's forward and right vectors
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Project vectors onto the horizontal plane (y = 0)
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate the desired movement direction
        Vector3 desiredMoveDirection = cameraForward * vertical + cameraRight * horizontal;

        // Normalize the movement direction to ensure consistent speed
        if (desiredMoveDirection.magnitude > 1f)
        {
            desiredMoveDirection.Normalize();
        }

        return desiredMoveDirection;
    }

    /// <summary>
    /// Moves the player character based on the calculated movement direction.
    /// </summary>
    /// <param name="moveDirection">Normalized movement direction vector.</param>
    void MovePlayer(Vector3 moveDirection)
    {
        // Calculate movement vector
        Vector3 movement = speed * Time.deltaTime * moveDirection;

        // Apply movement to the player's position
        transform.position += movement;
    }

    /// <summary>
    /// Rotates the player character smoothly towards the movement direction.
    /// </summary>
    /// <param name="moveDirection">Normalized movement direction vector.</param>
    void RotatePlayer(Vector3 moveDirection)
    {
        // Calculate the target rotation based on movement direction
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public bool getIsRunning()
    {
        return isRunning;
    }
}
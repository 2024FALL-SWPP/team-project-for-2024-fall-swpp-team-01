// ThirdPersonCameraController.cs
using Unity.VisualScripting;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("The player character the camera will follow.")]
    public Transform target;

    [Header("Rotation Settings")]
    [Tooltip("Rotation speed for mouse/controller input.")]
    public float rotationSpeed = 5.0f;
    [Tooltip("Minimum vertical angle (in degrees).")]
    public float minYAngle = 0f;
    [Tooltip("Maximum vertical angle (in degrees).")]
    public float maxYAngle = 30f;

    [Header("Zoom Settings")]
    [Tooltip("Default distance from the target.")]
    public float defaultDistance = 5.0f;
    [Tooltip("Minimum allowable distance.")]
    public float minDistance = 2.0f;
    [Tooltip("Maximum allowable distance.")]
    public float maxDistance = 10.0f;
    [Tooltip("Zoom speed.")]
    public float zoomSpeed = 2.0f;

    [Header("Smoothing Settings")]
    [Tooltip("Time it takes to smooth the camera movement.")]
    public float smoothTime = 0.2f;

    [Header("Collision Settings")]
    [Tooltip("Enable camera collision to prevent clipping through objects.")]
    public bool enableCollision = true;
    [Tooltip("Layers that the camera will detect for collision.")]
    public LayerMask collisionLayers;
    [Tooltip("Offset to prevent camera from being too close to collision objects.")]
    public float collisionOffset = 0.2f;

    // Private variables
    private float yaw = 0.0f;    // Horizontal rotation
    private float pitch = 0.0f;  // Vertical rotation
    private float currentDistance;
    private float desiredDistance;
    private float distanceSmoothVelocity = 0.0f;
    private Vector3 currentVelocity = Vector3.zero;

    public static ThirdPersonCameraController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
        if (target == null)
        {
            // Attempt to find the player by tag if target not set
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogError("ThirdPersonCameraController: No target set and no GameObject tagged 'Player' found.");
            }
        }

        // Initialize distance
        currentDistance = defaultDistance;
        desiredDistance = defaultDistance;

        // Initialize rotation based on current camera orientation
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        HandleInput();
        HandleZoom();
        CalculateDesiredPosition();
        ApplyCollision();
        //SmoothCameraMovement();
    }

    /// <summary>
    /// Handles mouse or controller input for rotating the camera.
    /// </summary>
    private void HandleInput()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        yaw += mouseX * rotationSpeed;
        pitch -= mouseY * rotationSpeed;
        pitch = Mathf.Clamp(pitch, minYAngle, maxYAngle);
    }

    /// <summary>
    /// Handles zoom input to adjust the camera distance.
    /// </summary>
    private void HandleZoom()
    {
        // Get zoom input (mouse scroll wheel)
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            desiredDistance -= scrollInput * zoomSpeed;
            desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        }

        // Smoothly interpolate the distance
        currentDistance = Mathf.SmoothDamp(currentDistance, desiredDistance, ref distanceSmoothVelocity, smoothTime);
    }

    /// <summary>
    /// Calculates the desired camera position based on rotation and distance.
    /// </summary>
    private void CalculateDesiredPosition()
    {
        // Convert yaw and pitch to a quaternion
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Calculate the desired position
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * currentDistance);

        // Set the desired position (excluding collision handling for now)
        transform.position = desiredPosition;

        // Always look at the target
        transform.LookAt(target.position + Vector3.up * 1.5f); // Adjust the look target height as needed
    }

    /// <summary>
    /// Applies collision detection to prevent the camera from clipping through objects.
    /// </summary>
    private void ApplyCollision()
    {
        if (!enableCollision)
            return;

        Vector3 direction = (transform.position - target.position).normalized;
        float targetDistance = currentDistance;

        // Raycast to check for collision
        RaycastHit hit;
        if (Physics.Raycast(target.position, direction, out hit, currentDistance, collisionLayers))
        {
            // Adjust the camera distance based on collision
            targetDistance = hit.distance - collisionOffset;
            targetDistance = Mathf.Clamp(targetDistance, minDistance, currentDistance);
        }

        // Update the camera position based on collision
        Vector3 collisionPosition = target.position + direction * targetDistance;
        transform.position = collisionPosition;
    }

    /// <summary>
    /// Smoothly moves the camera towards the desired position.
    /// </summary>
    private void SmoothCameraMovement()
    {
        // Smooth the rotation by interpolating the yaw and pitch
        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
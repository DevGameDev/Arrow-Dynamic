using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    //////////////////////////////////////////////////
    // Public Properties and Methods
    //////////////////////////////////////////////////

    // Components
    public Rigidbody rb;
    public Transform camTransform;
    public CapsuleCollider col;

    public SpawnPoints initialRespawnPoint;
    public RespawnPoint tutorialStartPoint;
    public RespawnPoint levelOneStartPoint;
    public RespawnPoint levelTwoStartPoint;
    public RespawnPoint jungleStartPoint;
    public RespawnPoint voidStartPoint;

    public SpawnPoints lastSpawnPoint = SpawnPoints.TutorialStart;

    public static PlayerController Instance { get; set; }

    public void SetSpawnPoint(SpawnPoints pointType)
    {
        lastSpawnPoint = pointType;
    }

    public void RespawnPoint()
    {
        transform.position = points[lastSpawnPoint].transform.position;
    }

    public void SpawnPlayer(SpawnPoints pointType)
    {
        RespawnPoint point = points[pointType];

        transform.position = point.transform.position;
    }

    public void HandleMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void HandleLook(InputAction.CallbackContext context)
    {
        if (disabled)
            return;

        Vector2 mouseDelta = context.ReadValue<Vector2>() * mouseSensitivity;

        mousePosition = Vector2.Lerp(mousePosition, mousePosition + mouseDelta, 1.0f / viewSmoothing);

        mousePosition.y = Mathf.Clamp(mousePosition.y, -60.0f, 45.0f); // Don't allow 360 y-axis
    }

    public void HandleJump(InputAction.CallbackContext context)
    {
        if (disabled) return;

        if (context.performed)
        {
            if (IsGrounded())
            {
                canDoubleJump = true;
                isJumping = true;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else if (canDoubleJump)
            {
                Vector3 currentVelocity = rb.velocity;
                currentVelocity.y = 0; // reset y velocity before so jump always feels the same
                rb.velocity = currentVelocity;
                canDoubleJump = false;
                isJumping = true;
                rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
            }
        }
    }


    public void HandleSprint(InputAction.CallbackContext context)
    {
        if (disabled) return;

        if (context.performed) isSprinting = true;
        else if (context.canceled) isSprinting = false;
    }

    public void HandleCrouch(InputAction.CallbackContext context)
    {
        if (disabled) return;

        if (context.performed)
        {
            camBaseHeight = crouchHeight;
            camTransform.localPosition = new Vector3(camTransform.localPosition.x, camTransform.localPosition.y - (standingHeight - crouchHeight), camTransform.localPosition.z);
            isCrouching = true;
        }
        else if (context.canceled)
        {
            camBaseHeight = standingHeight;
            camTransform.localPosition = new Vector3(camTransform.localPosition.x, camTransform.localPosition.y + (standingHeight - crouchHeight), camTransform.localPosition.z);
            isCrouching = false;
        }
    }

    public void HandlePull(InputAction.CallbackContext context)
    {
        if (disabled) return;

        if (context.performed) isAiming = true;
        else if (context.canceled) isAiming = false;
    }

    public void HandleCancel(InputAction.CallbackContext context)
    {
        if (disabled) return;

        if (context.performed) isAiming = false;
    }

    public void HandleArrowWheel(InputAction.CallbackContext context)
    {
    }

    public IEnumerator ShakeCamera(float duration, float magnitude, float speed)
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0.0f;
        float randomStart = UnityEngine.Random.Range(-1000f, 1000f);

        while (elapsed < duration)
        {
            float x = originalPosition.x + (Mathf.PerlinNoise(randomStart, elapsed * speed) * magnitude * 2 - magnitude);
            float y = originalPosition.y + (Mathf.PerlinNoise(randomStart + 100, elapsed * speed) * magnitude * 2 - magnitude);

            transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }

    //////////////////////////////////////////////////
    // Private Fields and Methods
    //////////////////////////////////////////////////

    // State
    private Vector2 move = Vector2.zero;
    private Vector2 mousePosition = Vector2.zero;
    // private float currentSpeed = 0f;
    private bool canDoubleJump = false;
    private bool isJumping = false;
    private bool isSprinting = false;
    private bool isCrouching = false;
    private bool isAiming = false;
    private float bobTimer = 0;
    private float camBaseHeight;
    public static bool disabled = false;
    public bool gravityArrowActive = false;
    public bool timeArrowActive = false;
    public bool arrowWheelActive = false;
    public bool timeSlowed = false;

    private Dictionary<SpawnPoints, RespawnPoint> points = new Dictionary<SpawnPoints, RespawnPoint>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            GameManager.Instance.HandleGameQuit(false, "Duplicate PlayerControllers");
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Update settings and register for further updates
        UpdateSettings();
        GameSettings.OnSettingsChanged += UpdateSettings;

        camBaseHeight = standingHeight;

        points[SpawnPoints.TutorialStart] = tutorialStartPoint;
        points[SpawnPoints.LevelOneStart] = levelOneStartPoint;
        points[SpawnPoints.LevelTwoStart] = levelTwoStartPoint;
        points[SpawnPoints.JungleStart] = jungleStartPoint;
        points[SpawnPoints.VoidStart] = voidStartPoint;
    }

    float currMaxSpeed;
    private void Update()
    {
        if (arrowWheelActive || timeArrowActive)
            Time.timeScale = 0.5f;
        else Time.timeScale = 1f;

        if (WeaponWheelController.Instance.open)
        {
            disabled = true;
            return;
        }
        else disabled = false;

        float sideways = move.x;
        float forward = move.y;

        if (move.magnitude == 0) return;

        var inputDir = new Vector3(sideways, 0, forward).normalized;
        var velocity = ((transform.forward * inputDir.z) + (transform.right * inputDir.x)) * speed;

        if (isSprinting && forward > 0) // Only sprint when moving forward
            velocity *= sprintSpeedMultiplier;

        if (forward < 0) velocity *= reverseSpeedMultiplier;
        else if (Mathf.Abs(sideways) > Mathf.Abs(forward)) velocity *= sidewaysSpeedMultiplier;

        if (isCrouching) velocity *= crouchSpeedMultiplier;

        if (isAiming) velocity *= aimSpeedMultiplier;

        if (!IsGrounded())
            velocity = ((1 - airSpeedMultiplier) * rb.velocity) + (airSpeedMultiplier * velocity); // Preserve momentum

        // Preserve vertical velocity (gravity)
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;

    }

    private void LateUpdate()
    {
        // Look
        camTransform.localRotation = Quaternion.Euler(-mousePosition.y, 0, 0);
        transform.localRotation = Quaternion.Euler(0, mousePosition.x, 0);
        // camTransform.localRotation = Quaternion.Slerp(camTransform.localRotation, Quaternion.Euler(-mousePosition.y, 0, 0), Time.deltaTime * viewSmoothing);
        // transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, mousePosition.x, 0), Time.deltaTime * viewSmoothing);

        // Apply camera bobbing if enabled and player is moving
        if (enableCameraBobbing && IsGrounded() && rb.velocity.magnitude > bobbingMinSpeed)
        {
            float currentBobbingSpeed = bobbingSpeed * (rb.velocity.x + rb.velocity.z) * Time.deltaTime;
            bobTimer = bobTimer + currentBobbingSpeed;
            float waveslice = Mathf.Sin(bobTimer);

            if (bobTimer > Mathf.PI * 2)
            {
                bobTimer = bobTimer - (Mathf.PI * 2);
            }

            Vector3 localPos = camTransform.localPosition;
            localPos.y = Mathf.Lerp(localPos.y, camBaseHeight + waveslice * bobbingAmount, viewSmoothing);
            camTransform.localPosition = localPos; // Set the new position
            Bow.Instance.SetBobOffset(waveslice * bobbingAmount);
        }
        else if (!enableCameraBobbing)
        {
            bobTimer = 0;
            Vector3 localPos = camTransform.localPosition;
            localPos.y = Mathf.Lerp(localPos.y, camBaseHeight, viewSmoothing);
            camTransform.localPosition = localPos;
        }
    }

    private void OnDestroy()
    {
        GameSettings.OnSettingsChanged -= UpdateSettings;
    }

    private bool IsGrounded()
    {
        Vector3 groundCheckPosition = transform.position;
        groundCheckPosition.y -= col.bounds.extents.y;
        return Physics.CheckSphere(groundCheckPosition, groundCheckRadius, groundMask);
    }

    private float pushForce = 500f;
    void OnCollision(Collision collision)
    {
        // Check if the collision is with a wall
        if (collision.gameObject.tag == "environment")
        {
            // Calculate the direction to push the player
            Vector3 pushDirection = collision.contacts[0].normal;
            // Apply the push force
            GetComponent<Rigidbody>().AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }

    //////////////////////////////////////////////////
    // Settings
    //////////////////////////////////////////////////

    private float speed;
    private float maxSpeed;
    private float reverseSpeedMultiplier;
    private float sidewaysSpeedMultiplier;
    private float sprintSpeedMultiplier;
    private float crouchSpeedMultiplier;
    private float airSpeedMultiplier;
    private float aimSpeedMultiplier;

    private float standingHeight;
    private float crouchHeight;

    private float jumpForce;
    private float doubleJumpForce;
    private float fallMultiplier;
    private float lowJumpMultiplier;
    private float groundCheckRadius;
    private LayerMask groundMask;

    private float viewSmoothing;
    private bool enableCameraBobbing;
    private float bobbingSpeed;
    private float bobbingAmount;
    private float bobbingMinSpeed;
    private float mouseSensitivity;

    private void UpdateSettings()
    {
        GameSettings settings = GameManager.GetSettings();

        speed = settings.gameplay.speed;
        maxSpeed = settings.gameplay.maxVelocity;
        reverseSpeedMultiplier = settings.gameplay.reverseSpeedMultiplier;
        sidewaysSpeedMultiplier = settings.gameplay.sidewaysSpeedMultiplier;
        sprintSpeedMultiplier = settings.gameplay.sprintSpeedMultiplier;
        crouchSpeedMultiplier = settings.gameplay.crouchSpeedMultiplier;
        airSpeedMultiplier = settings.gameplay.airSpeedMultiplier;
        aimSpeedMultiplier = settings.gameplay.aimSpeedMultiplier;
        standingHeight = settings.gameplay.standingHeight;
        crouchHeight = settings.gameplay.crouchHeight;

        jumpForce = settings.gameplay.jumpForce;
        doubleJumpForce = settings.gameplay.doubleJumpForce;
        fallMultiplier = settings.gameplay.fallMultiplier;
        lowJumpMultiplier = settings.gameplay.lowJumpMultiplier;
        groundCheckRadius = settings.gameplay.groundCheckRadius;
        groundMask = settings.gameplay.groundMask;

        viewSmoothing = settings.display.viewSmoothing;
        enableCameraBobbing = settings.display.enableCameraBobbing;
        bobbingSpeed = settings.display.bobbingSpeed;
        bobbingAmount = settings.display.bobbingAmount;
        bobbingMinSpeed = settings.display.bobbingMinSpeed;
        mouseSensitivity = settings.input.mouseSensitivity;
    }
}
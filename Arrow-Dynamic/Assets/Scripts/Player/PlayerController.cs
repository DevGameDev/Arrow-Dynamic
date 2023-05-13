using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    //////////////////////////////////////////////////
    // Public Properties and Methods
    //////////////////////////////////////////////////

    public static PlayerController Instance { get; set; }

    public void HandleMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void HandleLook(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>() * mouseSensitivity;

        mousePosition = Vector2.Lerp(mousePosition, mousePosition + mouseDelta, 1.0f / viewSmoothing);

        mousePosition.y = Mathf.Clamp(mousePosition.y, -60.0f, 45.0f); // Don't allow 360 y-axis
    }

    public void HandleJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (IsGrounded())
            {
                canDoubleJump = true;
                isJumping = true;
            }
            else if (canDoubleJump)
            {
                Vector3 currentVelocity = rb.velocity;
                currentVelocity.y = 0; // reset y velocity before so jump always feels the same
                rb.velocity = currentVelocity;
                canDoubleJump = false;
                isJumping = true;
            }
        }
    }

    public void HandleSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.started ? true : false;
    }

    public void HandleCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            col.height = crouchHeight;
            isCrouching = true;
        }
        else if (context.performed)
        {
            col.height = standingHeight;
            isCrouching = false;
        }
    }

    public void HandlePull(InputAction.CallbackContext context)
    {
        isAiming = context.started ? true : false;
    }

    public void HandleCancel(InputAction.CallbackContext context)
    {
        if (context.performed) isAiming = false;
    }

    public void HandleArrowWheel(InputAction.CallbackContext context)
    {
    }

    //////////////////////////////////////////////////
    // Private Fields and Methods
    //////////////////////////////////////////////////

    // Components
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform camTransform;
    [SerializeField] private CapsuleCollider col;

    // State
    private Vector2 move = Vector2.zero;
    private Vector3 targetVelocity = Vector3.zero;
    private Vector2 mousePosition = Vector2.zero;
    private bool canDoubleJump = false;
    private bool isJumping = false;
    private bool isSprinting = false;
    private bool isCrouching = false;
    private bool isAiming = false;
    private float bobTimer;

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
    }

    private void Update()
    {
        // Look
        camTransform.localRotation = Quaternion.AngleAxis(-mousePosition.y, Vector3.right);
        transform.localRotation = Quaternion.AngleAxis(mousePosition.x, Vector3.up);

        // Apply camera bobbing if enabled and player is moving
        float playerSpeed = targetVelocity.magnitude;
        if (enableCameraBobbing && playerSpeed > 0)
        {
            float waveslice = Mathf.Sin(bobTimer);
            float currentBobbingSpeed = bobbingSpeed * playerSpeed;
            bobTimer = bobTimer + currentBobbingSpeed;

            if (bobTimer > Mathf.PI * 2)
            {
                bobTimer = bobTimer - (Mathf.PI * 2);
            }

            Vector3 localPos = camTransform.localPosition;
            localPos.y = Mathf.Lerp(localPos.y, Mathf.Abs(waveslice) * bobbingAmount, Time.deltaTime * viewSmoothing);
            camTransform.localPosition = localPos;
        }
        else
        {
            bobTimer = 0;
            Vector3 localPos = camTransform.localPosition;
            localPos.y = Mathf.Lerp(localPos.y, 0, Time.deltaTime * viewSmoothing);
            camTransform.localPosition = localPos;
        }

        if (isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = false;
        }

        if (move.magnitude == 0)
        {
            targetVelocity = Vector3.zero;
            return;
        }

        float horizontal = move.x;
        float vertical = move.y;
        float sidewaysRatio = Mathf.Abs(horizontal) / move.magnitude; // Get how non-straight movement is

        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;
        Vector3 moveDirection = transform.TransformDirection(inputDirection);

        // Start with base speed
        float currentSpeed = speed;

        // START OF SPEED MODIFIERS //
        if (vertical < 0)
            currentSpeed *= reverseSpeedMultiplier;
        else if (isSprinting)
            currentSpeed *= sprintSpeedMultiplier;

        if (sidewaysRatio > 0)
        {
            // Apply a slow to player movement depending on how sideways they are moving
            float currSidewaysSpeedMultiplier = Mathf.Lerp(1f, sidewaysSpeedMultiplier, sidewaysRatio);
            currentSpeed *= currSidewaysSpeedMultiplier;
        }

        if (isCrouching) currentSpeed *= crouchSpeedMultiplier;
        if (isAiming) currentSpeed *= aimSpeedMultiplier;
        // END OF SPEED MODIFIERS //

        targetVelocity = moveDirection * currentSpeed;

        // Less control in the air
        if (!IsGrounded())
        {
            targetVelocity = (targetVelocity * airSpeedMultiplier) + (rb.velocity * (1 - airSpeedMultiplier));
            targetVelocity.y = rb.velocity.y;
        }

        // Move
        float lerpFactor = Time.deltaTime * moveSmoothing;
        rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, targetVelocity.x, lerpFactor), rb.velocity.y, Mathf.Lerp(rb.velocity.z, targetVelocity.z, lerpFactor));
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
    private float reverseSpeedMultiplier;
    private float sidewaysSpeedMultiplier;
    private float sprintSpeedMultiplier;
    private float crouchSpeedMultiplier;
    private float airSpeedMultiplier;
    private float aimSpeedMultiplier;
    private float moveSmoothing;

    private float standingHeight;
    private float crouchHeight;

    private float jumpForce;
    private float doubleJumpForce;
    private float groundCheckRadius;
    private LayerMask groundMask;

    private float viewSmoothing;
    private bool enableCameraBobbing;
    private float bobbingSpeed;
    private float bobbingAmount;
    private float mouseSensitivity;

    private void UpdateSettings()
    {
        GameSettings settings = GameManager.GetSettings();

        speed = settings.gameplay.speed;
        reverseSpeedMultiplier = settings.gameplay.reverseSpeedMultiplier;
        sidewaysSpeedMultiplier = settings.gameplay.sidewaysSpeedMultiplier;
        sprintSpeedMultiplier = settings.gameplay.sprintSpeedMultiplier;
        crouchSpeedMultiplier = settings.gameplay.crouchSpeedMultiplier;
        airSpeedMultiplier = settings.gameplay.airSpeedMultiplier;
        aimSpeedMultiplier = settings.gameplay.aimSpeedMultiplier;
        moveSmoothing = settings.gameplay.moveSmoothing;

        standingHeight = settings.gameplay.standingHeight;
        crouchHeight = settings.gameplay.crouchHeight;

        jumpForce = settings.gameplay.jumpForce;
        doubleJumpForce = settings.gameplay.doubleJumpForce;
        groundCheckRadius = settings.gameplay.groundCheckRadius;
        groundMask = settings.gameplay.groundMask;

        viewSmoothing = settings.display.viewSmoothing;
        enableCameraBobbing = settings.display.enableCameraBobbing;
        bobbingSpeed = settings.display.bobbingSpeed;
        bobbingAmount = settings.display.bobbingAmount;
        mouseSensitivity = settings.input.mouseSensitivity;
    }
}
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
        Debug.Log("moving");
        move = context.ReadValue<Vector2>();
    }

    public void HandleLook(InputAction.CallbackContext context)
    {
        Debug.Log("looking");
        if (disabled)
            return;

        Vector2 mouseDelta = context.ReadValue<Vector2>() * mouseSensitivity;

        mousePosition = Vector2.Lerp(mousePosition, mousePosition + mouseDelta, 1.0f / viewSmoothing);

        mousePosition.y = Mathf.Clamp(mousePosition.y, -60.0f, 45.0f); // Don't allow 360 y-axis
    }

    public void HandleJump(InputAction.CallbackContext context)
    {
        Debug.Log("jumping");
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
            isCrouching = true;
        }
        else if (context.canceled)
        {
            camBaseHeight = standingHeight;
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

    //////////////////////////////////////////////////
    // Private Fields and Methods
    //////////////////////////////////////////////////

    // Components
    public Rigidbody rb;
    public Transform camTransform;
    public CapsuleCollider col;

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
    }

    float currMaxSpeed;
    private void Update()
    {
        if (WeaponWheelController.Instance.open)
        {
            disabled = true;
            return;
        }
        else disabled = false;

        currMaxSpeed = maxSpeed;

        if (isJumping)
        {
            isJumping = false;
        }
        float horizontal = move.x;
        float vertical = move.y;
        float sidewaysRatio = Mathf.Abs(horizontal) / (Mathf.Abs(horizontal) + Mathf.Abs(vertical)); // Get how non-straight movement is

        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;
        Vector3 moveDirection = transform.TransformDirection(inputDirection);

        // Start with base speed
        float currentSpeed = speed;

        // START OF SPEED MODIFIERS //
        if (vertical < 0)
            currMaxSpeed *= reverseSpeedMultiplier;
        else if (isSprinting)
            currMaxSpeed *= sprintSpeedMultiplier;

        if (sidewaysRatio > 0)
        {
            // Apply a slow to player movement depending on how sideways they are moving
            float currSidewaysSpeedMultiplier = Mathf.Lerp(1f, sidewaysSpeedMultiplier, sidewaysRatio);
            currMaxSpeed *= currSidewaysSpeedMultiplier;
        }

        if (isCrouching) currMaxSpeed *= crouchSpeedMultiplier;
        if (isAiming) currMaxSpeed *= aimSpeedMultiplier;
        // END OF SPEED MODIFIERS //

        if (rb.velocity.magnitude < currMaxSpeed)
        {
            if (IsGrounded())
            {
                rb.AddForce(moveDirection * speed, ForceMode.VelocityChange);
            }
            else
            {
                // Less control in the air
                rb.AddForce(moveDirection * speed * airSpeedMultiplier, ForceMode.VelocityChange);
            }
        }
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
            float currentBobbingSpeed = bobbingSpeed * speed * Time.deltaTime;
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
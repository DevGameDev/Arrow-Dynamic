using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public PlayerMovement Instance { get; set; }

    private GameSettings settings;
    private GameState state;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            GameManager.Instance.HandleGameQuit(false, "Duplicate PlayerMovement's");
        }
    }

    // Components
    private Rigidbody rb;
    private CapsuleCollider col;

    // State
    private bool canDoubleJump = false;
    private bool isSprinting = false;
    private bool isCrouching = false;
    private bool isAiming = false;

    void Start()
    {
        settings = GameManager.GetSettings();
        state = GameManager.GetState();

        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        // Check for sprint input
        isSprinting = Input.GetKey(settings.input.sprintKey);

        // Check for crouch input
        if (Input.GetKeyDown(settings.input.crouchKey))
        {
            isCrouching = !isCrouching;
            col.height = isCrouching ? settings.gameplay.crouchHeight : settings.gameplay.standingHeight;
        }

        // Grounded check and jump
        if (IsGrounded())
        {
            canDoubleJump = true;
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * settings.gameplay.jumpForce, ForceMode.Impulse);
            }
        }
        else if (canDoubleJump && Input.GetButtonDown("Jump"))
        {
            Vector3 currentVelocity = rb.velocity;
            currentVelocity.y = 0;
            rb.velocity = currentVelocity;
            rb.AddForce(Vector3.up * settings.gameplay.doubleJumpForce, ForceMode.Impulse);
            canDoubleJump = false;
        }

        // Calculate movement direction
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

        // Calculate the base speed
        float currentSpeed = settings.gameplay.speed;
        if (vertical < 0)
        {
            currentSpeed *= settings.gameplay.reverseSpeedMultiplier;
        }
        else if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            currentSpeed *= settings.gameplay.sidewaysSpeedMultiplier;
        }

        // Apply sprint multiplier
        if (isSprinting && vertical > 0)
        {
            currentSpeed *= settings.gameplay.sprintSpeedMultiplier;
        }

        // Calculate target velocity
        Vector3 moveDirection = transform.TransformDirection(inputDirection);
        Vector3 targetVelocity = moveDirection * currentSpeed;

        // Apply crouch and aim multipliers
        if (isCrouching) targetVelocity *= settings.gameplay.crouchSpeedMultiplier;
        if (isAiming) targetVelocity *= settings.gameplay.aimSpeedMultiplier;

        // Update the Rigidbody's velocity
        targetVelocity.y = rb.velocity.y;
        if (targetVelocity.magnitude == 0)
            rb.velocity = Vector3.zero;
        else
            rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.deltaTime * 20f);
    }

    bool IsGrounded()
    {
        Vector3 groundCheckPosition = transform.position;
        groundCheckPosition.y -= col.bounds.extents.y;
        return Physics.CheckSphere(groundCheckPosition, settings.gameplay.groundCheckRadius, settings.gameplay.groundMask);
    }

    public void UpdateAimSpeedMultiplier(bool active)
    {
        isAiming = active;
    }

    public float GetHorizontalSpeedRatio()
    {
        return rb.velocity.x + rb.velocity.z;
    }
}
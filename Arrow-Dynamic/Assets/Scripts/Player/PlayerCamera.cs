using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public PlayerCamera Instance { get; set; }

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
            GameManager.Instance.HandleGameQuit(false, "Duplicate PlayerCam's");
        }
    }

    private Vector2 mouse; // base mouse movement
    private Vector2 smoothMouse; // smoothened actual location
    private float timer;

    private PlayerMovement playerMovement;

    void Start()
    {
        settings = GameManager.GetSettings();
        state = GameManager.GetState();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {
        // Get the mouse input
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * settings.input.mouseSensitivity;

        mouse = Vector2.Lerp(mouse, mouseDelta, 1.0f / settings.display.viewSmoothing);
        smoothMouse += mouse;

        smoothMouse.y = Mathf.Clamp(smoothMouse.y, -60.0f, 45.0f); // Don't allow 360 y-axis

        // Apply the y rotation to the camera
        transform.localRotation = Quaternion.AngleAxis(-smoothMouse.y, Vector3.right);

        // Apply the x rotation to the player
        transform.parent.localRotation = Quaternion.AngleAxis(smoothMouse.x, Vector3.up);

        // Apply camera bobbing if enabled and player is moving
        float playerSpeed = playerMovement.GetHorizontalSpeedRatio();
        if (settings.display.enableCameraBobbing && playerSpeed > 0)
        {
            float waveslice = Mathf.Sin(timer);
            float currentBobbingSpeed = settings.display.bobbingSpeed * playerSpeed;
            timer = timer + currentBobbingSpeed;

            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }

            Vector3 localPos = transform.localPosition;
            localPos.y = Mathf.Lerp(localPos.y, Mathf.Abs(waveslice) * settings.display.bobbingAmount, Time.deltaTime * settings.display.viewSmoothing);
            transform.localPosition = localPos;
        }
        else
        {
            timer = 0;
            Vector3 localPos = transform.localPosition;
            localPos.y = Mathf.Lerp(localPos.y, 0, Time.deltaTime * settings.display.viewSmoothing);
            transform.localPosition = localPos;
        }
    }
}
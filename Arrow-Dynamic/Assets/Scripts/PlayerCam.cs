using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensitivity = 5.0f;
    public float smoothFactor = 2.0f;

    private Vector2 mouse; // base mouse movement
    private Vector2 smoothMouse; // smoothened actual location

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get the mouse input
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * sensitivity;

        mouse = Vector2.Lerp(mouse, mouseDelta, 1.0f / smoothFactor);
        smoothMouse += mouse;

        smoothMouse.y = Mathf.Clamp(smoothMouse.y, -90.0f, 90.0f); // Don't allow 360 y-axis

        // Apply the y rotation to the camera
        transform.localRotation = Quaternion.AngleAxis(-smoothMouse.y, Vector3.right);

        // Apply the x rotation to the player
        transform.parent.localRotation = Quaternion.AngleAxis(smoothMouse.x, Vector3.up);
    }
}
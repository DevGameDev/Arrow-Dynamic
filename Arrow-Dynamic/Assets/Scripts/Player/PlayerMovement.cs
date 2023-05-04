using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls basic player movement and all movement abilities. 
/// </summary>
/// <remarks>
/// All movement is with physics (using a rigidbody)
/// </remarks>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpForce = 5.0f;
    public float doubleJumpForce = 4.0f;
    public float groundCheckDistance = 0.5f;

    private Rigidbody rb;
    private bool canDoubleJump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.TransformDirection(new Vector3(horizontal, 0, vertical))
         * speed;

        rb.AddForce(moveDirection * Time.deltaTime, ForceMode.VelocityChange);

        if (IsGrounded())
        {
            canDoubleJump = true;
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            }
        }
        else if (canDoubleJump && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.VelocityChange);
            canDoubleJump = false;
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }
}
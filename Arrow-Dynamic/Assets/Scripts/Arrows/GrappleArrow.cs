using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleArrow : BasicArrow
{
    public float pullForce = 10000.0f;
    private LineRenderer grappleLine;
    private Vector3 grapplePoint;
    private bool grappleActive;

    private void Start()
    {
        grappleLine = GetComponent<LineRenderer>();
        grappleLine.enabled = false;
    }

    public override void OnRelease()
    {
        base.OnRelease();
        grappleLine.SetPosition(0, transform.position);
        grappleActive = true;
        grappleLine.enabled = true;
    }

    public override void OnHit(Collider other)
    {
        base.OnHit(other);
        grapplePoint = transform.position;
        // Give player an impulse towards the grapple point
        PlayerController playerController = PlayerController.Instance;
        Vector3 pullDirection = (grapplePoint - playerController.transform.position).normalized;
        playerController.grappleVelocity += (pullDirection * pullForce) / playerController.rb.mass;
        grappleActive = false;
        grappleLine.enabled = false;
    }

    private void Update()
    {
        if (grappleActive)
        {
            grappleLine.SetPosition(0, transform.position);
            grappleLine.SetPosition(1, PlayerController.Instance.transform.position);
        }
    }
}
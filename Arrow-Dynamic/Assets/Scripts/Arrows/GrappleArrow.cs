using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleArrow : BasicArrow
{
    public float pullForce = 10000.0f;
    public float duration = 1f;
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

        StartCoroutine(GrappleDespawnTimer());
    }

    public override void OnHit(Collider other)
    {
        base.OnHit(other);
        grapplePoint = transform.position;

        // Start the coroutine to apply the grapple force smoothly
        StartCoroutine(GrappleForce());
    }

    private IEnumerator GrappleForce()
    {
        PlayerController playerController = PlayerController.Instance;

        // Cancel the player's vertical velocity.
        playerController.rb.velocity = Vector3.zero;

        // The grapple effect duration
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Vector3 pullDirection = (grapplePoint - playerController.transform.position).normalized;
            playerController.rb.AddForce(pullDirection * pullForce * Time.fixedDeltaTime, ForceMode.VelocityChange);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

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

    private IEnumerator GrappleDespawnTimer()
    {
        float startTime = Time.time;

        while (Time.time - startTime < 10f)
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}
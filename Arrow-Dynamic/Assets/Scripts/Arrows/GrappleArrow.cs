using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleArrow : BasicArrow
{
    public float pullSpeed = 10.0f;

    public override void OnHit(Collision collision)
    {
        base.OnHit(collision);

        // Start pulling player
        StartCoroutine(PullPlayerToArrow());
    }

    private IEnumerator PullPlayerToArrow()
    {
        PlayerController playerController = PlayerController.Instance;
        while ((playerController.transform.position - transform.position).magnitude > 0.1f)
        {
            Vector3 pullDirection = (transform.position - playerController.transform.position).normalized;
            playerController.rb.velocity = pullDirection * pullSpeed;
            yield return null;
        }

        playerController.rb.velocity = Vector3.zero;
    }
}
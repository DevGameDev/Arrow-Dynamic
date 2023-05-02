using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float impactSpeedThreshold = 5.0f;
    public string arrowTipName = "ArrowTip";
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude > impactSpeedThreshold)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.thisCollider.name == arrowTipName)
                {
                    StickArrow(collision);
                    break;
                }
            }
        }
    }

    private void StickArrow(Collision collision)
    {
        rb.isKinematic = true;
        // rb.detectCollisions = false;
        gameObject.layer = LayerMask.NameToLayer("environment");
        GetComponent<Collider>().enabled = false;

        transform.SetParent(collision.transform);
    }
}
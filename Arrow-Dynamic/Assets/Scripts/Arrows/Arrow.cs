using UnityEngine;

/// <summary>
/// Handles basic arrow behavior
/// </summary>
public class Arrow : MonoBehaviour
{
    public float impactSpeedThreshold = 5.0f;
    public string arrowTipName = "ArrowTip";
    private Rigidbody rb;

    // Initialize the rigidbody component
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Detect arrow collision and stick it if the impact is strong enough
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

    // Stick the arrow to the collided object and disable its physics
    private void StickArrow(Collision collision)
    {
        rb.isKinematic = true;
        gameObject.layer = LayerMask.NameToLayer("environment");
        GetComponent<Collider>().enabled = false;

        transform.SetParent(collision.transform);
    }
}
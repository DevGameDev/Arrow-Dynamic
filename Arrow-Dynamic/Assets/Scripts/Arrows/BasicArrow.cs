using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles basic arrow behavior
/// </summary>
public class BasicArrow : MonoBehaviour, IArrow
{
    [SerializeField] private Rigidbody myRb;
    public Rigidbody rb
    {
        get { return myRb; }
        set { myRb = value; }
    }

    [SerializeField] private List<Collider> myColliders;
    public List<Collider> colliders
    {
        get { return myColliders; }
        set { myColliders = value; }
    }

    public float impactSpeedThreshold = 5.0f;
    public string arrowTipName = "ArrowTip";
    private bool hit;

    // Detect arrow collision and stick it if the impact is strong enough
    void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude > impactSpeedThreshold)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.thisCollider.name == arrowTipName)
                {
                    OnHit(collision);
                    break;
                }
            }
        }
    }

    virtual public void OnLoad()
    {

    }

    virtual public void OnRelease()
    {

    }

    virtual public void OnHit(Collision collision)
    {
        rb.isKinematic = true;

        foreach (Collider col in colliders)
            col.enabled = false;

        gameObject.layer = LayerMask.NameToLayer("environment");

        transform.SetParent(collision.transform);
    }

    virtual public void OnUnload()
    {
        Destroy(gameObject);
    }
}
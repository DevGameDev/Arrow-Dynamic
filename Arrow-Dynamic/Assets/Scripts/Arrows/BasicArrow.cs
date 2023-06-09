using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles basic arrow behavior
/// </summary>
public class BasicArrow : MonoBehaviour, IArrow
{
    public bool isEnabled
    {
        get { return myEnabled; }
        set { myEnabled = value; }
    }
    public bool myEnabled = false;

    [SerializeField] private Rigidbody myRb;
    [SerializeField] public AudioSource sfxSource;
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

    virtual public void OnLoad()
    {

    }

    virtual public void OnRelease()
    {
        myEnabled = true;
    }

    void FixedUpdate()
    {
        if (myEnabled && !rb.isKinematic && rb.velocity.magnitude > impactSpeedThreshold)
        {
            Ray ray = new Ray(transform.position, rb.velocity.normalized);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rb.velocity.magnitude * Time.fixedDeltaTime, LayerMask.GetMask("environment")))
            {
                rb.isKinematic = true;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                foreach (Collider col in colliders)
                    col.enabled = false;

                transform.position = hit.point;

                OnHit(hit.collider);
            }
        }
    }

    virtual public void OnHit(Collider other)
    {
        //old sfx code
        //AudioManager.Instance.PlaySFX(AudioManager.SoundEffect.BowHit, 1.0f);

        sfxSource.volume *= AudioManager.Instance.sfxVolume;
        
        AudioManager.Instance.PlaySFX(sfxSource, 0.5f, AudioManager.SoundEffect.BowHit);

    }
}
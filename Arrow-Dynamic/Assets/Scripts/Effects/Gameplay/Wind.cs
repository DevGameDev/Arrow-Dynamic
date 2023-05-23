using UnityEngine;

public class Wind : MonoBehaviour
{
    public Vector3 windDirection = new Vector3(1, 0, 0);
    public float windStrength = 1.0f;
    public bool endless = false;
    public float windDuration = 5f;

    private bool on = false;
    private float timeRemaining = 0f;

    public void ActivateWind(Vector3 direction, bool destroyOnEnd = true)
    {
        windDirection = direction;
        on = true;
        timeRemaining = windDuration;
    }

    public void DeactivateWind()
    {
        on = false;
        timeRemaining = 0;
    }

    void FixedUpdate()
    {
        if (on)
        {
            if (endless || timeRemaining > 0)
            {
                Rigidbody[] allRigidbodies = FindObjectsOfType<Rigidbody>();
                foreach (Rigidbody rb in allRigidbodies)
                {
                    rb.AddForce(windDirection * windStrength);
                }
            }
            else if (timeRemaining == 0)
                Destroy(this);
        }
    }
}
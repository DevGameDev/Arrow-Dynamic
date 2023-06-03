using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float radius = 5.0f;
    public float power = 10.0f;

    void Start()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            PlayerController pc = hit.GetComponent<PlayerController>();

            if (pc != null)
            {
                // Calculate the direction and scale by the explosion power and the distance.
                Vector3 explosionDir = (hit.transform.position - explosionPos).normalized;
                float distance = Vector3.Distance(hit.transform.position, explosionPos);
                Vector3 explosionForce = explosionDir * (power / Mathf.Max(1f, distance));

                pc.explosionVelocity += explosionForce / pc.rb.mass;
            }
            else // Just push everything else
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0f);
            }

        }
    }
}
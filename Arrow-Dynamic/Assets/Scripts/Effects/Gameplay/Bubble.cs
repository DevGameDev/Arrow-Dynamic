using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float power = 10.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 directionToPlayer = other.transform.position - transform.position;
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            playerRb.AddForce(power * directionToPlayer, ForceMode.VelocityChange);
        }

        Destroy(gameObject);
    }
}
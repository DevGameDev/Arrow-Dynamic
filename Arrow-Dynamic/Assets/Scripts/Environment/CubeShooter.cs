using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeShooter : MonoBehaviour
{
    [SerializeField] float attack_pause;
    public GameObject projectile;
    private float timer;

    
    void Update()
    {
        timer += Time.deltaTime;

        // Check if the desired interval has passed
        if (timer >= attack_pause)
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            timer = 0f;
        }

        
    }
}

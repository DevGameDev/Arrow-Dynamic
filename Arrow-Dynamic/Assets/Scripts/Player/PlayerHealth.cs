using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 10f;
    public float currentHealth;
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "damege")
        {
            currentHealth -= 3;

            if(currentHealth <= 0f)
            {
                
            }

        }
    }
    void awake()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <=  0f)
        {
            PlayerController.Instance.RespawnPoint();
        }
        if ((currentHealth < maxHealth) && (currentHealth > 0))
        {
            Debug.Log(currentHealth);
            currentHealth += Time.deltaTime;     
                   
        }
        
    }   
}

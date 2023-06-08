using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 10f;
    public float currentHealth;
    public static PlayerHealth Instance { get; set; }

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
    public void doDamege(float damege)
    {
        currentHealth -= damege;
        Debug.Log(currentHealth);
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentHealth = maxHealth;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("here!");
        if(currentHealth <=  0f)
        {
            Debug.Log("Dead");
            PlayerController.Instance.RespawnPoint();
        }
        if ((currentHealth < maxHealth) && (currentHealth > 0))
        {
            currentHealth += Time.deltaTime;                
        }
        
    }   
    
}

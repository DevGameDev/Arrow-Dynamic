using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    public AudioSource onHit;

    void Update()
    {
        
    }

    void Start()
    {
        onHit = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Arrow"){
            gameObject.SetActive(false);
            onHit.Play();
        }
            
        
        
      
    }
    
}

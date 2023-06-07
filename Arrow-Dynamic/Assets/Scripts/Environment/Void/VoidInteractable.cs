using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidInteractable : MonoBehaviour
{
    public Vector3 rotationSpeed; // The speed of rotation around each axis
    public static int Door1Trig = 0;
    void Update()
    {
        // Rotate the object continuously based on the rotation speed
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Arrow"){
            gameObject.SetActive(false);
            
        }
            
        
        
      
    }
}

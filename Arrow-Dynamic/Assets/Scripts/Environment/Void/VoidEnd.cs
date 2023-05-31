using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidEnd : MonoBehaviour
{
    Animation animator; 
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        if (VoidInteractableLeft.ThroneTrig == 3){
            animator.Play("EndFloor");
        }
    }
}

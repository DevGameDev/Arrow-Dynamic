using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidDoor1 : MonoBehaviour
{
    Animation animator; 
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (VoidInteractable.Door1Trig > 2)
        {
            animator.Play("VoidDoor1");
            VoidInteractable.Door1Trig = 0;
        }
    }
}

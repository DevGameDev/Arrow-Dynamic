using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    public bool isColliding = false;
    public AudioSource source1;
    public bool isOpen = false;

    void Update()
    {
        //isColliding = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (isColliding) return;
        isColliding = true;
        //need to give arrows tag 
        //if (col.gameObject.tag == "Arrow") {
        source1.Play();
        if (isOpen == false) {
            //door1.transform.position += new Vector3(0,5,0);
            //door2.transform.position += new Vector3(0,5,0);
            isOpen = true;
        }
    }
}

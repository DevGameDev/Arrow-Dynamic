using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    public bool isColliding = false;
    public AudioSource source1;

    void Update()
    {
        isColliding = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (isColliding) return;
        isColliding = true;
        //need to give arrows tag 
        //if (col.gameObject.tag == "Arrow") {
        source1.Play();
    }
}

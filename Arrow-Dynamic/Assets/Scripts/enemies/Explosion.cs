using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    public GameObject bomb;
    public float delay = 3f;

    float countdown;

    void awake()
    {
        countdown = delay;

    }

    void Update(){
        countdown -= Time.deltaTime;
        if (countdown <= 0f)
        {
            Explode();
        }
    }
    void Explode()
    {
        
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeFront : MonoBehaviour
{
    public GameObject skull1;    
    public GameObject skull2;
    public GameObject skull3;

    

    private Animation anim;
    private bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        skull1 = GameObject.Find("2Skull1");
        skull2 = GameObject.Find("2Skull2");
        skull3= GameObject.Find("2Skull3");
        anim = gameObject.GetComponent<Animation>();
        
        
    }

    

    // Update is called once per frame
    void Update()
    {
        
        if (!(skull1.activeSelf) && !(skull2.activeSelf) && !(skull3.activeSelf) && isOpen == false)
        {
            anim.Play("BridgeFront");
            isOpen = true;
        }
    }
}


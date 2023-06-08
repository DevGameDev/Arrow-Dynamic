using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidCrystal : MonoBehaviour
{
    

    public GameObject Cone2;    
    
    
    // Start is called before the first frame update
    void Start()
    {
        Cone2 = GameObject.Find("2Cone2");
        
        
        
    }

    

    // Update is called once per frame
    void Update()
    {
        
        if (!Cone2.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}

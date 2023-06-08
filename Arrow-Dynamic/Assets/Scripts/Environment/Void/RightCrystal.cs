using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightCrystal : MonoBehaviour
{
   public GameObject Cone3;    
    
    
    // Start is called before the first frame update
    void Start()
    {
        Cone3 = GameObject.Find("2Cone3");
        
    
    }

    

    // Update is called once per frame
    void Update()
    {
        
        if (!Cone3.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}

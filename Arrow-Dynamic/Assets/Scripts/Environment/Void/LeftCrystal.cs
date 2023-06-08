using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftCrystal : MonoBehaviour
{
    

    public GameObject Cone1;    
    
    
    // Start is called before the first frame update
    void Start()
    {
        Cone1 = GameObject.Find("2Cone1");
        
        
        
    }

    

    // Update is called once per frame
    void Update()
    {

        if (!Cone1.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}

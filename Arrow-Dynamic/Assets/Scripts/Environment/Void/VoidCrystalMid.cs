using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidCrystalMid : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (VoidInteractableMid.gone == true){
            gameObject.SetActive(false);
        }
    }
}

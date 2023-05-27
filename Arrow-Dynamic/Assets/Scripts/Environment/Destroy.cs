using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject fractured;

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.name == "Sphere")
            BreakTheThing();
    }
    
    public void BreakTheThing()
    {
        Instantiate(fractured, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

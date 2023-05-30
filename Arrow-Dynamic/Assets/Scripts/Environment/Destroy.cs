using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject fractured;
    private Vector3 position;
    private Quaternion rotation;
    private GameObject sphere;

    private void awake(){
        position = transform.position;
        rotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.name == "BasicArrow(Clone)"){
             Debug.Log("Hit!");
            sphere = GameObject.Find("Sphere");
            Destroy(sphere);
            BreakTheThing();
        }
    }
    
    public void BreakTheThing()
    {
        Debug.Log(transform.position);

        Instantiate(fractured, transform.position, Quaternion.identity);
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

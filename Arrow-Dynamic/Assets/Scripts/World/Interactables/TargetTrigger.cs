using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    public Material[] materials;
    public int count = 0;
    public Material red;
    public bool isColliding = false;
    public AudioSource source1;
    public AudioClip source2;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        isColliding = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if(isColliding) return;
        isColliding = true;
        //need to give arrows tag 
        //if (col.gameObject.tag == "Arrow") {
        if (count == 2) { 
            AudioSource.PlayClipAtPoint(source2, this.gameObject.transform.position);
            Destroy(gameObject); 
        }
        else {
            source1.Play();
            transform.GetComponent<Renderer>().material = materials[count++];
        }
    }
}

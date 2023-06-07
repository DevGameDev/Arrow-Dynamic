using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidDoor : MonoBehaviour
{
    public GameObject Cone1;    
    public GameObject Cone2;
    public GameObject Cone3;
    private Animation anim;
    private bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        Cone1 = GameObject.Find("1Cone1");
        Cone2 = GameObject.Find("1Cone2");
        Cone3 = GameObject.Find("1Cone3");
        anim = gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!(Cone1.activeSelf) && !(Cone2.activeSelf) && !(Cone3.activeSelf) && isOpen == false)
        {
            anim.Play("Door");
            isOpen = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] GameObject door;

    bool isOpen = false;

    void OnTriggerEnter(Collider col) {
        if (isOpen == false) {
            door.transform.position += new Vector3(0,5,0);
            isOpen = true;
        }
    }
}

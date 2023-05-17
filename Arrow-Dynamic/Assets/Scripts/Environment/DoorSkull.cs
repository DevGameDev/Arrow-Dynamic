using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSkull : MonoBehaviour
{
    public LayerMask arrowLayer;
    private void OnCollisionEnter(Collision other)
    {
        if ((arrowLayer.value & 1 << other.gameObject.layer) != 0)
        {
            Debug.Log("hit");
            SkullDoor.skullsLeft -= 1;
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("dmaege done");
            PlayerHealth.Instance.doDamege(3);
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    
    public float timeUntilDestroy;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject, timeUntilDestroy);
    }
}

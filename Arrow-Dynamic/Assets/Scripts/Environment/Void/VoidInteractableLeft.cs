using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidInteractableLeft : MonoBehaviour
{
    public Vector3 rotationSpeed; // The speed of rotation around each axis
    public static int ThroneTrig = 0;
    public static bool gone = false;
    void Update()
    {
        // Rotate the object continuously based on the rotation speed
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider arrow)
    {
        // Disable the object or remove it from the scene
        gameObject.SetActive(false);
        gone = true;
        VoidInteractableLeft.ThroneTrig++;
    }

}

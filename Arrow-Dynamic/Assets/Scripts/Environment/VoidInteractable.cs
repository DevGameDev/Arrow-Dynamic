using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidInteractable : MonoBehaviour
{
    public Vector3 rotationSpeed; // The speed of rotation around each axis

    void Update()
    {
        // Rotate the object continuously based on the rotation speed
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Disable the object or remove it from the scene
        gameObject.SetActive(false); // or Destroy(gameObject) to completely remove it
    }
}

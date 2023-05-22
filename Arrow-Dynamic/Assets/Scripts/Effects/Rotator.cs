using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotateSpeed = 20f;

    void Update()
    {
        transform.Rotate(new Vector3(0, rotateSpeed, 0) * Time.deltaTime, Space.World);
    }
}
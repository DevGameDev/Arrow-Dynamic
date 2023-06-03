using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : MonoBehaviour
{
    public float shrinkTime = 15.0f;
    public Vector3 initialSize = Vector3.one;
    public Vector3 finalSize = Vector3.one * 0.5f;

    public IEnumerator Shrink()
    {
        float startTime = Time.time;
        float initialY = transform.position.y + initialSize.y / 2; // Top Y position of the block

        while (Time.time - startTime < shrinkTime)
        {
            float ratio = (Time.time - startTime) / shrinkTime;
            Vector3 newSize = Vector3.Lerp(initialSize, finalSize, ratio);
            transform.localScale = newSize;

            float newY = initialY - newSize.y / 2; // Calculate new Y position so the top stays at the same location
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            yield return null;
        }

        Destroy(gameObject);
    }
}
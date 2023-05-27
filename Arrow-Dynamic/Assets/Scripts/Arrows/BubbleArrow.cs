using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleArrow : BasicArrow
{
    public GameObject bubblePrefab;
    public float despawnTime = 15.0f;
    private GameObject bubble;

    public override void OnHit(Collision collision)
    {
        base.OnHit(collision);
        Vector3 directionFromHit = collision.transform.position - transform.position;

        // Spawn bubble
        bubble = Instantiate(bubblePrefab, transform.position, Quaternion.LookRotation(directionFromHit));
        StartCoroutine(ControlBubble(bubble));
    }

    private IEnumerator ControlBubble(GameObject iceBlock)
    {
        float startTime = Time.time;

        while (Time.time - startTime < despawnTime)
        {
            yield return null;
        }

        if (bubble) Destroy(bubble);
    }

    private void OnDestroy()
    {
        if (bubble) Destroy(bubble);
    }
}
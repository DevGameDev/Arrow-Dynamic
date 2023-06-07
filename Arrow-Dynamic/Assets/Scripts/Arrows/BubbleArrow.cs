using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleArrow : BasicArrow
{
    public GameObject bubblePrefab;
    private GameObject bubbleObj;

    public override void OnHit(Collider other)
    {
        base.OnHit(other);
        Vector3 directionFromHit = other.transform.position - transform.position;

        // Spawn bubble
        bubbleObj = Instantiate(bubblePrefab, transform.position, Quaternion.LookRotation(directionFromHit));
        AudioManager.Instance.PlaySFX(AudioManager.SoundEffect.ArrowHit, 0.7f);
        Bubble bubble = bubbleObj.GetComponent<Bubble>();

        bubble.StartCoroutine(bubble.StartDespawnTimer());

        Destroy(gameObject);
    }
}
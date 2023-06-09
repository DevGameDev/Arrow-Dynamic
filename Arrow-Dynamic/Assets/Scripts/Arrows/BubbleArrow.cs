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
            
        AudioManager.Instance.PlaySFX(this.sfxSource, 1.0f, AudioManager.SoundEffect.BounceHit);

        // Spawn bubble
        bubbleObj = Instantiate(bubblePrefab, transform.position, Quaternion.LookRotation(directionFromHit));
        Bubble bubble = bubbleObj.GetComponent<Bubble>();

        bubble.StartCoroutine(bubble.StartDespawnTimer());
        Debug.Log("bubble arrow");
        
        Destroy(gameObject, 1.0f);
    }
}
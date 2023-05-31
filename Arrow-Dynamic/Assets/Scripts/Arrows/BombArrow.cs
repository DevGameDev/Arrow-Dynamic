using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombArrow : BasicArrow
{
    public Explosion explosionPrefab;

    public override void OnHit(Collision collision)
    {
        base.OnHit(collision);
        
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

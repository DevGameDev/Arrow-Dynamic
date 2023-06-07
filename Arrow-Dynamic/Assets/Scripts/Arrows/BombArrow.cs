using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombArrow : BasicArrow
{
    public Explosion explosionPrefab;

    public override void OnHit(Collider other)
    {
        base.OnHit(other);

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySFX(AudioManager.SoundEffect.ArrowHit, 0.7f);
        Destroy(gameObject);
    }
}

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
        AudioManager.Instance.PlaySFX(AudioManager.SoundEffect.Explosion, 0.5f);
        Destroy(gameObject);
    }
}

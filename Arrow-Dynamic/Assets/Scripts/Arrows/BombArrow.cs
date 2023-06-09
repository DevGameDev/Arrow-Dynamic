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
        
        //old audio code
        //AudioManager.Instance.PlaySFX(AudioManager.SoundEffect.Explosion, 0.5f);
        AudioManager.Instance.PlaySFX(this.sfxSource, 1.0f, AudioManager.SoundEffect.Explosion);
        Debug.Log("bomb arrow");
        
        Destroy(gameObject, 1.0f);
    }
}

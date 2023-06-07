using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceArrow : BasicArrow
{
    public GameObject iceBlockPrefab;
    private GameObject iceBlockObj;

    public override void OnHit(Collider other)
    {
        base.OnHit(other);
        AudioManager.Instance.PlaySFX(AudioManager.SoundEffect.ArrowHit, 0.7f);

        // Spawn ice block
        if (!other.gameObject.GetComponent<IceBlock>()) // can't stack
        {
            iceBlockObj = Instantiate(iceBlockPrefab, transform.position, Quaternion.identity);
            IceBlock iceBlock = iceBlockObj.GetComponent<IceBlock>();

            iceBlock.StartCoroutine(iceBlock.Shrink());
        }

        Destroy(gameObject);
    }
}
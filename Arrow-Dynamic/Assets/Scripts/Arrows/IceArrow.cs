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

        // Spawn ice block
        if (!other.gameObject.GetComponent<IceBlock>()) // can't stack
        {
            iceBlockObj = Instantiate(iceBlockPrefab, transform.position, Quaternion.identity);
            IceBlock iceBlock = iceBlockObj.GetComponent<IceBlock>();

            iceBlock.StartCoroutine(iceBlock.Shrink());
            //AudioManager.Instance.PlaySFX(this.sfxSource, 1.0f, AudioManager.SoundEffect.Death);
            Debug.Log("ice arrow");
        }

        Destroy(gameObject);
    }
}
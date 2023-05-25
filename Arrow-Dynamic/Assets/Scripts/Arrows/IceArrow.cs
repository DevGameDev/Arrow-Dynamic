using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceArrow : BasicArrow
{
    public GameObject iceBlockPrefab;
    public float shrinkTime = 15.0f;
    public Vector3 initialSize = Vector3.one;
    public Vector3 finalSize = Vector3.one * 0.5f;
    private GameObject iceBlock;

    public override void OnHit(Collision collision)
    {
        base.OnHit(collision);
        Debug.Log("Hit!");

        // Spawn ice block
        iceBlock = Instantiate(iceBlockPrefab, transform.position, Quaternion.identity);
        StartCoroutine(ShrinkIceBlock(iceBlock));
    }

    private IEnumerator ShrinkIceBlock(GameObject iceBlock)
    {
        float startTime = Time.time;

        while (Time.time - startTime < shrinkTime)
        {
            float ratio = (Time.time - startTime) / shrinkTime;
            iceBlock.transform.localScale = Vector3.Lerp(initialSize, finalSize, ratio);
            yield return null;
        }

        Destroy(iceBlock);
    }

    private void OnDestroy()
    {
        if (iceBlock) Destroy(iceBlock);
    }
}
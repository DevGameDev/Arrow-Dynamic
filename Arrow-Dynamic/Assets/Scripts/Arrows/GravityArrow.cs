using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityArrow : BasicArrow
{
    public float effectDuration = 5.0f;
    public float effectGravity = -3f;

    public override void OnHit(Collision collision)
    {
        base.OnHit(collision);

        StartCoroutine(ChangeGravity());
    }

    IEnumerator ChangeGravity()
    {
        float originalGravity = Physics.gravity.y;
        Physics.gravity = new Vector3(0, effectGravity, 0);

        yield return new WaitForSeconds(effectDuration);

        Physics.gravity = new Vector3(0, originalGravity, 0);
    }
}

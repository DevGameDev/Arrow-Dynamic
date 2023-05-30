using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeArrow : BasicArrow
{
    public float effectDuration = 5.0f;

    public override void OnHit(Collision collision)
    {
        base.OnHit(collision);

        if (!PlayerController.Instance.timeArrowActive)
        {
            PlayerController.Instance.timeArrowActive = true;
            StartCoroutine(ApplyTimeEffect());
        }
    }

    IEnumerator ApplyTimeEffect()
    {
        yield return new WaitForSeconds(effectDuration);

        PlayerController.Instance.timeArrowActive = false;
    }
}

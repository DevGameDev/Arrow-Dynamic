using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeArrow : BasicArrow
{
    public float effectDuration = 5.0f;

    public override void OnHit(Collider other)
    {
        base.OnHit(other);

        if (!PlayerController.Instance.timeArrowActive)
        {
            PlayerController.Instance.timeArrowActive = true;
            UIManager.Instance.ControlTimeEffectIcon(true);

            StartCoroutine(ApplyTimeEffect());
        }
    }

    IEnumerator ApplyTimeEffect()
    {
        yield return new WaitForSeconds(effectDuration);

        PlayerController.Instance.timeArrowActive = false;
        UIManager.Instance.ControlTimeEffectIcon(false);
    }
}

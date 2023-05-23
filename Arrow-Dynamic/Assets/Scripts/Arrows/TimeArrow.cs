using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeArrow : BasicArrow
{
    public float effectDuration = 5.0f;
    public float effectTimeScale = 0.25f;

    public override void OnHit(Collision collision)
    {
        base.OnHit(collision);

        StartCoroutine(ApplyTimeEffect());
    }

    IEnumerator ApplyTimeEffect()
    {
        Time.timeScale = effectTimeScale;

        yield return new WaitForSeconds(effectDuration);

        Time.timeScale = GameManager.GetSettings().gameplay.baseTimeScale;
    }
}

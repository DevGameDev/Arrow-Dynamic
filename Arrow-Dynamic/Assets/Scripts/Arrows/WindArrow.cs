using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArrow : BasicArrow
{
    public GameObject windPrefab;
    public static int activeWindEffects = 0;

    public override void OnRelease()
    {
        base.OnRelease();

        if (activeWindEffects < 3)
        {
            UIManager.Instance.ControlWindEffectIcon(true, activeWindEffects);

            ApplyWindEffect();
            //AudioManager.Instance.PlaySFX(this.sfxSource, 1.0f, AudioManager.SoundEffect.Death);
            Debug.Log("wind arrow");
        }
        else Destroy(gameObject);
    }

    private void ApplyWindEffect()
    {
        activeWindEffects += 1;

        GameObject windObj = Instantiate(windPrefab, transform.position, Quaternion.identity);

        windObj.GetComponent<Wind>().ActivateWind(-transform.up);
        Destroy(gameObject);
    }
}

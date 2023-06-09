using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityArrow : BasicArrow
{
    public float effectDuration = 5.0f;
    public float effectGravity = -3f;

    public override void OnHit(Collider other)
    {
        base.OnHit(other);

        if (!PlayerController.Instance.gravityArrowActive)
        {
            PlayerController.Instance.gravityArrowActive = true;
            UIManager.Instance.ControlGravityEffectIcon(true);
            StartCoroutine(ChangeGravity());
            AudioManager.Instance.PlaySFX(this.sfxSource, 1.0f, AudioManager.SoundEffect.Death);
            Debug.Log("grav arrow");
        }
    }

    IEnumerator ChangeGravity()
    {
        float originalGravity = Physics.gravity.y;
        // float originalPlayerGravity = PlayerController.Instance.playerGravity;
        Physics.gravity = new Vector3(0, effectGravity, 0);

        yield return new WaitForSeconds(effectDuration);

        Physics.gravity = new Vector3(0, originalGravity, 0);
        // PlayerController.Instance.playerGravity = originalPlayerGravity;

        PlayerController.Instance.gravityArrowActive = false;
        UIManager.Instance.ControlGravityEffectIcon(false);
    }
}


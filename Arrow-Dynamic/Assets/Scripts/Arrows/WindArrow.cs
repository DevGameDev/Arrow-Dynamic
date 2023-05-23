using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArrow : BasicArrow
{
    Wind windPrefab;

    public override void OnHit(Collision collision)
    {
        base.OnHit(collision);

        StartCoroutine(ApplyWindEffect());
    }

    IEnumerator ApplyWindEffect()
    {
        Instantiate(windPrefab, transform.position, Quaternion.identity);

        windPrefab.ActivateWind(transform.forward);

        Destroy(gameObject);

        yield return null;
    }
}

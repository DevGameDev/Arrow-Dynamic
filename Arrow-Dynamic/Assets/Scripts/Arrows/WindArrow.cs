using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArrow : BasicArrow
{
    public GameObject windPrefab;

    public override void OnHit(Collider other)
    {
        base.OnHit(other);

        StartCoroutine(ApplyWindEffect());
    }

    IEnumerator ApplyWindEffect()
    {
        Instantiate(windPrefab, transform.position, Quaternion.identity);

        windPrefab.GetComponent<Wind>().ActivateWind(transform.forward);

        Destroy(gameObject);

        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    public bool isColliding = false;
    public AudioSource source1;
    public bool isOpen = false;

    void Update()
    {
        //isColliding = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (isColliding) return;
        isColliding = true;
        //need to give arrows tag 
        //if (col.gameObject.tag == "Arrow") {
        
        //StartCoroutine(Fade(false, source1, 1f, 0f));
        source1.Play();
        
        if (isOpen == false) {
            //door1.transform.position += new Vector3(0,5,0);
            //door2.transform.position += new Vector3(0,5,0);
            isOpen = true;
        }
    }
    private IEnumerator Fade(bool fadeIn, AudioSource source, float duration, float targetVolume)
    {
        //fade audio method from SwishSwoosh https://www.youtube.com/watch?v=kYGXGDjL5jM
        float time = 0f;
        float startVol = source.volume;

        if (!fadeIn)
        {
            double lengthOfSource = (double)source.clip.samples / source.clip.frequency;
            yield return new WaitForSecondsRealtime((float)(lengthOfSource - duration));
        }

        while(time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVol, targetVolume, time / duration);
            yield return null;
        }

        yield break;
    }
}

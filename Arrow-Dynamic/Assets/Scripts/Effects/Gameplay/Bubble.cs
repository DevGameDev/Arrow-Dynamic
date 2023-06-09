using System.Collections;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float power = 10.0f;
    public float despawnTime = 10.0f;
    public AudioSource sfxSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 directionToPlayer = other.transform.position - transform.position;
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            playerRb.AddForce(power * directionToPlayer, ForceMode.VelocityChange);
            AudioManager.Instance.PlaySFX(this.sfxSource, 1.0f, AudioManager.SoundEffect.BounceHit);
        }

        Destroy(gameObject, 1.0f);
    }

    public IEnumerator StartDespawnTimer()
    {
        float startTime = Time.time;

        while (Time.time - startTime < despawnTime)
        {
            yield return null;
        }

        Destroy(gameObject);
    }

}
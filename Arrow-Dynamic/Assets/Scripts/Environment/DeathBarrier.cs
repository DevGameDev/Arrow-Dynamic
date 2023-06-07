using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.SoundEffect.Death, 0.7f);
            other.GetComponent<PlayerController>().RespawnPoint();
        }
    }
}

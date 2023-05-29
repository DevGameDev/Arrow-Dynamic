using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class RespawnPoint : MonoBehaviour
{
    public SpawnPoints pointType = SpawnPoints.Disabled;
    public EventTypes onFirstTriggerEvent = EventTypes.Nothing;
    public bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            if (!(pointType is SpawnPoints.Disabled))
                PlayerController.Instance.SetSpawnPoint(pointType);

            if (!triggered)
            {
                GameManager.Instance.HandleGameEvent(onFirstTriggerEvent);
                triggered = true;
            }
        }
    }
}
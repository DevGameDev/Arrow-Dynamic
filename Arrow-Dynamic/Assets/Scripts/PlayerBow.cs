using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBow : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public Transform cameraTransform;
    public float arrowSpeed = 50.0f;
    public float arrowReadyTime = 0.25f;
    public float maxPullTime = 2.0f;

    private float currentPullTime = 0.0f;
    private bool isArrowReady = false;
    private bool isBowPulled = false;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!isBowPulled)
            {
                isBowPulled = true;
                currentPullTime = 0.0f;
            }
            else if (currentPullTime > arrowReadyTime) isArrowReady = true;

            // Increase the bow pull distance up to the maximum
            currentPullTime += Time.deltaTime;
            currentPullTime = Mathf.Clamp(currentPullTime, 0, maxPullTime);

            UpdateBowPullAnimation(currentPullTime);
        }
        // Check if the player releases the left mouse button
        else if (isArrowReady)
        {
            isArrowReady = false;
            isBowPulled = false;

            // Spawn an arrow and shoot it
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, cameraTransform.rotation);
            Rigidbody arrowRigidbody = arrow.GetComponent<Rigidbody>();

            Vector3 shootDirection = (cameraTransform.forward + transform.forward).normalized;

            arrowRigidbody.AddForce(shootDirection * currentPullTime * arrowSpeed, ForceMode.Impulse);
        }
        // Check if the player cancels the shot by pressing the right mouse button
        else if (Input.GetMouseButton(1))
        {
            isBowPulled = false;
            currentPullTime = 0.0f;
            UpdateBowPullAnimation(currentPullTime);
        }
    }

    void UpdateBowPullAnimation(float pullDistance)
    {
    }
}
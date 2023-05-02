using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBow : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public Transform cameraTransform;
    public Transform handTransform;
    private Transform arrowTransform;
    public Transform bowTransform;
    public float arrowSpeed = 50.0f;
    public float arrowReadyTime = 0.25f;
    public float maxPullTime = 2.0f;

    public Vector3 handRestPosition;
    public Vector3 handMinPullPosition;
    public Vector3 handMaxPullPosition;
    public Vector3 arrowRestPosition;
    public Vector3 arrowPullPosition;
    public Vector3 arrowRestRotation;
    public Vector3 arrowPullRotation;
    public Vector3 bowRestPosition;
    public Vector3 bowPullPosition;
    public Vector3 bowRestRotation;
    public Vector3 bowPulledRotation;

    private float currentPullTime = 0.0f;
    private GameObject currentArrow = null;
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
                currentArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, cameraTransform.rotation);
                arrowTransform = currentArrow.transform;
                arrowTransform.parent = cameraTransform;
            }
            else if (currentPullTime > arrowReadyTime) isArrowReady = true;

            // Increase the bow pull distance up to the maximum
            currentPullTime += Time.deltaTime;
            currentPullTime = Mathf.Clamp(currentPullTime, 0, maxPullTime);

        }
        // Check if the player releases the left mouse button
        else if (isArrowReady)
        {
            isArrowReady = false;
            isBowPulled = false;

            // Shoot the arrow
            Rigidbody arrowRigidbody = currentArrow.GetComponent<Rigidbody>();
            arrowRigidbody.isKinematic = false;
            arrowRigidbody.useGravity = true;
            arrowTransform.parent = null;
            arrowTransform = null;
            Vector3 shootDirection = cameraTransform.forward;
            arrowRigidbody.AddForce(shootDirection * currentPullTime * arrowSpeed, ForceMode.Impulse);

            // Reset pull time
            currentPullTime = 0.0f;
        }
        // Check if the player cancels the shot by pressing the right mouse button
        else if (Input.GetMouseButton(1))
        {
            isBowPulled = false;
            currentPullTime = 0.0f;

            if (currentArrow != null)
            {
                // Destroy the arrow and remove the reference
                Destroy(currentArrow);
                currentArrow = null;
            }
        }
        // float test = 2.0f * Mathf.Abs(Mathf.Sin(Time.time));

        UpdateBowPullAnimation(currentPullTime);
    }

    void UpdateBowPullAnimation(float pullTime)
    {
        if (currentArrow == null) return;

        float pullRatio = pullTime / maxPullTime;

        if (pullRatio < 0.5f)
            handTransform.localPosition = Vector3.Lerp(handRestPosition, handMinPullPosition, pullRatio * 2);
        else
            handTransform.localPosition = Vector3.Lerp(handMinPullPosition, handMaxPullPosition, (pullRatio - 0.5f) * 2);

        if (arrowTransform)
        {
            arrowTransform.localPosition = Vector3.Lerp(arrowRestPosition, arrowPullPosition, pullRatio);
            arrowTransform.localRotation = Quaternion.Lerp(Quaternion.Euler(arrowRestRotation), Quaternion.Euler(arrowPullRotation), pullRatio);
        }
        bowTransform.localPosition = Vector3.Lerp(bowRestPosition, bowPullPosition, pullRatio);
        bowTransform.localRotation = Quaternion.Lerp(Quaternion.Euler(bowRestRotation), Quaternion.Euler(bowPulledRotation), pullRatio);
    }
}
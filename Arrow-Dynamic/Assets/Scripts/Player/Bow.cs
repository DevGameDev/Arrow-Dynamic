using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [Header("References")]
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public Transform cameraTransform;
    public Transform handTransform;
    public Transform bowTransform;

    [Header("Settings")]
    public float arrowSpeed = 50.0f;
    public float arrowReadyTime = 0.25f;
    public float maxPullTime = 2.0f;
    public float pitchAdjustmentFactor = 0.2f;

    [Header("Animation Positions")]
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
    private Transform arrowTransform;
    private bool arrowReady = false;
    private bool isBowPulled = false;

    void Update()
    {
        HandleBowPull();
    }

    private void HandleBowPull()
    {
        if (Input.GetMouseButton(1))
        {
            CancelBowPull();
        }
        else if (Input.GetMouseButton(0))
        {
            PullBow();
        }
        else if (arrowReady)
        {
            ReleaseBow();
        }
        else
            currentPullTime -= Time.deltaTime;

        UpdateBowPullAnimation(currentPullTime);
    }

    private void PullBow()
    {
        if (!isBowPulled)
        {
            InitializeBowPull();
        }
        else if (currentPullTime > arrowReadyTime)
        {
            arrowReady = true;
        }

        // Increase the bow pull distance up to the maximum
        currentPullTime += Time.deltaTime;
        currentPullTime = Mathf.Clamp(currentPullTime, 0, maxPullTime);
    }

    private void InitializeBowPull()
    {
        isBowPulled = true;
        Quaternion shootRotation = Quaternion.LookRotation(CalculateAdjustedShootDirection());
        currentArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, shootRotation);
        arrowTransform = currentArrow.transform;
        arrowTransform.parent = cameraTransform;
    }

    private void ReleaseBow()
    {
        arrowReady = false;
        isBowPulled = false;

        // Shoot the arrow
        Rigidbody arrowRigidbody = currentArrow.GetComponent<Rigidbody>();
        arrowRigidbody.isKinematic = false;
        arrowRigidbody.useGravity = true;
        Vector3 shootDirection = CalculateAdjustedShootDirection();
        arrowTransform.parent = null;
        // arrowTransform.rotation = Quaternion.LookRotation(shootDirection);
        arrowRigidbody.AddForce(shootDirection * currentPullTime * arrowSpeed, ForceMode.Impulse);

        arrowTransform = null;
    }

    private void CancelBowPull()
    {
        isBowPulled = false;
        currentPullTime = 0.0f;

        if (currentArrow != null)
        {
            Destroy(currentArrow);
            currentArrow = null;
        }
    }

    private void UpdateBowPullAnimation(float pullTime)
    {
        if (currentArrow == null) return;

        float pullRatio = pullTime / maxPullTime;

        handTransform.localPosition = Vector3.Lerp(handRestPosition, handMaxPullPosition, pullRatio);
        bowTransform.localPosition = Vector3.Lerp(bowRestPosition, bowPullPosition, pullRatio);
        bowTransform.localRotation = Quaternion.Lerp(Quaternion.Euler(bowRestRotation), Quaternion.Euler(bowPulledRotation), pullRatio);
        if (arrowTransform)
        {
            arrowTransform.localPosition = Vector3.Lerp(arrowRestPosition, arrowPullPosition, pullRatio);
            arrowTransform.localRotation = Quaternion.Lerp(Quaternion.Euler(arrowRestRotation), Quaternion.Euler(arrowPullRotation), pullRatio);
        }
    }

    private Vector3 CalculateAdjustedShootDirection()
    {
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraForwardHorizontal = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
        float pitchAngle = Vector3.Angle(cameraForward, cameraForwardHorizontal);

        Vector3 adjustedShootDirection = Quaternion.AngleAxis(pitchAngle * pitchAdjustmentFactor, cameraTransform.right) * cameraForward;
        return adjustedShootDirection;
    }
}
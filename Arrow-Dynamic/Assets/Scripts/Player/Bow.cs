using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bow : MonoBehaviour
{
    //////////////////////////////////////////////////
    // Instance Settings
    //////////////////////////////////////////////////

    [Header("References")]
    public PlayerController playerController;
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public Transform cameraTransform;
    public Transform handTransform;

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

    //////////////////////////////////////////////////
    // Public Properties and Methods
    //////////////////////////////////////////////////

    public void HandlePull(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);

        if (isCoolingDown)
        {
            if (currentPullTime == 0.0f) isCoolingDown = false;
            else return;
        }
        if (context.canceled)
        {
            if (arrowReady) ReleaseBow();
        }
        else if (context.started)
        {
            if (!isBowPulled) InitializeBowPull();
        }
    }

    public void HandleCancel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isBowPulled = false;
            isCoolingDown = true;
        }
    }

    //////////////////////////////////////////////////
    // Private Fields and Methods
    //////////////////////////////////////////////////

    private float currentPullTime = 0.0f;
    private GameObject currentArrow = null;
    private Transform arrowTransform;
    private bool arrowReady = false;
    private bool isBowPulled = false;
    private bool isCoolingDown = false;

    private void Start()
    {
        UpdateSettings();
        GameSettings.OnSettingsChanged += UpdateSettings;
    }

    private void Update()
    {
        if (isBowPulled) currentPullTime += Time.deltaTime;
        else currentPullTime -= 2 * Time.deltaTime;

        currentPullTime = Mathf.Clamp(currentPullTime, 0, maxPullTime);

        if (currentPullTime > arrowReadyTime) arrowReady = true;

        UpdateBowPullAnimation(currentPullTime);
    }

    private void OnDestroy()
    {
        GameSettings.OnSettingsChanged -= UpdateSettings;
    }

    private void InitializeBowPull()
    {
        isBowPulled = true;
        Quaternion shootRotation = cameraTransform.rotation;
        if (currentArrow != null) Destroy(currentArrow);
        currentArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, shootRotation);
        arrowTransform = currentArrow.transform;
        arrowTransform.parent = cameraTransform;
    }

    private void ReleaseBow()
    {
        isCoolingDown = true;
        arrowReady = false;
        isBowPulled = false;

        // Shoot the arrow
        Rigidbody arrowRigidbody = currentArrow.GetComponent<Arrow>().rb;
        arrowRigidbody.isKinematic = false;
        arrowRigidbody.useGravity = true;
        Vector3 shootDirection = cameraTransform.forward;
        arrowTransform.parent = null;
        arrowRigidbody.AddForce(shootDirection * currentPullTime * arrowSpeed, ForceMode.Impulse);

        arrowTransform = null;
    }

    private void UpdateBowPullAnimation(float pullTime)
    {
        if (currentArrow == null) return;

        float pullRatio = pullTime / maxPullTime;

        handTransform.localPosition = Vector3.Lerp(handRestPosition, handMaxPullPosition, pullRatio);
        transform.localPosition = Vector3.Lerp(bowRestPosition, bowPullPosition, pullRatio);
        transform.localRotation = Quaternion.Lerp(Quaternion.Euler(bowRestRotation), Quaternion.Euler(bowPulledRotation), pullRatio);
        if (arrowTransform)
        {
            arrowTransform.localPosition = Vector3.Lerp(arrowRestPosition, arrowPullPosition, pullRatio);
            arrowTransform.localRotation = Quaternion.Lerp(Quaternion.Euler(arrowRestRotation), Quaternion.Euler(arrowPullRotation), pullRatio);
        }
    }

    IEnumerator StartAutoFire()
    {
        while (true)
        {
            // Begin pulling the bow
            if (!isBowPulled)
            {
                InitializeBowPull();
                // playerController.UpdateAimingState(true);
            }

            // Gradually pull the bow to its maximum over maxPullTime duration
            float pullStartTime = Time.time;
            while (Time.time - pullStartTime < maxPullTime)
            {
                currentPullTime = Mathf.Lerp(0, maxPullTime, (Time.time - pullStartTime) / maxPullTime);
                yield return null;
            }
            currentPullTime = maxPullTime;
            ReleaseBow();
            yield return new WaitForSeconds(autoFireInterval);
        }
    }

    //////////////////////////////////////////////////
    // Settings
    //////////////////////////////////////////////////

    private float arrowSpeed;
    private float arrowReadyTime;
    private float maxPullTime;
    private float autoFireInterval;

    private void UpdateSettings()
    {
        GameSettings settings = GameManager.GetSettings();

        arrowSpeed = settings.gameplay.arrowSpeed;
        arrowReadyTime = settings.gameplay.arrowReadyTime;
        maxPullTime = settings.gameplay.maxPullTime;

        autoFireInterval = settings.developer.autoFireInterval;

        if (settings.developer.autoFireEnabled)
            StartCoroutine(StartAutoFire());
        else
            StopCoroutine(StartAutoFire());
    }
}
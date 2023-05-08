using UnityEngine;

public class Bow : MonoBehaviour
{
    private GameSettings settings;
    private GameState state;

    [Header("References")]
    public GameObject playerObj;
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

    private float currentPullTime = 0.0f;
    private GameObject currentArrow = null;
    private Transform arrowTransform;
    private bool arrowReady = false;
    private bool isBowPulled = false;

    private PlayerMovement playerMovement;

    void Start()
    {
        settings = GameManager.GetSettings();
        state = GameManager.GetState();

        playerMovement = playerObj.GetComponent<PlayerMovement>();
    }

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
            currentPullTime -= 2 * Time.deltaTime;

        UpdateBowPullAnimation(currentPullTime);
    }

    private void PullBow()
    {
        if (currentPullTime > settings.gameplay.arrowReadyTime && !isBowPulled) // So can't spam
        {
            currentPullTime -= 2 * Time.deltaTime;
            return;
        }

        if (!isBowPulled)
        {
            InitializeBowPull();
            playerMovement.UpdateAimSpeedMultiplier(true);
        }
        else if (currentPullTime > settings.gameplay.arrowReadyTime)
        {
            arrowReady = true;
        }

        // Increase the bow pull distance up to the maximum
        currentPullTime += Time.deltaTime;
        currentPullTime = Mathf.Clamp(currentPullTime, 0, settings.gameplay.maxPullTime);
    }

    private void InitializeBowPull()
    {
        isBowPulled = true;
        Quaternion shootRotation = cameraTransform.rotation;
        currentArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, shootRotation);
        arrowTransform = currentArrow.transform;
        arrowTransform.parent = cameraTransform;
    }

    private void ReleaseBow()
    {
        arrowReady = false;
        isBowPulled = false;
        playerMovement.UpdateAimSpeedMultiplier(false);

        // Shoot the arrow
        Rigidbody arrowRigidbody = currentArrow.GetComponent<Arrow>().rb;
        arrowRigidbody.isKinematic = false;
        arrowRigidbody.useGravity = true;
        Vector3 shootDirection = cameraTransform.forward;
        arrowTransform.parent = null;
        arrowRigidbody.AddForce(shootDirection * currentPullTime * settings.gameplay.arrowSpeed, ForceMode.Impulse);

        arrowTransform = null;
    }

    private void CancelBowPull()
    {
        isBowPulled = false;
        currentPullTime = 0.0f;
        playerMovement.UpdateAimSpeedMultiplier(false);

        if (currentArrow != null)
        {
            Destroy(currentArrow);
            currentArrow = null;
        }
    }

    private void UpdateBowPullAnimation(float pullTime)
    {
        if (currentArrow == null) return;

        float pullRatio = pullTime / settings.gameplay.maxPullTime;

        handTransform.localPosition = Vector3.Lerp(handRestPosition, handMaxPullPosition, pullRatio);
        transform.localPosition = Vector3.Lerp(bowRestPosition, bowPullPosition, pullRatio);
        transform.localRotation = Quaternion.Lerp(Quaternion.Euler(bowRestRotation), Quaternion.Euler(bowPulledRotation), pullRatio);
        if (arrowTransform)
        {
            arrowTransform.localPosition = Vector3.Lerp(arrowRestPosition, arrowPullPosition, pullRatio);
            arrowTransform.localRotation = Quaternion.Lerp(Quaternion.Euler(arrowRestRotation), Quaternion.Euler(arrowPullRotation), pullRatio);
        }
    }
}
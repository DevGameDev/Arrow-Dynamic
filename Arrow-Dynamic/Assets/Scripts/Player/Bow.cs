using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class Bow : MonoBehaviour
{
    //////////////////////////////////////////////////
    // Instance Settings
    //////////////////////////////////////////////////

    [Header("References")]
    public PlayerController playerController;
    public Transform arrowSpawnPoint;
    public Transform cameraTransform;
    public Transform handTransform;

    [Header("References")]
    public GameObject basicArrowPrefab;
    public GameObject iceArrowPrefab;
    public GameObject grappleArrowPrefab;
    public GameObject bombArrowPrefab;
    public GameObject gravityArrowPrefab;
    public GameObject bubbleArrowPrefab;
    public GameObject timeArrowPrefab;
    public GameObject windArrowPrefab;

    [Header("Animation Positions")]
    public Vector3 handRestPosition;
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

    public static Bow Instance { get; set; }

    public void HandlePull(InputAction.CallbackContext context)
    {
        if (!bowReady || PlayerController.disabled) return;

        if (context.canceled)
        {
            if (arrowReady) { ReleaseBow(); }
            else if (isBowPulled)
            {
                isBowPulled = false;
                isCoolingDown = true;
            }
            else pullWaiting = false;
        }
        else if (context.started)
        {
            if (isCoolingDown)
            {
                pullWaiting = true;
                return;
            }
            if (!isBowPulled) InitializeBowPull();
        }
    }

    public void HandleCancel(InputAction.CallbackContext context)
    {
        if (context.performed) CancelPull();
    }

    public void SetBobOffset(float heightDifference)
    {
        bobOffset = -heightDifference;
    }

    public void SetArrowType(ArrowType type)
    {
        if (currentArrow != null)
        {
            Destroy(currentArrowObj);
            currentArrow = null;
            currentArrowObj = null;
        }
        CancelPull();
        currentArrowType = type;
    }

    public ArrowType GetArrowType()
    {
        return currentArrowType;
    }

    //////////////////////////////////////////////////
    // Private Fields and Methods
    //////////////////////////////////////////////////

    private bool bowReady = false;
    private Dictionary<ArrowType, GameObject> arrowPrefabs;
    private Queue<GameObject> shotArrows = new Queue<GameObject>();
    private ArrowType currentArrowType;
    private GameObject currentArrowObj = null;
    private IArrow currentArrow = null;
    private Transform arrowTransform;
    private float currentPullTime = 0.0f;
    private bool arrowReady = false;
    private bool isBowPulled = false;
    private bool isCoolingDown = false;
    private bool pullWaiting = false;
    private float initialHeight;
    private float bobOffset;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            GameManager.Instance.HandleGameQuit(false, "Duplicate Bows");
        }
    }

    private void Start()
    {
        UpdateSettings();
        GameSettings.OnSettingsChanged += UpdateSettings;

        arrowPrefabs = new Dictionary<ArrowType, GameObject>()
        {
            { ArrowType.Basic, basicArrowPrefab },
            { ArrowType.Ice, iceArrowPrefab },
            { ArrowType.Grapple, grappleArrowPrefab },
            { ArrowType.Bomb, bombArrowPrefab },
            { ArrowType.Gravity, gravityArrowPrefab },
            { ArrowType.Bubble, bubbleArrowPrefab },
            { ArrowType.Time, timeArrowPrefab },
            { ArrowType.Wind, windArrowPrefab }
        };

        currentArrowType = ArrowType.Basic;
        initialHeight = transform.position.y;

        bowReady = true;
    }

    private void Update()
    {
        if (currentPullTime == 0.0f)
        {
            isCoolingDown = false;
            if (pullWaiting) InitializeBowPull();
        }

        if (isBowPulled)
        {
            currentPullTime += Time.deltaTime;
            if (currentPullTime > arrowReadyTime) arrowReady = true;
        }
        else currentPullTime -= 2 * Time.deltaTime;

        currentPullTime = Mathf.Clamp(currentPullTime, 0, maxPullTime);

        UpdateBowPullAnimation(currentPullTime);
    }

    private void OnDestroy()
    {
        GameSettings.OnSettingsChanged -= UpdateSettings;
    }

    private void InitializeBowPull()
    {
        pullWaiting = false;

        if (shotArrows.Count > maxArrows)
        {
            GameObject oldArrow = shotArrows.Dequeue();
            if (oldArrow != null) Destroy(oldArrow);
        }

        if (currentArrow != null)
        {
            Destroy(currentArrowObj);
            currentArrow = null;
            currentArrowObj = null;
        }

        if (arrowPrefabs == null || arrowPrefabs[currentArrowType] == null) return;
        GameObject arrowPrefab = arrowPrefabs[currentArrowType];

        isBowPulled = true;
        Quaternion shootRotation = cameraTransform.rotation;
        currentArrowObj = Instantiate(arrowPrefab, arrowSpawnPoint.position, shootRotation);
        currentArrowObj.SetActive(false);

        currentArrow = currentArrowObj.GetComponent<IArrow>();
        currentArrow.OnLoad();

        arrowTransform = currentArrowObj.transform;
        arrowTransform.parent = cameraTransform;
        currentArrowObj.SetActive(true);
    }

    private void CancelPull()
    {
        isBowPulled = false;
        isCoolingDown = true;
    }

    private void ReleaseBow()
    {
        isCoolingDown = true;
        arrowReady = false;
        isBowPulled = false;
        if (currentArrow == null) return;

        // Shoot the arrow


        Rigidbody arrowRigidbody = currentArrow.rb;
        arrowRigidbody.isKinematic = false;
        arrowRigidbody.useGravity = true;
        Vector3 shootDirection = cameraTransform.forward;
        arrowTransform.parent = null;
        arrowTransform = null;

        arrowRigidbody.AddForce(shootDirection * currentPullTime * arrowSpeed, ForceMode.Impulse);
        currentArrow.OnRelease();

        shotArrows.Enqueue(currentArrowObj);

        currentArrowObj = null;
        currentArrow = null;

        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    private void UpdateBowPullAnimation(float pullTime)
    {
        float pullRatio = pullTime / maxPullTime;

        handTransform.localPosition = Vector3.Lerp(handRestPosition, handMaxPullPosition, pullRatio);
        transform.localPosition = Vector3.Lerp(bowRestPosition, bowPullPosition, pullRatio);
        transform.localRotation = Quaternion.Lerp(Quaternion.Euler(bowRestRotation), Quaternion.Euler(bowPulledRotation), pullRatio);
        if (currentArrowObj && arrowTransform)
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
            // Play the audio clip
            string arrowSwishClipPath = "Audio/SFX/arrow_swish";
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                AudioClip arrowSwishClip = Resources.Load<AudioClip>(arrowSwishClipPath);
                if (arrowSwishClip != null)
                {
                    audioSource.PlayOneShot(arrowSwishClip);
                }
                else
                {
                    Debug.LogWarning("Arrow swish audio clip not found: " + arrowSwishClipPath);
                }
            }
            yield return new WaitForSeconds(autoFireInterval);
        }
    }

    //////////////////////////////////////////////////
    // Settings
    //////////////////////////////////////////////////

    private float arrowSpeed;
    private float arrowReadyTime;
    private float maxPullTime;
    private float maxArrows;

    private float autoFireInterval;

    private void UpdateSettings()
    {
        GameSettings settings = GameManager.GetSettings();

        arrowSpeed = settings.gameplay.arrowSpeed;
        arrowReadyTime = settings.gameplay.arrowReadyTime;
        maxPullTime = settings.gameplay.maxPullTime;
        maxArrows = settings.gameplay.maxArrows;

        autoFireInterval = settings.developer.autoFireInterval;

        if (settings.developer.autoFireEnabled)
            StartCoroutine(StartAutoFire());
        else
            StopCoroutine(StartAutoFire());
    }
}
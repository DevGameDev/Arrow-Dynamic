using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class WeaponWheelController : MonoBehaviour
{
    public static WeaponWheelController Instance { get; set; }
    public Animator anim;
    public Image selectedItem;
    public Sprite noImage;
    public bool open = false;
    public CanvasGroup WeaponWheel;
    public GameObject crosshair;

    // Update is called once per frame
    public void input_action(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            open = true;
            WeaponWheel.alpha = 1;
            WeaponWheel.interactable = true;
            Time.timeScale = arrowWheelTimeScale;
            crosshair.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else if (context.canceled && open)
        {
            open = false;
            WeaponWheel.alpha = 0;
            WeaponWheel.interactable = false;
            Time.timeScale = baseTimeScale;
            crosshair.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        selectedItem.sprite = noImage;
        WeaponWheel.alpha = 0;

        UpdateSettings();
        GameSettings.OnSettingsChanged += UpdateSettings;
    }

    private void OnDestroy()
    {
        GameSettings.OnSettingsChanged -= UpdateSettings;
    }

    private float baseTimeScale = 1f;
    private float arrowWheelTimeScale = 1f;
    private void UpdateSettings()
    {
        GameSettings settings = GameManager.GetSettings();
        baseTimeScale = settings.gameplay.baseTimeScale;
        arrowWheelTimeScale = settings.gameplay.arrowWheelTimeScale;
    }
}

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

    public void input_action(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PlayerController.Instance.arrowWheelActive = true;
            open = true;
            WeaponWheel.alpha = 1;
            WeaponWheel.interactable = true;
            crosshair.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else if (context.canceled && open)
        {
            PlayerController.Instance.arrowWheelActive = false;
            open = false;
            WeaponWheel.alpha = 0;
            WeaponWheel.interactable = false;
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

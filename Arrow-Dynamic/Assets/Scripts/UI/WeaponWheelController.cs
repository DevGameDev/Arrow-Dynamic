using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class WeaponWheelController : MonoBehaviour
{
    public static WeaponWheelController Instance { get; set; }
    public Animator anim;
    public Image selectedItem;
    public Sprite noImage;
    public ArrowType selectedType = ArrowType.Basic;
    public bool open = false;
    public CanvasGroup WeaponWheel;
    // Update is called once per frame
    public void input_action(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            open = true;
            WeaponWheel.alpha = 1;
            WeaponWheel.interactable = true;
            Time.timeScale = arrowWheelTimeScale;
            selectedType = Bow.Instance.GetArrowType();
            //InputManager.Instance.SetInputActionMap(InputManager.InputMapType.ArrowWheel);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else if (context.canceled && open)
        {
            open = false;
            WeaponWheel.alpha = 0;
            WeaponWheel.interactable = false;
            Time.timeScale = baseTimeScale;
           //Bow.Instance.SetArrowType(selectedType);
            //InputManager.Instance.SetInputActionMap(InputManager.InputMapType.Gameplay);
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

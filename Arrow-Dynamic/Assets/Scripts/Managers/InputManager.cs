using System;
using UnityEngine;

/// <summary>
/// Handles user input and dispatches input events.
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; } // Singleton

    private GameSettings settings;
    private GameState state;

    // Events
    public event Action OnJump;
    public event Action OnSprint;
    public event Action OnCrouch;
    public event Action OnFire;
    public event Action<float> OnHorizontal;
    public event Action<float> OnVertical;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            GameManager.Instance.HandleGameQuit(false, "Duplicate InputManagers's");
        }
    }

    private void Start()
    {
        settings = GameManager.GetSettings();
        state = GameManager.GetState();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
            OnJump?.Invoke();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            OnSprint?.Invoke();

        if (Input.GetKeyDown(KeyCode.LeftControl))
            OnCrouch?.Invoke();

        if (Input.GetMouseButtonDown(0))
            OnFire?.Invoke();

        float horizontal = Input.GetAxis("Horizontal");
        OnHorizontal?.Invoke(horizontal);

        float vertical = Input.GetAxis("Vertical");
        OnVertical?.Invoke(vertical);
    }
}
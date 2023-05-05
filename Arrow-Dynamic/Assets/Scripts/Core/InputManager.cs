using UnityEngine;

/// <summary>
/// Handles user input and dispatches input events.
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; } // Singleton

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
}
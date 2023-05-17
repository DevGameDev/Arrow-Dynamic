using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", order = 0)]
public class GameSettings : ScriptableObject
{
    // public 
    [Header("Developer Settings")]
    public DeveloperSettings developer;

    [Header("Gameplay Settings")]
    public GameplaySettings gameplay;

    [Header("Input Settings")]
    public InputSettings input;

    [Header("Display Settings")]
    public DisplaySettings display;

    [Header("Audio Settings")]
    public AudioSettings audio;

    // For notifying other scripts that something changed. 
    public static Action OnSettingsChanged;
    public void NotifySettingsChanged()
    {
        OnSettingsChanged?.Invoke();
        Debug.Log("Settings Updated.");
    }

    public static GameSettings Instance { get; set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

}

[Serializable]
public class DeveloperSettings
{
    public bool debugEnabled = true;
    public bool autoFireEnabled = false;
    public float autoFireInterval = 1.0f;
}

[Serializable]
public class GameplaySettings
{
    public GameStates initialState = GameStates.MainMenu;

    [Header("Basic")]
    public float baseTimeScale = 1f;

    [Header("Movement")]
    public float speed = 10f;
    public float maxVelocity = 15f;
    public float moveSmoothing = 20f;
    public float sidewaysSpeedMultiplier = 0.9f;
    public float reverseSpeedMultiplier = 0.6f;
    public float sprintSpeedMultiplier = 1.5f;
    public float aimSpeedMultiplier = 0.8f;

    [Header("Jump")]
    public float jumpForce = 500000.0f;
    public float doubleJumpForce = 45555555.0f;
    public float airSpeedMultiplier = 5555555.25f;
    public float lowJumpMultiplier = 1f;
    public float fallMultiplier = 1f;
    public bool airControl = false;
    public float groundCheckRadius = 0.5f;
    public LayerMask groundMask;

    [Header("Crouch")]
    public float crouchSpeedMultiplier = 0.7f;
    public float crouchHeight = 0.5f;
    public float standingHeight = 2.0f;

    [Header("Arrows")]
    public float arrowSpeed = 2;
    public float maxPullTime = 2;
    public float arrowReadyTime = 0.25f;
    public int maxArrows = 10;

    [Header("Arrow Wheel")]
    public float arrowWheelTimeScale = 0.75f;
}

[Serializable]
public class InputSettings
{
    [Header("Mouse")]
    public float mouseSensitivity = 2;
}

[Serializable]
public class DisplaySettings
{
    public float viewSmoothing = 2;
    public bool enableCameraBobbing = true;
    public float bobbingSpeed = 0.18f;
    public float bobbingAmount = 0.2f;
    public float bobbingMinSpeed = 0.1f;
}

[Serializable]
public class AudioSettings
{
    // audio settings here
}

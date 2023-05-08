using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", order = 0)]
public class GameSettings : ScriptableObject
{
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
}

[Serializable]
public class DeveloperSettings
{
    public bool debugEnabled = true;
}

[Serializable]
public class GameplaySettings
{
    public GameStates initialState = GameStates.MainMenu;

    [Header("Player Movement")]
    public float speed = 10.0f;
    public float sidewaysSpeedMultiplier = 0.9f;
    public float reverseSpeedMultiplier = 0.6f;
    public float sprintSpeedMultiplier = 1.5f;
    public float crouchSpeedMultiplier = 0.7f;
    public float aimSpeedMultiplier = 0.8f;
    public float jumpForce = 5.0f;
    public float doubleJumpForce = 4.0f;
    public float groundCheckRadius = 0.5f;
    public LayerMask groundMask;
    public float crouchHeight = 0.5f;
    public float standingHeight = 2.0f;

    [Header("Arrows")]
    [Range(0, 1000)] public float arrowSpeed = 2;
    [Range(0, 1000)] public float maxPullTime = 2;
    [Range(0, 1000)] public float arrowReadyTime = 0.25f;
    public int maxArrows = 10;
}

[Serializable]
public class InputSettings
{
    [Header("Mouse")]
    public float mouseSensitivity = 2;

    [Header("Movement")]
    public KeyCode moveForwardKey = KeyCode.W;
    public KeyCode moveBackwardKey = KeyCode.S;
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Actions")]
    public KeyCode pullKey = KeyCode.Mouse0;
    public KeyCode cancelKey = KeyCode.Q;
    public KeyCode interactKey = KeyCode.E;
    public KeyCode meleeAttackKey = KeyCode.F;

    [Header("Abilities")]
    public KeyCode ability1Key = KeyCode.Alpha1;
    public KeyCode ability2Key = KeyCode.Alpha2;
    public KeyCode ability3Key = KeyCode.Alpha3;

    [Header("Menus")]
    public KeyCode pauseKey = KeyCode.Escape;
    public KeyCode arrowWheelKey = KeyCode.Tab;

    [Header("Quick Slots")]
    public KeyCode quickSlot1Key = KeyCode.Alpha1;
    public KeyCode quickSlot2Key = KeyCode.Alpha2;
    public KeyCode quickSlot3Key = KeyCode.Alpha3;
    public KeyCode quickSlot4Key = KeyCode.Alpha4;
    public KeyCode quickSlot5Key = KeyCode.Alpha5;
    public KeyCode quickSlot6Key = KeyCode.Alpha6;
    public KeyCode quickSlot7Key = KeyCode.Alpha7;
    public KeyCode quickSlot8Key = KeyCode.Alpha8;
    public KeyCode quickSlot9Key = KeyCode.Alpha9;
}

[Serializable]
public class DisplaySettings
{
    public float viewSmoothing = 2;
    public bool enableCameraBobbing = true;
    public float bobbingSpeed = 0.18f;
    public float bobbingAmount = 0.2f;
}

[Serializable]
public class AudioSettings
{
    // audio settings here
}

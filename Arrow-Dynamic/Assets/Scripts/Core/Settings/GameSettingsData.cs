using System;
using UnityEngine;

/// <summary>
/// The structure for accessing settings in game. 
/// </summary>
[Serializable]
public class GameSettingsData
{
    // Developer
    public bool debugEnabled;

    // Audio 
    public float musicVolume;
    public float soundEffectsVolume;

    // Lighting 
    public bool lightingDayCycleEnabled;
    public float lightingDayCycleDurationSeconds;
    public Color lightingDefaultColor;
    public float lightingDefaultIntensity;
    public Color lightingMorningColor;
    public float lightingMorningIntensity;
    public Color lightingDayColor;
    public float lightingDayIntensity;
    public Color lightingAfternoonColor;
    public float lightingAfternoonIntensity;
    public Color lightingNightColor;
    public float lightingNightIntensity;
    public Color defaultSkyColor;
    public Color morningSkyColor;
    public Color daySkyColor;
    public Color afternoonSkyColor;
    public Color nightSkyColor;

    // Controls
    public float mouseSensitivity;

    // Gameplay
    public float timeScale;
    public float arrowSpeed;
    public float maxPullTime;
    public float arrowReadyTime;
    public float pitchAdjustmentFactor;

    // Graphics
    public int screenResolutionWidth;
    public int screenResolutionHeight;
    public bool fullscreen;
    public int qualityLevel;
}
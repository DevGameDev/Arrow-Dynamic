using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings", order = 0)]
public class GameSettings : ScriptableObject
{
    [Header("Developer Settings")]
    public bool debugEnabled = true;

    [Header("Initial State Settings")]
    public GameStates intialState = GameStates.MainMenu;

    [Header("Audio Settings")]
    [Range(0, 1)] public float musicVolume = 1;
    [Range(0, 1)] public float soundEffectsVolume = 1;

    [Header("Lighting Settings")]
    public bool lightingDayCycleEnabled = false;
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

    [Header("Control Settings")]
    [Range(0, 5)] public float mouseSensitivity = 2;

    [Header("Gameplay Settings")]
    [Range(0, 5)] public float timeScale = 1;
    [Range(0, 5)] public float arrowSpeed = 2;
    [Range(0, 5)] public float maxPullTime = 2;
    [Range(0, 5)] public float arrowReadyTime = 0.25f;

    [Header("Graphics Settings")]
    public int screenResolutionWidth = 1920;
    public int screenResolutionHeight = 1080;
    public bool fullscreen = true;
    public int qualityLevel = 2;

    public GameSettingsData GetGameData()
    {
        GameSettingsData data = new GameSettingsData();

        // Developer
        data.debugEnabled = debugEnabled;

        // Audio
        data.musicVolume = musicVolume;
        data.soundEffectsVolume = soundEffectsVolume;

        // Lighting
        data.lightingDayCycleEnabled = lightingDayCycleEnabled;
        data.lightingDayCycleDurationSeconds = lightingDayCycleDurationSeconds;
        data.lightingDefaultColor = lightingDefaultColor;
        data.lightingDefaultIntensity = lightingDefaultIntensity;
        data.lightingMorningColor = lightingMorningColor;
        data.lightingMorningIntensity = lightingMorningIntensity;
        data.lightingDayColor = lightingDayColor;
        data.lightingDayIntensity = lightingDayIntensity;
        data.lightingAfternoonColor = lightingAfternoonColor;
        data.lightingAfternoonIntensity = lightingAfternoonIntensity;
        data.lightingNightColor = lightingNightColor;
        data.lightingNightIntensity = lightingNightIntensity;
        data.defaultSkyColor = defaultSkyColor;
        data.morningSkyColor = morningSkyColor;
        data.daySkyColor = daySkyColor;
        data.afternoonSkyColor = afternoonSkyColor;
        data.nightSkyColor = nightSkyColor;

        // Gameplay
        data.timeScale = timeScale;
        data.mouseSensitivity = mouseSensitivity;
        data.arrowSpeed = arrowSpeed;
        data.maxPullTime = maxPullTime;
        data.arrowReadyTime = arrowReadyTime;

        // Graphics
        data.screenResolutionWidth = screenResolutionWidth;
        data.screenResolutionHeight = screenResolutionHeight;
        data.fullscreen = fullscreen;
        data.qualityLevel = qualityLevel;
        return data;
    }

    public void ReadGameData(GameSettingsData data)
    {
        // Developer
        debugEnabled = data.debugEnabled;

        // Audio
        musicVolume = data.musicVolume;
        soundEffectsVolume = data.soundEffectsVolume;

        // Lighting
        lightingDayCycleEnabled = data.lightingDayCycleEnabled;
        lightingDayCycleDurationSeconds = data.lightingDayCycleDurationSeconds;
        lightingDefaultColor = data.lightingDefaultColor;
        lightingDefaultIntensity = data.lightingDefaultIntensity;
        lightingMorningColor = data.lightingMorningColor;
        lightingMorningIntensity = data.lightingMorningIntensity;
        lightingDayColor = data.lightingDayColor;
        lightingDayIntensity = data.lightingDayIntensity;
        lightingAfternoonColor = data.lightingAfternoonColor;
        lightingAfternoonIntensity = data.lightingAfternoonIntensity;
        lightingNightColor = data.lightingNightColor;
        lightingNightIntensity = data.lightingNightIntensity;
        defaultSkyColor = data.defaultSkyColor;
        morningSkyColor = data.morningSkyColor;
        daySkyColor = data.daySkyColor;
        afternoonSkyColor = data.afternoonSkyColor;
        nightSkyColor = data.nightSkyColor;

        // Gameplay
        timeScale = data.timeScale;
        mouseSensitivity = data.mouseSensitivity;
        arrowSpeed = data.arrowSpeed;
        maxPullTime = data.maxPullTime;
        arrowReadyTime = data.arrowReadyTime;

        // Graphics
        screenResolutionWidth = data.screenResolutionWidth;
        screenResolutionHeight = data.screenResolutionHeight;
        fullscreen = data.fullscreen;
        qualityLevel = data.qualityLevel;
    }
}
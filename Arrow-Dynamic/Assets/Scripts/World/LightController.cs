using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LightingModes
{
    Default,
    DayCycle,
    Morning,
    Day,
    Afternoon,
    Night
}

public class LightController : MonoBehaviour
{
    public static LightController Instance { get; private set; }

    [SerializeField] private Light mainLight;

    private void Start()
    {
        if (dayCycleEnabled) SetLightingMode(LightingModes.DayCycle);
        else SetLightingMode(LightingModes.Default);
    }

    private void Update()
    {
        if (currentMode == LightingModes.DayCycle) RefreshDayCycle();
    }

    public void UpdateSettings()
    {
        GameSettings settings = SettingsManager.Instance.gameSettings;

        dayCycleEnabled = settings.lightingDayCycleEnabled;
        dayCycleDurationSeconds = settings.lightingDayCycleDurationSeconds;
        typeTime = dayCycleDurationSeconds / 4;
        morningEndTime = dayCycleDurationSeconds / 4;
        dayEndTime = dayCycleDurationSeconds / 2;
        afternoonEndTime = 3 * dayCycleDurationSeconds / 4;

        defaultColor = settings.lightingDefaultColor;
        defaultIntensity = settings.lightingDefaultIntensity;

        morningLightColor = settings.lightingMorningColor;
        morningIntensity = settings.lightingMorningIntensity;

        dayLightColor = settings.lightingDayColor;
        dayIntensity = settings.lightingDayIntensity;

        afternoonLightColor = settings.lightingAfternoonColor;
        afternoonIntensity = settings.lightingAfternoonIntensity;

        nightLightColor = settings.lightingNightColor;
        nightIntensity = settings.lightingNightIntensity;

        defaultSkyColor = settings.defaultSkyColor;
        morningSkyColor = settings.morningSkyColor;
        daySkyColor = settings.daySkyColor;
        afternoonSkyColor = settings.afternoonSkyColor;
        nightSkyColor = settings.nightSkyColor;
    }

    public bool hasReset { get; set; } = true;
    public void Reset()
    {
        if (dayCycleEnabled) SetLightingMode(LightingModes.DayCycle);
        else SetLightingMode(LightingModes.Default);
    }

    public void SetLightingMode(LightingModes mode)
    {
        if (mode is LightingModes.Default)
        {
            mainLight.color = defaultColor;
            mainLight.intensity = defaultIntensity;
            mainLight.colorTemperature = defaultTemperature;
            RenderSettings.skybox.SetColor("default", defaultSkyColor);
        }
        else if (mode is LightingModes.Morning)
        {
            mainLight.color = morningLightColor;
            mainLight.intensity = morningIntensity;
            mainLight.colorTemperature = morningTemperature;
            RenderSettings.skybox.SetColor("morning", morningSkyColor);
        }
        else if (mode is LightingModes.Day)
        {
            mainLight.color = dayLightColor;
            mainLight.intensity = dayIntensity;
            mainLight.colorTemperature = dayTemperature;
            RenderSettings.skybox.SetColor("day", daySkyColor);
        }
        else if (mode is LightingModes.Afternoon)
        {
            mainLight.color = afternoonLightColor;
            mainLight.intensity = afternoonIntensity;
            mainLight.colorTemperature = afternoonTemperature;
            RenderSettings.skybox.SetColor("afternoon", afternoonSkyColor);
        }
        else if (mode is LightingModes.Night)
        {
            mainLight.color = nightLightColor;
            mainLight.intensity = nightIntensity;
            mainLight.colorTemperature = nightTemperature;
            RenderSettings.skybox.SetColor("night", nightSkyColor);
        }
        else if (mode is LightingModes.DayCycle) StartDayCycle(dayCycleDurationSeconds / 4);
    }

    public void StartDayCycle(float startTime = 0)
    {
        dayCycleCurrentTime = startTime;

        currentMode = LightingModes.DayCycle;
    }

    public void Destroy()
    {
        Instance = null;
    }

    // Settings
    private bool dayCycleEnabled;
    private float dayCycleDurationSeconds;

    Color defaultColor;
    float defaultIntensity;
    float defaultTemperature;

    Color morningLightColor;
    float morningIntensity;
    float morningTemperature;

    Color dayLightColor;
    float dayIntensity;
    float dayTemperature;

    Color afternoonLightColor;
    float afternoonIntensity;
    float afternoonTemperature;

    Color nightLightColor;
    float nightIntensity;
    float nightTemperature;

    private Color defaultSkyColor;
    private Color morningSkyColor;
    private Color daySkyColor;
    private Color afternoonSkyColor;
    private Color nightSkyColor;

    // Day Cycle
    private LightingModes currentMode;
    private float dayCycleCurrentTime = 0;

    private float typeTime;
    private float morningEndTime;
    private float dayEndTime;
    private float afternoonEndTime;

    private void Awake()
    {
        if (Instance is not null) GameManager.Instance.HandleError(true, "An CameraController is being initialized when one already exists.");
        Instance = this;
    }

    private void RefreshDayCycle()
    {
        Color currentLightColor;
        Color currentSkyColor;
        float currentIntensity;
        float percentageProgress;

        dayCycleCurrentTime += Time.deltaTime;
        if (dayCycleCurrentTime > dayCycleDurationSeconds) dayCycleCurrentTime = 0;

        if (dayCycleCurrentTime < morningEndTime)
        {
            percentageProgress = 1 - (morningEndTime - dayCycleCurrentTime) / typeTime;
            currentLightColor = Color.Lerp(nightLightColor, morningLightColor, percentageProgress);
            currentIntensity = Mathf.Lerp(nightIntensity, morningIntensity, percentageProgress);
            currentSkyColor = Color.Lerp(nightSkyColor, morningSkyColor, percentageProgress);
        }
        else if (dayCycleCurrentTime < dayEndTime)
        {
            percentageProgress = 1 - (dayEndTime - dayCycleCurrentTime) / typeTime;
            currentLightColor = Color.Lerp(morningLightColor, dayLightColor, percentageProgress);
            currentIntensity = Mathf.Lerp(morningIntensity, dayIntensity, percentageProgress);
            currentSkyColor = Color.Lerp(morningSkyColor, daySkyColor, percentageProgress);
        }
        else if (dayCycleCurrentTime < afternoonEndTime)
        {
            percentageProgress = 1 - (afternoonEndTime - dayCycleCurrentTime) / typeTime;
            currentLightColor = Color.Lerp(dayLightColor, afternoonLightColor, percentageProgress);
            currentIntensity = Mathf.Lerp(dayIntensity, afternoonIntensity, percentageProgress);
            currentSkyColor = Color.Lerp(daySkyColor, afternoonSkyColor, percentageProgress);
        }
        else
        {
            percentageProgress = 1 - (dayCycleDurationSeconds - dayCycleCurrentTime) / typeTime;
            currentLightColor = Color.Lerp(afternoonLightColor, nightLightColor, percentageProgress);
            currentIntensity = Mathf.Lerp(afternoonIntensity, nightIntensity, percentageProgress);
            currentSkyColor = Color.Lerp(afternoonSkyColor, nightSkyColor, percentageProgress);
        }

        mainLight.color = currentLightColor;
        mainLight.intensity = currentIntensity;
        RenderSettings.skybox.SetColor("day-cycle", currentSkyColor);
    }
}
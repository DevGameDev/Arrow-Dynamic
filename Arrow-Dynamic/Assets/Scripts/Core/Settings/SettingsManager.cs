using System;
using System.IO;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; } // Singleton

    public GameSettings gameSettings;
    private string settingsFilePath;

    public event Action OnSettingsLoaded;
    public event Action OnSettingsSaved;
    public event Action OnSettingsApplied;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        settingsFilePath = Application.persistentDataPath + "/settings.json";
    }

    void Start()
    {
        LoadSettings();
    }

    public void LoadSettings()
    {
        if (File.Exists(settingsFilePath))
        {
            string json = File.ReadAllText(settingsFilePath);
            GameSettingsData data = JsonUtility.FromJson<GameSettingsData>(json);
            gameSettings.ReadGameData(data);
        }
        else
        {
            SaveSettings();
        }

        // Apply settings to the game
        ApplySettings();
        OnSettingsLoaded?.Invoke();
    }

    public void SaveSettings()
    {
        GameSettingsData data = gameSettings.GetGameData();
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(settingsFilePath, json);
        OnSettingsSaved?.Invoke();
    }

    public void ApplySettings()
    {
        Screen.SetResolution(gameSettings.screenResolutionWidth, gameSettings.screenResolutionHeight, gameSettings.fullscreen);
        QualitySettings.SetQualityLevel(gameSettings.qualityLevel);

        OnSettingsApplied?.Invoke(); // Let them all know
    }
}
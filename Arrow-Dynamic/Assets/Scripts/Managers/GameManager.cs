using System;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton

    public GameSettings settings;
    public static GameSettings GetSettings()
    {
        return Instance.settings;
    }

    public GameState state;
    public static GameState GetState()
    {
        return Instance.state;
    }

    private string settingsFilePath;
    private string stateFilePath;

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
            HandleGameQuit(false, "Duplicate GameManager's");
        }

        if (!settings)
            HandleGameQuit(false, "GameManger missing settings");

        if (!state)
            HandleGameQuit(false, "GameManger missing state");

        settingsFilePath = Application.persistentDataPath + "/settings.json";
        stateFilePath = Application.persistentDataPath + "/gameState.json";
    }

    private void Start()
    {
        LoadSettings(); // Must load settings before state
        LoadState();
    }

    private void Update()
    {
        switch (state.currentState)
        {
            case GameStates.Gameplay:
                // Handle Gameplay
                break;
            case GameStates.MainMenu:
                // Handle MainMenu
                break;
            default:
                Debug.LogError($"GameEngine reached undefined state: {state.currentState}"); // $ makes formatted string
                break;
        }
    }

    public void SetGameState(GameStates newState)
    {
        switch (state.currentState)
        {
            case GameStates.Gameplay:
                // Handle Gameplay
                break;
            case GameStates.MainMenu:
                // Handle MainMenu
                break;
            default:
                Debug.LogError($"SetGameState undefined state: {state.currentState}"); // $ makes formatted string
                break;
        }

        state.currentState = newState;
    }

    /// <summary>
    /// Should be called in any instance of application closing. Provides logging and saving of game state. 
    /// </summary>
    /// <param name="save">True if game should be saved.</param>
    /// <param name="message">An optional quit message.</param>
    /// <param name="exception">An optional system exception to be thrown.</param>
    public void HandleGameQuit(bool save, String message = "No quit message.", System.Exception exception = null)
    {
        if (save)
        {
            Debug.Log("Saving game...");
            SaveSettings();
            SaveState();
            Debug.Log("Game Saved.");
        }
        else
        {
            Debug.Log("Game Not Saved.");
        }

        if (exception is not null) throw exception;

        Debug.Log("Game Quit. " + message);
        Application.Quit();
    }

    private void LoadState()
    {
        if (File.Exists(stateFilePath))
        {
            string json = File.ReadAllText(stateFilePath);
            JsonUtility.FromJsonOverwrite(json, state);
        }
        else
        {
            SaveState();
        }
        if (state.currentState is GameStates.Initial)
            state.currentState = settings.gameplay.initialState;
    }

    private void SaveState()
    {
        string json = JsonUtility.ToJson(state);
        File.WriteAllText(stateFilePath, json);
    }

    private void LoadSettings()
    {
        if (File.Exists(settingsFilePath))
        {
            string json = File.ReadAllText(settingsFilePath);
            JsonUtility.FromJsonOverwrite(json, settings);
        }
        else
        {
            SaveSettings();
        }

        Time.timeScale = state.timeScale;
    }

    private void SaveSettings()
    {
        string json = JsonUtility.ToJson(settings);
        File.WriteAllText(settingsFilePath, json);
    }
}
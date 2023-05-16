using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //////////////////////////////////////////////////
    // Public Properties and Methods
    //////////////////////////////////////////////////

    public static GameManager Instance { get; private set; } // Singleton
    public GameSettings settings;
    public GameState state;

    public static GameSettings GetSettings()
    {
        return Instance.settings;
    }

    public static GameState GetState()
    {
        return Instance.state;
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
            case GameStates.Paused:
                // Handle Paused
                break;
            default:
                Debug.LogError($"SetGameState undefined state: {state.currentState}"); // $ makes formatted string
                break;
        }

        state.currentState = newState;
    }

    public void HandlePause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (state.currentState is GameStates.Paused)
            {
                state.currentState = state.lastState;
                state.lastState = GameStates.Paused;
                // TODO: close pause menu
            }
            else
            {
                state.lastState = state.currentState;
                state.currentState = GameStates.Paused;
                // TODO: Open pause menu?
            }
        }
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

    //////////////////////////////////////////////////
    // Private Fields and Methods
    //////////////////////////////////////////////////

    private string settingsFilePath;
    private string stateFilePath;

    private void Awake()
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
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A manager for a game instance.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameStates currentState = GameStates.Initial;
    private GameStateData state;

    // Settings
    private bool debugEnabled = false;
    private float timeScale = 1.0f;

    private void Awake()
    {
        if (Instance is not null) HandleError(true, "A GameManager is being initialized when one already exists.");
        Instance = this;
    }

    private void Start()
    {
        state = GameStateManager.Instance.gameStateData;

        SettingsManager.Instance.OnSettingsApplied += UpdateSettings;

        UpdateSettings();
        LoadState();
        HandleMessage("Game setup completed.", false);
    }

    private void Update()
    {
        if (currentState is GameStates.Gameplay)
        {
            // Handle Gameplay
        }
        else if (currentState is GameStates.MainMenu)
        {
            // Handle Gameplay
        }
        else
        {
            // Undefined current state
            HandleError(true, String.Format("GameEngine reached undefined state: {0}.", currentState));
        }
    }

    private void OnDestroy()
    {
        SaveState();
    }

    /// <summary>
    /// Handles and logs game errors appropriately. Requires that console and debug modes are enabled in GameSettings.
    /// </summary>
    /// <param name="fatal">True if error results in undefined game state.</param>
    /// <param name="message">An optional erorr message.</param>
    /// <param name="exception">An optional system exception to be thrown.</param>
    /// <remarks>
    /// Behavior will differ when ran with Unity editor.
    /// </remarks>
    public void HandleError(bool fatal, String message = "No fail message.", System.Exception exception = null)
    {
#if UNITY_EDITOR
        if (debugEnabled)
        {
            if (exception is null)
            {
                HandleMessage("Game failed with no exception. " + message, true);
            }
            else
            {
                HandleMessage("Game failed with exception. " + message, true);
            }

            if (fatal)
            {
                // HandleGameQuit(false, "Triggered by fail state.", exception);
                HandleMessage("Fatal game error. Paused.", true);
                PauseGame();
            }
        }
#else
            if (fatal) HandleGameQuit(false, "Triggered by fail state.", exception);
#endif
    }

    private void PauseGame()
    {
        Time.timeScale = 0.0f;
    }

    private void UnpauseGame()
    {
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
        }
        else
        {
            Debug.Log("Game not saved.");
        }

        HandleMessage("Game Quit. " + message, false);

        // DestroyGame();

        if (exception is not null) throw exception;

        Application.Quit();
    }

    /// <summary>
    /// Should be ultimately called for any console messages.  
    /// </summary>
    /// <param name="message">The console message.</param>
    /// <param name="error">True if error message.</param>
    private void HandleMessage(System.String message, bool error)
    {
        if (error)
        {
            Debug.LogError(System.String.Format("Error: {0}", message));
        }
        else
        {
            Debug.Log(System.String.Format("Message: {0}", message));
        }
    }

    private void UpdateSettings()
    {
        GameSettings settings = SettingsManager.Instance.gameSettings;

        if (state.currentState is GameStates.Initial) // If first time
            state.currentState = settings.intialState;

        timeScale = settings.timeScale;

        debugEnabled = settings.debugEnabled;
    }

    private void LoadState()
    {
        currentState = state.currentState;
    }

    private void SaveState()
    {
        state.currentState = currentState;
    }
}
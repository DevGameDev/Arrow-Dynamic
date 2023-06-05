using System;
using System.Collections;
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

    public GameObject tutorialChamber;
    public GameObject tutorialBlocker;
    public GameObject tutorialToOnePass;
    public GameObject levelOneChamber;
    public GameObject levelOneBlocker;
    public GameObject OneToTwoPass;
    public GameObject levelTwoChamber;
    public GameObject jungleLevel;
    public GameObject voidLevel;

    public GameObject playerObj;

    public static GameSettings GetSettings()
    {
        return Instance.settings;
    }

    public static GameState GetState()
    {
        return Instance.state;
    }

    private bool gameStarted = false;
    private float gameTimeScale;
    public void UpdateGameState(GameStates newState)
    {
        state.lastState = state.currentState;
        state.currentState = newState;

        // Handle end of last state
        switch (state.lastState)
        {
            case GameStates.Gameplay:
                break;
            case GameStates.MainMenu:
                break;
            case GameStates.PauseMenu:
                break;
            case GameStates.SettingsMenu:
                break;
            case GameStates.ControlsMenu:
                break;
            default:
                Debug.LogError($"SetGameState undefined last state: {state.lastState}"); // $ makes formatted string
                break;
        }

        // Setup new state
        switch (state.currentState)
        {
            case GameStates.Gameplay:
                if (!gameStarted)
                {
                    StartCoroutine(LoadTutorialChamber());
                    gameStarted = true;
                }
                else
                {
                    Time.timeScale = gameTimeScale;
                }
                InputManager.Instance.SetInputActionMap(InputManager.InputMapType.Gameplay);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case GameStates.MainMenu:
                InputManager.Instance.SetInputActionMap(InputManager.InputMapType.Menu);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                if (gameStarted)
                {
                    gameStarted = false;
                    Time.timeScale = gameTimeScale;
                    StartCoroutine(LoadMainMenu());
                }
                break;
            case GameStates.PauseMenu:
                InputManager.Instance.SetInputActionMap(InputManager.InputMapType.Menu);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                if (state.lastState == GameStates.Gameplay)
                    gameTimeScale = Time.timeScale;
                Time.timeScale = 0f;
                break;
            case GameStates.SettingsMenu:
                InputManager.Instance.SetInputActionMap(InputManager.InputMapType.Menu);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                break;
            case GameStates.ControlsMenu:
                InputManager.Instance.SetInputActionMap(InputManager.InputMapType.Menu);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                break;
            default:
                Debug.LogError($"SetGameState undefined current state: {state.currentState}"); // $ makes formatted string
                break;
        }
    }

    public void HandlePause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            UIManager.Instance.ControlPausePanel(true); // Will call UpdateGameForNewState
            UIManager.Instance.ControlGameplayPanel(false);
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

    public void HandleGameEvent(EventTypes type)
    {
        switch (type)
        {
            case EventTypes.LoadLevelTutorial:
                StartCoroutine(LoadTutorialChamber());
                break;
            case EventTypes.LoadLevelOne:
                StartCoroutine(LoadLevelOneChamber());
                break;
            case EventTypes.LoadLevelTwo:
                StartCoroutine(LoadLevelTwoChamber());
                break;
            case EventTypes.LoadLevelJungle:
                StartCoroutine(LoadJungle());
                break;
            case EventTypes.LoadLevelVoid:
                StartCoroutine(LoadVoid());
                break;
        }
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
        //Debug.Log(1 / Time.deltaTime); // FPS
        // Setup new state
        switch (state.currentState)
        {
            case GameStates.Gameplay:
                break;
            case GameStates.MainMenu:
                break;
            case GameStates.PauseMenu:
                break;
            case GameStates.SettingsMenu:
                break;
            case GameStates.ControlsMenu:
                break;
            default:
                Debug.LogError($"GameManager.Update() undefined current state: {state.currentState}"); // $ makes formatted string
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

    private IEnumerator LoadTutorialChamber()
    {
        tutorialBlocker.SetActive(false);
        levelOneChamber.SetActive(false);
        levelOneBlocker.SetActive(false);
        OneToTwoPass.SetActive(false);
        levelTwoChamber.SetActive(false);
        jungleLevel.SetActive(false);
        voidLevel.SetActive(false);
        tutorialChamber.SetActive(true);
        tutorialToOnePass.SetActive(true);

        playerObj.SetActive(true);

        yield return null;
    }

    private IEnumerator LoadLevelOneChamber()
    {
        GameplaySetInputEnabled(false);
        StartCoroutine(PlayerController.Instance.ShakeCamera(4, 20f, 2));
        AudioManager.Instance.PlaySFX(AudioManager.SoundEffect.RockImpact, 1.0f);
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlaySFX(AudioManager.SoundEffect.RockFall, 1.0f);

        tutorialChamber.SetActive(false);
        tutorialBlocker.SetActive(true);
        levelOneChamber.SetActive(true);
        OneToTwoPass.SetActive(true);

        yield return new WaitForSeconds(3.5f);

        GameplaySetInputEnabled(true);

        yield return null;
    }

    private IEnumerator LoadLevelTwoChamber()
    {
        GameplaySetInputEnabled(false);
        StartCoroutine(PlayerController.Instance.ShakeCamera(5, 20f, 2));
        AudioManager.Instance.PlaySFX(AudioManager.SoundEffect.RockFall, 1.0f);

        tutorialBlocker.SetActive(false);
        tutorialToOnePass.SetActive(false);
        levelOneBlocker.SetActive(true);
        levelOneChamber.SetActive(false);
        levelTwoChamber.SetActive(true);

        yield return new WaitForSeconds(5);

        GameplaySetInputEnabled(true);

        yield return null;
    }

    private IEnumerator LoadJungle()
    {
        GameplaySetInputEnabled(false);
        yield return StartCoroutine(UIManager.Instance.ControlFade(true, 2));

        jungleLevel.SetActive(true);
        PlayerController.Instance.SpawnPlayer(SpawnPoints.JungleStart);
        levelOneBlocker.SetActive(false);
        levelTwoChamber.SetActive(false);
        OneToTwoPass.SetActive(false);

        StartCoroutine(AudioManager.Instance.ChangeSong(AudioManager.Song.JungleTheme, 2));

        GameplaySetInputEnabled(true);

        yield return StartCoroutine(UIManager.Instance.ControlFade(false, 2));


        yield return null;
    }

    private IEnumerator LoadVoid()
    {
        GameplaySetInputEnabled(false);
        yield return StartCoroutine(UIManager.Instance.ControlFade(true, 2));

        voidLevel.SetActive(true);
        PlayerController.Instance.SpawnPlayer(SpawnPoints.VoidStart);
        jungleLevel.SetActive(false);

        StartCoroutine(AudioManager.Instance.ChangeSong(AudioManager.Song.VoidTheme, 2));

        GameplaySetInputEnabled(true);

        yield return StartCoroutine(UIManager.Instance.ControlFade(false, 2));

        yield return null;
    }

    private IEnumerator LoadMainMenu()
    {
        tutorialChamber.SetActive(true);
        tutorialToOnePass.SetActive(true);
        tutorialBlocker.SetActive(false);
        levelOneChamber.SetActive(true);
        OneToTwoPass.SetActive(true);
        levelOneBlocker.SetActive(false);
        levelTwoChamber.SetActive(true);
        jungleLevel.SetActive(false);
        voidLevel.SetActive(false);

        PlayerController.Instance.SetSpawnPoint(SpawnPoints.TutorialStart);
        PlayerController.Instance.SpawnPlayer(SpawnPoints.TutorialStart);
        playerObj.SetActive(false);

        yield return null;
    }

    private void GameplaySetInputEnabled(bool on)
    {
        if (on)
        {
            InputManager.Instance.SetInputActionMap(InputManager.InputMapType.Gameplay);
        }
        else
        {
            InputManager.Instance.SetInputActionMap(InputManager.InputMapType.Disabled);
        }
    }
}

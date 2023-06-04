using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; set; }

    [Header("Panel Prefabs")]
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject GameplayPanel;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Settings")]
    [SerializeField] private GameStates initialMenuType = GameStates.MainMenu;
    [SerializeField] private GameObject mainMenuCamera;
    [SerializeField] private GameObject cameraTrack;
    [SerializeField] private GameObject mainMenuCamFollow;
    [SerializeField] private GameObject virtualCamera;
    [SerializeField] private Image FadeImage;

    [Header("Effect Icons")]
    [SerializeField] private GameObject gravityEffectIcon;
    [SerializeField] private GameObject windEffectIcon;
    [SerializeField] private GameObject wind2x;
    [SerializeField] private GameObject wind3x;
    [SerializeField] private GameObject timeEffectIcon;

    public AnimationCurve Curve;
    public float openFadeDuration = 1f;
    public Color openFadeColor = Color.white;
    public float gameStartFadeDuration = 1f;
    public Color gameStartFadeColor = Color.black;

    private Dictionary<GameStates, GameObject> prefabDictionary = new Dictionary<GameStates, GameObject>();
    private GameState state;

    private bool gameplayStarted = false;
    public void ControlGameplayPanel(bool enabled)
    {
        if (enabled)
        {
            if (!gameplayStarted)
            {
                StartCoroutine(HandleGameStart());
                gameplayStarted = true;
            }
            else
            {
                GameplayPanel.SetActive(true);
                GameManager.Instance.UpdateGameState(GameStates.Gameplay);
            }
        }
        else
        {
            GameplayPanel.SetActive(false);
        }
    }
    public void ControlMainPanel(bool enabled)
    {
        if (enabled)
        {
            if (gameplayStarted)
            {
                StartCoroutine(HandleGameplayQuit());
                gameplayStarted = false;
            }
            else
            {
                CanvasGroup group = MainMenuPanel.GetComponent<CanvasGroup>();
                group.interactable = true;
                group.alpha = 1;
                MainMenuPanel.SetActive(true);

                GameManager.Instance.UpdateGameState(GameStates.MainMenu);
            }
        }
        else
        {
            MainMenuPanel.SetActive(false);
        }
    }

    public void ControlPausePanel(bool enabled)
    {
        if (enabled)
        {
            PausePanel.SetActive(true);
            GameManager.Instance.UpdateGameState(GameStates.PauseMenu);
        }
        else
        {
            PausePanel.SetActive(false);
        }
    }

    public void ControlSettingsPanel(bool enabled)
    {
        if (enabled)
        {
            SettingsPanel.SetActive(true);
            GameManager.Instance.UpdateGameState(GameStates.SettingsMenu);
        }
        else
        {
            if (state.lastState == GameStates.MainMenu)
            {
                ControlMainPanel(true);
            }
            else if (state.lastState == GameStates.PauseMenu)
            {
                ControlPausePanel(true);
            }
            SettingsPanel.SetActive(false);

        }
    }

    public void QuitGame()
    {
        // TODO: Game exit logic should probably go in a game manager

        Debug.Log("Game Quit."); // To show quit in editor
        Application.Quit();
    }

    private IEnumerator HandleGameStart()
    {
        StartCoroutine(FadeCanvasGroupToClear(MainMenuPanel.GetComponent<CanvasGroup>(), gameStartFadeDuration / 2));
        SetFadeColor(gameStartFadeColor);

        StartCoroutine(AudioManager.Instance.ChangeSong(AudioManager.Song.CaveTheme, gameStartFadeDuration * 2));
        yield return StartCoroutine(ControlFade(true, gameStartFadeDuration));

        MainMenuPanel.SetActive(false);
        GameplayPanel.SetActive(true);

        mainMenuCamera.SetActive(false);
        mainMenuCamFollow.SetActive(false);
        virtualCamera.SetActive(false);
        cameraTrack.SetActive(false);

        GameManager.Instance.UpdateGameState(GameStates.Gameplay);
        yield return StartCoroutine(ControlFade(false, gameStartFadeDuration));
    }

    private IEnumerator HandleGameplayQuit()
    {
        GameManager.Instance.UpdateGameState(GameStates.MainMenu);

        StartCoroutine(AudioManager.Instance.ChangeSong(AudioManager.Song.MenuTheme, gameStartFadeDuration * 2));
        yield return StartCoroutine(ControlFade(true, gameStartFadeDuration));

        mainMenuCamera.SetActive(true);
        mainMenuCamFollow.SetActive(true);
        virtualCamera.SetActive(true);
        cameraTrack.SetActive(true);
        MainMenuPanel.SetActive(true);

        yield return StartCoroutine(ControlFade(false, gameStartFadeDuration));

        yield return StartCoroutine(FadeCanvasGroupToVisible(MainMenuPanel.GetComponent<CanvasGroup>(), gameStartFadeDuration));

    }

    public void SetFadeColor(Color color)
    {
        FadeImage.color = new Color(color.r, color.g, color.b, FadeImage.color.a); // Preserve alpha
    }

    public IEnumerator ControlFade(bool on, float duration)
    {
        float elapsedTime = 0f;
        float startAlpha = FadeImage.color.a;
        float finalAlpha;

        if (on)
        {
            finalAlpha = 1;
            FadeImage.raycastTarget = true;
        }
        else
        {
            finalAlpha = 0;
            FadeImage.raycastTarget = false;
        }

        while (elapsedTime < duration)
        {
            float t = Curve.Evaluate(elapsedTime / duration);
            FadeImage.color = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, Mathf.Lerp(startAlpha, finalAlpha, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FadeCanvasGroupToVisible(CanvasGroup canvasGroup, float duration)
    {
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float elapsed = Time.time - startTime;
            canvasGroup.alpha = (elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 1;

        canvasGroup.interactable = true;
    }

    IEnumerator FadeCanvasGroupToClear(CanvasGroup canvasGroup, float duration)
    {
        canvasGroup.interactable = false;

        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float elapsed = Time.time - startTime;
            canvasGroup.alpha = 1f - (elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 0;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Cache UI panels
        prefabDictionary[GameStates.MainMenu] = MainMenuPanel;
        prefabDictionary[GameStates.SettingsMenu] = SettingsPanel;
        prefabDictionary[GameStates.Gameplay] = GameplayPanel;
        prefabDictionary[GameStates.PauseMenu] = PausePanel;

        // Make sure only initial panel is active
        foreach (var panel in prefabDictionary)
        {
            if (panel.Key == initialMenuType) panel.Value.SetActive(true);
            else panel.Value.SetActive(false);
        }

        state = GameManager.GetState();

        ControlWindEffectIcon(false, 1);
        ControlGravityEffectIcon(false);
        ControlTimeEffectIcon(false);

        SetFadeColor(openFadeColor);

        StartCoroutine(StartSequence(false)); // true to skip
    }

    private IEnumerator StartSequence(bool skip = false)
    {
        if (skip)
        {
            creditsPanel.SetActive(false);
            yield return StartCoroutine(FadeCanvasGroupToVisible(MainMenuPanel.GetComponent<CanvasGroup>(), 0.01f));
            yield return StartCoroutine(ControlFade(false, 0.01f));
        }
        else
        {
            yield return new WaitForSeconds(1);
            yield return StartCoroutine(FadeCanvasGroupToVisible(creditsPanel.GetComponent<CanvasGroup>(), 2));
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(FadeCanvasGroupToClear(creditsPanel.GetComponent<CanvasGroup>(), 1.5f));
            creditsPanel.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(ControlFade(false, openFadeDuration));

            StartCoroutine(FadeCanvasGroupToVisible(MainMenuPanel.GetComponent<CanvasGroup>(), 1.5f));
        }

        GameManager.Instance.UpdateGameState(initialMenuType);

        yield return null;
    }

    public void ControlGravityEffectIcon(bool on)
    {
        if (on)
            gravityEffectIcon.SetActive(true);
        else
            gravityEffectIcon.SetActive(false);

    }

    public void ControlWindEffectIcon(bool on, int activeWindEffects)
    {
        if (on)
        {
            switch (activeWindEffects)
            {
                case (0):
                    windEffectIcon.SetActive(true);
                    break;
                case (1):
                    wind2x.SetActive(true);
                    break;
                case (2):
                    wind2x.SetActive(false);
                    wind3x.SetActive(true);
                    break;
            };
        }
        else
        {
            switch (activeWindEffects)
            {
                case (1):
                    windEffectIcon.SetActive(false);
                    break;
                case (2):
                    wind2x.SetActive(false);
                    break;
                case (3):
                    wind3x.SetActive(false);
                    wind2x.SetActive(true);
                    break;
            };
        }

    }

    public void ControlTimeEffectIcon(bool on)
    {
        if (on)
            timeEffectIcon.SetActive(true);
        else
            timeEffectIcon.SetActive(false);

    }
}

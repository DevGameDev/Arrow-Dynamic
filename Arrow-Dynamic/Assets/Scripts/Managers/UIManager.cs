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
            CanvasGroup group = MainMenuPanel.GetComponent<CanvasGroup>();

            group.interactable = true;
            group.alpha = 1;
            MainMenuPanel.SetActive(true);
            GameManager.Instance.UpdateGameState(GameStates.MainMenu);
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
                GameManager.Instance.UpdateGameState(GameStates.MainMenu);
            }
            else if (state.lastState == GameStates.PauseMenu)
            {
                ControlPausePanel(true);
                GameManager.Instance.UpdateGameState(GameStates.PauseMenu);
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

    private IEnumerator MoveCameraToPosition(Transform cameraTransform, Vector3 newPosition, Quaternion newRotation)
    {
        float elapsedTime = 0f;
        Vector3 startingPos = cameraTransform.position;
        Quaternion startingRot = cameraTransform.rotation;

        while (elapsedTime < openFadeDuration * 2) // x2 for fade in and out
        {
            float t = Curve.Evaluate(elapsedTime / openFadeDuration);
            cameraTransform.position = Vector3.Lerp(startingPos, newPosition, t);
            cameraTransform.rotation = Quaternion.Slerp(startingRot, newRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraTransform.position = newPosition;
        cameraTransform.rotation = newRotation;
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

        while (elapsedTime < openFadeDuration)
        {
            float t = Curve.Evaluate(elapsedTime / openFadeDuration);
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
}

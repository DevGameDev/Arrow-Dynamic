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

    [Header("Settings")]
    [SerializeField] private GameStates initialMenuType = GameStates.MainMenu;
    [SerializeField] private GameObject OtherCamera;
    [SerializeField] private GameObject PlayerCamera;
    [SerializeField] private Image FadeImage;
    public AnimationCurve Curve;
    public float Duration = 1f;

    private Dictionary<GameStates, GameObject> prefabDictionary = new Dictionary<GameStates, GameObject>();
    private GameState state;

    public void ControlGameplayPanel(bool enabled)
    {
        if (enabled)
        {
            StartCoroutine(HandleGameStart());
        }
        else
        {
            GameplayPanel.SetActive(false);
            state.lastState = GameStates.Gameplay;
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
            state.currentState = GameStates.MainMenu;
            GameManager.Instance.UpdateGameForNewState();
        }
        else
        {
            MainMenuPanel.SetActive(false);
            state.lastState = GameStates.MainMenu;
        }
    }

    public void ControlPausePanel(bool enabled)
    {
        if (enabled)
        {
            PausePanel.SetActive(true);
            state.currentState = GameStates.PauseMenu;
            GameManager.Instance.UpdateGameForNewState();
        }
        else
        {
            PausePanel.SetActive(false);
            state.lastState = GameStates.PauseMenu;
        }
    }

    public void ControlSettingsPanel(bool enabled)
    {
        if (enabled)
        {
            SettingsPanel.SetActive(true);
            state.currentState = GameStates.SettingsMenu;
            GameManager.Instance.UpdateGameForNewState();
        }
        else
        {
            prefabDictionary[state.lastState].SetActive(true); // Reopen last panel
            state.currentState = state.lastState;
            state.lastState = GameStates.SettingsMenu;
            GameManager.Instance.UpdateGameForNewState();
            SettingsPanel.SetActive(false);
        }
    }

    public void QuitGame()
    {
        // TODO: Game exit logic should probably go in a game manager

        Debug.Log("Game Quit."); // To show quit in editor
        Application.Quit();
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
        state.currentState = initialMenuType; // Set current state

        StartCoroutine(FadeToClear());
        GameManager.Instance.UpdateGameForNewState();
    }

    private IEnumerator HandleGameStart()
    {
        StartCoroutine(MoveCameraToPosition(OtherCamera.transform, PlayerCamera.transform.position, PlayerCamera.transform.rotation));
        StartCoroutine(FadeCanvasGroupToClear(MainMenuPanel.GetComponent<CanvasGroup>()));
        yield return StartCoroutine(FadeToBlack());

        MainMenuPanel.SetActive(false);
        GameplayPanel.SetActive(true);
        state.currentState = GameStates.Gameplay;

        GameManager.Instance.UpdateGameForNewState();

        OtherCamera.SetActive(false);
        yield return StartCoroutine(FadeToClear());
    }

    private IEnumerator MoveCameraToPosition(Transform cameraTransform, Vector3 newPosition, Quaternion newRotation)
    {
        float elapsedTime = 0f;
        Vector3 startingPos = cameraTransform.position;
        Quaternion startingRot = cameraTransform.rotation;

        while (elapsedTime < Duration)
        {
            float t = Curve.Evaluate(elapsedTime / Duration);
            cameraTransform.position = Vector3.Lerp(startingPos, newPosition, t);
            cameraTransform.rotation = Quaternion.Slerp(startingRot, newRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraTransform.position = newPosition;
        cameraTransform.rotation = newRotation;
    }

    private IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;

        FadeImage.raycastTarget = false;

        while (elapsedTime < Duration)
        {
            float t = Curve.Evaluate(elapsedTime / Duration);
            FadeImage.color = new Color(0f, 0f, 0f, Mathf.Lerp(0f, 1f, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // FadeImage.color = new Color(0f, 0f, 0f, 1f);
        FadeImage.gameObject.SetActive(false);
    }

    private IEnumerator FadeToClear()
    {
        FadeImage.gameObject.SetActive(true);

        float elapsedTime = 0f;

        while (elapsedTime < Duration)
        {
            float t = Curve.Evaluate(elapsedTime / Duration);
            FadeImage.color = new Color(0f, 0f, 0f, Mathf.Lerp(1f, 0f, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        FadeImage.color = new Color(0f, 0f, 0f, 0f);
        FadeImage.raycastTarget = false;
    }

    IEnumerator FadeCanvasGroupToVisible(CanvasGroup canvasGroup)
    {

        float startTime = Time.time;

        while (Time.time < startTime + Duration)
        {
            float elapsed = Time.time - startTime;
            canvasGroup.alpha = (elapsed / Duration);
            yield return null;
        }

        canvasGroup.alpha = 1;

        canvasGroup.interactable = true;
    }

    IEnumerator FadeCanvasGroupToClear(CanvasGroup canvasGroup)
    {
        canvasGroup.interactable = false;

        float startTime = Time.time;

        while (Time.time < startTime + Duration)
        {
            float elapsed = Time.time - startTime;
            canvasGroup.alpha = 1f - (elapsed / Duration);
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
}
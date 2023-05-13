using UnityEngine;

[CreateAssetMenu(fileName = "State", order = 1)]
public class GameState : ScriptableObject
{
    public GameStates currentState = GameStates.Initial;
    public GameStates lastState = GameStates.Initial;

    [Range(0, 1000)] public float timeScale = 1;
}

/// <summary>
/// An enumeration of possible game state.
/// </summary>
public enum GameStates
{
    Initial,
    Loading,
    MainMenu,
    SettingsMenu,
    Gameplay,
    Paused,
}
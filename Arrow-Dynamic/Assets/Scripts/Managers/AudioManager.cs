using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages audio in the game, including music and sound effects.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } // Singleton

    private GameState state;
    private GameSettings settings;

    void Awake()
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
        state = GameManager.GetState();
        settings = GameManager.GetSettings();
    }
}

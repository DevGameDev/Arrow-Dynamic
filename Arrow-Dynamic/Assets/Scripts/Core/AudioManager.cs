using UnityEngine;

/// <summary>
/// Manages audio in the game, including music and sound effects.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } // Singleton

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
    // Play a specific sound effect or music track
    public void PlayAudio(string audioName)
    {
        // Implement audio playback logic here
    }
}
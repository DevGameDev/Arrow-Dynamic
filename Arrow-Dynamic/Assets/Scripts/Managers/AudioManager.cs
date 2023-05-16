using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages audio in the game, including music and sound effects.
/// </summary>
public class AudioManager : MonoBehaviour
{
    //////////////////////////////////////////////////
    // Public Properties and Methods
    //////////////////////////////////////////////////
    public static AudioManager Instance { get; private set; } // Singleton

    //////////////////////////////////////////////////
    // Private Fields and Methods
    //////////////////////////////////////////////////
    [SerializeField] private Slider audioSlider;
    private float audioVolume = 1f; // The current audio volume

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
    void Start()
    {
        // Set the initial volume to the value of the audio slider
        audioVolume = audioSlider.value;
        UpdateAudioVolume();
    }
    public void OnAudioSliderChanged()
    {
        // Called when the audio slider value changes
        audioVolume = audioSlider.value;
        UpdateAudioVolume();
    }
    private void UpdateAudioVolume()
    {
        // Update the audio volume for all relevant audio sources in the game
        // Replace this code with your actual audio source management logic
        AudioListener.volume = audioVolume;
    }

}

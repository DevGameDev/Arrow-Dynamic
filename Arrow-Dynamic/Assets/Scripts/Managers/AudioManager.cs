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

    //[SerializeField] private Slider audioSlider;
    private float audioVolume = 1f; // The current audio volume

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public void ChangeMasterVolume(float value){
        AudioListener.volume = value;
    }
}


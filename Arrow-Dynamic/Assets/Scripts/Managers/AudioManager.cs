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
}

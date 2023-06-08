using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //[SerializeField] Slider volumeSlider;
    [Serializable]
    public class SongClip
    {
        public Song song;
        public AudioClip clip;
    }

    [Serializable]
    public class SFXClip
    {
        public SoundEffect soundEffect;
        public AudioClip clip;
    }

    public enum SoundEffect
    {
        RockImpact,
        RockFall,
        Explosion,
        Jump,
        PowerUp,
        BowDraw,
        ArrowHit,
        Death,
        Teleport,
    }

    public enum Song
    {
        MenuTheme,
        CaveTheme,
        JungleTheme,
        VoidTheme
    }

    public static AudioManager Instance { get; private set; } // Singleton

    [SerializeField] private AudioSource musicSource; // A source for the music
    [SerializeField] private AudioSource sfxSource; // A source for sound effects
    [SerializeField] private SongClip[] songs; // Songs
    [SerializeField] private SFXClip[] soundEffects; // Sound effects
    private Dictionary<Song, AudioClip> songDict = new Dictionary<Song, AudioClip>();
    private Dictionary<SoundEffect, AudioClip> sfxDict = new Dictionary<SoundEffect, AudioClip>();
    private float audioVolume = 1f; // The current audio volume
    private float musicVolume = 1f; // The current music volume
    private float sfxVolume = 1f; // The current sfx volume

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Cache songs
            foreach (SongClip songClip in songs)
            {
                songDict[songClip.song] = songClip.clip;
            }

            // Cache sound effects
            foreach (SFXClip sfxClip in soundEffects)
            {
                sfxDict[sfxClip.soundEffect] = sfxClip.clip;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }
    public void ChangeSFXVolume(float value)
    {
        sfxVolume = value;
    }
    public void ChangeMusicVolume(float value)
    {
        musicVolume = value;
    }


    // Start a song
    public void StartSong(Song song)
    {
        if (songDict.ContainsKey(song))
        {
            musicSource.clip = songDict[song];
            musicSource.Play();
        }
    }

    // Switch between songs with a smooth transition
    public IEnumerator ChangeSong(Song newSong, float duration)
    {
        float originalVolume = musicSource.volume;

        // Fade out current song
        for (float t = 0.0f; t < duration; t += Time.deltaTime)
        {
            musicSource.volume = (duration - t) / duration * originalVolume;
            yield return null;
        }

        // Switch song and fade in
        if (songDict.ContainsKey(newSong))
        {
            musicSource.clip = songDict[newSong];
            musicSource.Play();

            for (float t = 0.0f; t < duration; t += Time.deltaTime)
            {
                musicSource.volume = t / duration * originalVolume;
                yield return null;
            }

            musicSource.volume = originalVolume;
        }
    }

    // Play sound effects
    public void PlaySFX(SoundEffect sfx, float adjust)
    {
        if (sfxDict.ContainsKey(sfx))
        {
            sfxSource.PlayOneShot(sfxDict[sfx], audioVolume * sfxVolume * adjust);
        }
    }
}
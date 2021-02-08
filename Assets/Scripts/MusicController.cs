using System;
using System.Collections;
using System.Collections.Generic;
using Static;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance;

    private static AudioSource _audioSource;
    private static AudioClip _currentAudioClip;

    // Start is called before the first frame update
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
        Instance = this;
    }

    void Start()
    {
        AudioFunctions.TryGetVolumeGameOption(AudioFunctions.AudioTypes.Music, out var gameOption);
        _audioSource.volume = Convert.ToSingle(gameOption.value) / 100f;
        gameOption.ValueChanged += value => _audioSource.volume = Convert.ToSingle(value) / 100f;
    }

    public static void Play(AudioClip musicAudioClip)
    {
        if (musicAudioClip == _currentAudioClip) return;
        _currentAudioClip = musicAudioClip;
        _audioSource.Stop();
        _audioSource.clip = musicAudioClip;
        _audioSource.Play();
    }

    public static void Stop()
    {
        _audioSource.Stop();
    }
}

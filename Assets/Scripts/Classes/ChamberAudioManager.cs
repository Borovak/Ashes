using System;
using UnityEngine;

public class ChamberAudioManager
{    
    private GameController _gameController;
    private AudioSource _audioSourceAmbient;
    private AudioClip _musicAudioClip;

    public ChamberAudioManager(ChamberController chamberController, AudioClip ambientAudioClip, AudioClip musicAudioClip)
    {
        _musicAudioClip = musicAudioClip;
        //Set up audio source
        _audioSourceAmbient = chamberController.gameObject.AddComponent<AudioSource>();
        _audioSourceAmbient.loop = true;
        _audioSourceAmbient.playOnAwake = false;
        _audioSourceAmbient.clip = ambientAudioClip;
        //Linking it to volume parameter
        if (GameOptionsManager.TryGetOption(GameOptionsManager.OPTION_AMBIENT_VOLUME, out var gameOptionAmbientVolume))
        {
            _audioSourceAmbient.volume = Convert.ToSingle(gameOptionAmbientVolume.value) / 100f;
            gameOptionAmbientVolume.ValueChanged += (value) => _audioSourceAmbient.volume = Convert.ToSingle(value) / 100f;
        }
    }

    public void Start()
    {
        MusicController.Play(_musicAudioClip);
        if (_audioSourceAmbient.clip != null)
        {
            _audioSourceAmbient.Play();
        }
    }

    public void Stop()
    {        
        _audioSourceAmbient.Stop();
    }
}
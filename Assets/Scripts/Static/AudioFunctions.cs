using System;
using System.Collections.Generic;
using UnityEngine;

public static class AudioFunctions
{
    public enum AudioTypes
    {
        Music,
        Ambient,
        Sfx
    }

    public static void PlaySound(AudioClip audioClip)
    {
        if (audioClip == null) return;
        LeanAudio.play(audioClip, GetVolume(AudioTypes.Sfx));
    }

    public static void PlaySound(AudioClip audioClip, Vector3 position)
    {
        if (audioClip == null) return;
        var audioSource = LeanAudio.playClipAt(audioClip, position);
        audioSource.volume = GetVolume(AudioTypes.Sfx);
    }

    public static void PlayRandomSound(AudioClip[] audioClips, Vector3 position)
    {
        if (audioClips.Length == 0) return;
        var index = UnityEngine.Random.Range(0, audioClips.Length);
        PlaySound(audioClips[index], position);
    }

    public static float GetVolume(AudioTypes audioType)
    {
        if (!TryGetVolumeGameOption(audioType, out var gameOption)) return 1f;
        return Convert.ToSingle(gameOption.value) / 100f;
    }

    public static bool TryGetVolumeGameOption(AudioTypes audioType, out GameOption gameOption)
    {
        var optionName = new Dictionary<AudioTypes, string> {
            {AudioTypes.Music, GameOptionsManager.OPTION_MUSIC_VOLUME},
            {AudioTypes.Ambient, GameOptionsManager.OPTION_AMBIENT_VOLUME},
            {AudioTypes.Sfx, GameOptionsManager.OPTION_SFX_VOLUME},
        }[audioType];
        return GameOptionsManager.TryGetOption(optionName, out gameOption);
    }
}
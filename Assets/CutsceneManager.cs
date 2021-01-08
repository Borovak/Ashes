using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    public enum Cutscenes
    {
        Intro
    }
    public static VideoPlayer videoPlayer;

    private static Animator _animator;
    private static Camera _targetCamera;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _targetCamera = GameObject.FindGameObjectWithTag("SubCamera").GetComponent<Camera>();
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
        videoPlayer.playOnAwake = false;
    }

    public static void Play(Cutscenes cutscene)
    {
        if (!TryGetVideoClip(cutscene, out var videoClip)) return;
        videoPlayer.clip = videoClip;
        videoPlayer.targetCamera = _targetCamera;
        _animator.SetBool("Cutscene", true);
        FadeInOutController.FadeIn();
    }

    private static bool TryGetVideoClip(Cutscenes cutscene, out VideoClip videoClip)
    {
        var videoClips = new Dictionary<Cutscenes, string>
        {
            {Cutscenes.Intro, "IntroTest"}
        };
        if (!videoClips.TryGetValue(cutscene, out var path))
        {
            videoClip = null;
            return false;
        }
        videoClip = Resources.Load<VideoClip>($"Cutscenes/{path}");
        return videoClip != null;
    }
}

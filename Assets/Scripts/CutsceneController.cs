using System;
using System.Collections;
using System.Collections.Generic;
using Classes;
using UI;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneController : MonoBehaviour
{
    public enum Cutscenes
    {
        Intro
    }

    public GameObject skipObject;
    
    private static Cutscenes _cutscene;
    private static Action _actionOnEnd;
    private VideoPlayer _videoPlayer;
    private bool _cutsceneDone;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!TryGetVideoClip(_cutscene, out var videoClip))
        {
            CloseCutscene();
            return;
        }
        _cutsceneDone = false;
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.clip = videoClip;
        _videoPlayer.targetCamera = GameObject.FindGameObjectWithTag("SubCamera").GetComponent<Camera>();
        _videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
        _videoPlayer.Play();
        FadeInOutController.FadeIn();
    }

    void OnEnable()
    {
        ControllerInputs.controllerButtons[Constants.ControllerButtons.A].Filled += CloseCutscene;
    }

    void OnDisable()
    {
        ControllerInputs.controllerButtons[Constants.ControllerButtons.A].Filled -= CloseCutscene;
    }

    // Update is called once per frame
    void Update()
    {
        if (_cutsceneDone || _videoPlayer.time < _videoPlayer.clip.length - 1f) return;
        CloseCutscene();
    }

    private void CloseCutscene()
    {
        _cutsceneDone = true;
        Destroy(skipObject);
        FadeInOutController.FadeOut(() =>
        {
            MenuController.ReturnToGame();
            _actionOnEnd?.Invoke();
        });
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

    public static void Init(Cutscenes cutscene, Action actionOnEnd)
    {
        _cutscene = cutscene;
        _actionOnEnd = actionOnEnd;
    }
}

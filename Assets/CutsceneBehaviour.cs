using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneBehaviour : StateMachineBehaviour
{
    public UnityEngine.Video.VideoClip videoClip;
    private UnityEngine.Video.VideoPlayer _videoPlayer;

    private Animator _animator;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _animator = animator;
        if (_videoPlayer == null)
        {
            _videoPlayer = animator.gameObject.AddComponent<VideoPlayer>();
            _videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
        }
        _videoPlayer.targetCamera = GameObject.FindGameObjectWithTag("SubCamera").GetComponent<Camera>();
        _videoPlayer.playOnAwake = false;
        _videoPlayer.loopPointReached += CancelAnimation;
        _videoPlayer.clip = videoClip;
        _videoPlayer.Play();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _videoPlayer.targetCamera = null;
        _videoPlayer.loopPointReached -= CancelAnimation;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    private void CancelAnimation(VideoPlayer videoPlayer)
    {
        _animator.SetBool("Cutscene", false);
    }
}

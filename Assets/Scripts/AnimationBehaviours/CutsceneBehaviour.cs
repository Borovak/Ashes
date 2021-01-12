using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneBehaviour : StateMachineBehaviour
{
    public bool fadeOutDone;
    private VideoPlayer _videoPlayer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        fadeOutDone = false;
        _videoPlayer = animator.GetComponent<VideoPlayer>();
        _videoPlayer.enabled = true;
        _videoPlayer.Play();
        _videoPlayer.loopPointReached += OnEnd;
        GameController.gameState = GameController.GameStates.Cutscene;
        FadeInOutController.FadeIn();
    }

    private void OnEnd(VideoPlayer videoPlayer)
    {
        videoPlayer.loopPointReached -= OnEnd;
        videoPlayer.Stop();
        videoPlayer.targetCamera = null;
        videoPlayer.enabled = false;        
        videoPlayer.GetComponent<Animator>().SetBool("Cutscene", false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (fadeOutDone || _videoPlayer.time < _videoPlayer.clip.length - 1f) return;
       fadeOutDone = true;
       FadeInOutController.FadeOut(null);
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CutsceneManager.playbackEndAction?.Invoke();
        CutsceneManager.playbackEndAction = null;
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
}

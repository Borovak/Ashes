using UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace AnimationBehaviours
{
    public class CutsceneBehaviour : StateMachineBehaviour
    {
        private bool _cutsceneDone;
        private bool _skip;
        private CutsceneSkipController _cutsceneSkipController;
        private VideoPlayer _videoPlayer;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _cutsceneSkipController = GameObject.FindGameObjectWithTag("CutsceneSkipControl").GetComponent<CutsceneSkipController>();
            _cutsceneSkipController.Reset();
            _cutsceneDone = false;
            _skip = false;
            _videoPlayer = animator.GetComponent<VideoPlayer>();
            _videoPlayer.enabled = true;
            _videoPlayer.Play();
            _videoPlayer.loopPointReached += OnEnd;
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
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_cutsceneDone) return;
            //Skip
            if (MenuInputs.okButtonPressedTimer > CutsceneSkipController.SkipDelay)
            {
                _cutsceneDone = true;
                _skip = true;
                _videoPlayer.time = _videoPlayer.clip.length - 0.9f;
            }
            //End of clip
            if (_skip || _videoPlayer.time < _videoPlayer.clip.length - 1f) return;
            _cutsceneDone = true;
            FadeInOutController.FadeOut(null);
        }

        //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
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
}

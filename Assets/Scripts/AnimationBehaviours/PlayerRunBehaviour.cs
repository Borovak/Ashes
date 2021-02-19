using Static;
using UnityEngine;

namespace AnimationBehaviours
{
    public class PlayerRunBehaviour : StateMachineBehaviour
    {
        public float stepRate = 1f;
        public AudioClip[] stepSounds;

        private float _countdown;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        // override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        // {
        // }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stepSounds.Length == 0) return;
            if (_countdown > 0)
            {
                _countdown -= Time.deltaTime;
                return;
            }
            _countdown = 1f / stepRate;
            AudioFunctions.PlayRandomSound(stepSounds, animator.transform.position);
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

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

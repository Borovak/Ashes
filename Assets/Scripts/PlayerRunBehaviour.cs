using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunBehaviour : StateMachineBehaviour
{
    public float stepRate = 1f;
    public AudioClip[] stepSounds;

    private AudioSource _audioSource;
    private float countdown;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _audioSource = animator.GetComponent<AudioSource>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stepSounds.Length == 0) return;
        if (countdown > 0) {
            countdown -= Time.deltaTime;
            return;
        }
        countdown =  1f / stepRate;
        var index = UnityEngine.Random.Range(0, stepSounds.Length);
        _audioSource.PlayOneShot(stepSounds[index]);
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

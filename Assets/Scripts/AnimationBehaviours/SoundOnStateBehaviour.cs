using UnityEngine;

public class SoundOnStateBehaviour : StateMachineBehaviour
{
    public enum Positions
    {
        Start,
        Update,
        Exit
    }
    public AudioClip sound;
    public Positions position;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckIfPlayNeeded(Positions.Start);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckIfPlayNeeded(Positions.Update);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckIfPlayNeeded(Positions.Exit);
    }

    private void CheckIfPlayNeeded(Positions currentPosition)
    {
        if (position != currentPosition || sound == null) return;
        LeanAudio.play(sound);
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

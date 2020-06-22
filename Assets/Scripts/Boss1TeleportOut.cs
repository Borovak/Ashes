using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1TeleportOut : StateMachineBehaviour
{

    private Vector2 _teleportLocation;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       _teleportLocation = animator.GetComponent<Boss1Controller>().GetTeleportLocation();
       animator.transform.Find("Portal").gameObject.SetActive(true);       
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       var newPosition = animator.transform.localPosition;
       newPosition.x = _teleportLocation.x;
       newPosition.y = _teleportLocation.y;
       animator.transform.localPosition = newPosition;
       animator.transform.Find("Portal").gameObject.SetActive(false);      
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

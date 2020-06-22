using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1IdleWait : StateMachineBehaviour
{
    private static bool _state;
    private float _waitTimeRemaining;
    private SpriteRenderer _mainSpriteRenderer;
    private Transform playerTransform;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _mainSpriteRenderer = animator.transform.Find("MainSprite").GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        var bossController = animator.GetComponent<Boss1Controller>();
        animator.SetTrigger(_state ? "attack" : "teleport");
        _waitTimeRemaining = _state ? bossController.waitBeforeAttack : bossController.waitAfterAttack;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _mainSpriteRenderer.flipX = playerTransform.position.x > animator.transform.position.x;
        _waitTimeRemaining -= Time.deltaTime;
        if (_waitTimeRemaining <= 0)
        {
            animator.SetBool("waitOver", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _state = !_state;
        animator.SetBool("teleport", false);
        animator.SetBool("attack", false);
        animator.SetBool("waitOver", false);
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

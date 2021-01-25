using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossWolfIdleBehaviour : StateMachineBehaviour
{
    private Transform _playerTransform;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var spriteRenderer = animator.GetComponent<SpriteRenderer>();
        var bossWolfController = animator.GetComponent<BossWolfController>();
        if (_playerTransform == null)
        {
            var playerGameObject = GameObject.FindGameObjectWithTag("Player");
            if (playerGameObject == null) return;
            _playerTransform = playerGameObject.transform;
        }
        var distance = Vector3.Distance(_playerTransform.position, animator.transform.position);
        string animationName;
        Transform waypoint = null;
        if (distance > 3f)
        {            
            var availableWaypoints = bossWolfController.waypoints.Where(x => x != bossWolfController.currentWaypoint);
            var playerDistanceToClosestWaypoint = availableWaypoints.Min(x => Vector3.Distance(x.position, _playerTransform.position));
            var closestWaypointToPlayer = availableWaypoints.First(x => Vector3.Distance(x.position, _playerTransform.position) == playerDistanceToClosestWaypoint);
            waypoint = closestWaypointToPlayer;
            animationName = bossWolfController.currentWaypoint.position.y != waypoint.position.y ? "jump" : "run";      
            spriteRenderer.flipX = (waypoint.position - animator.transform.position).x < 0f;
        }
        else
        {
            var closeQuarterOptions = new[] { "attack", "attackCombo", "block" };
            animationName = closeQuarterOptions[UnityEngine.Random.Range(0, closeQuarterOptions.Length)];
            spriteRenderer.flipX = (_playerTransform.position - animator.transform.position).x < 0f;
        }
        bossWolfController.currentWaypoint = waypoint != null ? waypoint : bossWolfController.currentWaypoint;
        animator.SetTrigger(animationName);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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

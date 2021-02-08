using UnityEngine;

namespace AnimationBehaviours
{
    public class BossWolfMoveBehaviour : StateMachineBehaviour
    {
        public float metersPerAnim = 5f;
        private BossWolfController _bossWolfController;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _bossWolfController = animator.GetComponent<BossWolfController>();
            if (stateInfo.IsName("Jump"))
            {
                LeanTween.Framework.LeanTween.move(_bossWolfController.gameObject, _bossWolfController.currentDestination, animator.GetCurrentAnimatorStateInfo(0).length);
            }
            else
            {
                var time = Vector3.Distance(_bossWolfController.gameObject.transform.position, _bossWolfController.currentDestination) / metersPerAnim;
                LeanTween.Framework.LeanTween.move(_bossWolfController.gameObject, _bossWolfController.currentDestination, time).setOnComplete(() => animator.SetTrigger("arrived"));
            }
            //Debug.Log($"Boss going from {_bossWolfController.transform.position.x} to {_bossWolfController.currentWaypoint.position.x} in {animator.GetCurrentAnimatorStateInfo(0).length} seconds");
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        // override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        // {
        // }

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

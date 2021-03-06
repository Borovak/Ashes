using UnityEngine;

namespace AnimationBehaviours
{
    public class BossWolfDecisionMakingBehaviour : StateMachineBehaviour
    {
        private Transform _playerTransform;
    
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
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
            if (distance > 3f)
            {            
                animationName = "run";     
                bossWolfController.currentDestination = _playerTransform.position;
            }
            else
            {
                var closeQuarterOptions = new[] { "attack", "attackCombo", "block" };
                animationName = closeQuarterOptions[UnityEngine.Random.Range(0, closeQuarterOptions.Length)];
                spriteRenderer.flipX = (_playerTransform.position - animator.transform.position).x < 0f;
            }
            spriteRenderer.flipX = (_playerTransform.position - animator.transform.position).x < 0f;
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
}

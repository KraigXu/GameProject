namespace Invector
{
    public class vAnimatorSetInt : vAnimatorSetValue<int>
    {       
        [vHelpBox("Random Value between Default Value and Max Value")]
        public bool randomEnter;
        [vHideInInspector("randomEnter")]
        public int maxEnterValue;
        public bool randomExit;
        [vHideInInspector("randomExit")]
        public int maxExitValue;

        protected override int GetEnterValue()
        {
            return randomEnter ? UnityEngine.Random.Range(base.GetEnterValue(), maxEnterValue) : base.GetEnterValue();
        }
        protected override int GetExitValue()
        {
            return randomExit?UnityEngine.Random.Range( base.GetExitValue(),maxExitValue):base.GetExitValue();
        }
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}
    }
}
using System;
using UnityEngine;
namespace Invector
{
    [Serializable]
    public abstract class vAnimatorSetValue<T> : StateMachineBehaviour where T : IConvertible
    {
        public bool setOnEnter;
        [vHideInInspector("setOnEnter")]
        public T enterValue;
        public bool setOnExit;
        [vHideInInspector("setOnExit")]
        public T exitValue;
        public string animatorParameter = "My Animator Parameter";


        protected virtual T GetEnterValue()
        {           
            return enterValue;           
        }

        protected virtual T GetExitValue()
        {           
            return exitValue;           
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public sealed void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (setOnEnter)
            {
                if (typeof(T).Equals(typeof(int)))
                    animator.SetInteger(animatorParameter, (int)(object)GetEnterValue());
                else if (typeof(T).Equals(typeof(float)))
                    animator.SetFloat(animatorParameter, (float)(object)GetEnterValue());
                else if (typeof(T).Equals(typeof(bool)))
                    animator.SetBool(animatorParameter, (bool)(object)GetEnterValue());
            }
        }

        //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public sealed void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (setOnExit)
            {
                if (typeof(T).Equals(typeof(int)))
                    animator.SetInteger(animatorParameter, (int)(object)GetExitValue());
                else if (typeof(T).Equals(typeof(float)))
                    animator.SetFloat(animatorParameter, (float)(object)GetExitValue());
                else if (typeof(T).Equals(typeof(bool)))
                    animator.SetBool(animatorParameter, (bool)(object)GetExitValue());
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Invector.vEventSystems
{
    public class vAnimatorEvent : StateMachineBehaviour
    {
        [System.Serializable]
        public class vAnimatorEventTrigger
        {
            public string eventName = "New Event";
            public float normalizedTime;
            private int loopCount;
            public event OnTriggerEvent onTriggerEvent;
            public void UpdateEventTrigger(float normalizedTime)
            {
                var normalizedTimeClamped = Mathf.Clamp(normalizedTime, 0, loopCount + 1f);
                if (normalizedTimeClamped >= loopCount + this.normalizedTime)
                {
                    if (onTriggerEvent != null) onTriggerEvent(eventName);
                    loopCount++;
                }
            }
            public void Init()
            {
                loopCount = 0;
            }
        }
        public List<vAnimatorEventTrigger> eventTriggers;

        public delegate void OnTriggerEvent(string eventName);

        public bool HasEvent(string eventName)
        {
            return eventTriggers.Exists(e => e.eventName.Equals(eventName));
        }

        public void RegisterEvents(string eventName, OnTriggerEvent onTriggerEvent)
        {
            var _events = eventTriggers.FindAll(e => e.eventName.Equals(eventName));
            for (int i = 0; i < _events.Count; i++)
            {
                _events[i].onTriggerEvent -= onTriggerEvent;
                _events[i].onTriggerEvent += onTriggerEvent;
            }
        }
        public void RemoveEvents(string eventName, OnTriggerEvent onTriggerEvent)
        {
            var _events = eventTriggers.FindAll(e => e.eventName.Equals(eventName));
            for (int i = 0; i < _events.Count; i++)
            {
                _events[i].onTriggerEvent -= onTriggerEvent;
            }
        }
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for (int i = 0; i < eventTriggers.Count; i++) { eventTriggers[i].Init(); eventTriggers[i].UpdateEventTrigger(stateInfo.normalizedTime); }
        }
        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!stateInfo.loop && stateInfo.normalizedTime > 1) return;
            for (int i = 0; i < eventTriggers.Count; i++) eventTriggers[i].UpdateEventTrigger(stateInfo.normalizedTime);
        }
    }
}

namespace Invector
{
    public static class vAnimatorEventExtencion
    {
        /// <summary>
        /// Add event to the <seealso cref="vAnimatorEvent"/> in animator
        /// </summary>
        /// <param name="animator">target animator</param>
        /// <param name="eventName">event name</param>
        /// <param name="onTriggerEventAction">action to add to <seealso cref="vEventSystems.vAnimatorEvent"/></param>
        public static void RegisterEvent(this Animator animator, string eventName, vEventSystems.vAnimatorEvent.OnTriggerEvent onTriggerEventAction)
        {
            if (animator)
            {
                var behaviours = animator.GetBehaviours<vEventSystems.vAnimatorEvent>();
                for (int i = 0; i < behaviours.Length; i++)
                {
                    if (behaviours[i].HasEvent(eventName))
                    {
                        behaviours[i].RegisterEvents(eventName, onTriggerEventAction);
                    }
                }
            }
        }

        /// <summary>
        /// Remove event of the <seealso cref="vEventSystems.vAnimatorEvent"/> in animator
        /// </summary>
        /// <param name="animator">target animator</param>
        /// <param name="eventName">event name</param>
        /// <param name="onTriggerEventAction">action to remove of the <seealso cref="vAnimatorEvent"/></param>
        public static void RemoveEvent(this Animator animator, string eventName, vEventSystems.vAnimatorEvent.OnTriggerEvent onTriggerEventAction)
        {
            if (animator)
            {
                var behaviours = animator.GetBehaviours<vEventSystems.vAnimatorEvent>();
                for (int i = 0; i < behaviours.Length; i++)
                {
                    if (behaviours[i].HasEvent(eventName))
                    {
                        behaviours[i].RemoveEvents(eventName, onTriggerEventAction);
                    }
                }
            }
        }

    }
}
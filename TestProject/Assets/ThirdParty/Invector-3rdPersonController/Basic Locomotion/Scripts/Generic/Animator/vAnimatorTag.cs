using System.Collections.Generic;
using UnityEngine;

namespace Invector.vEventSystems
{
    public class vAnimatorTag : StateMachineBehaviour
    {
        public delegate void OnStateTrigger(List<string> tags);
        public string[] tags = new string[] { "CustomAction" };
        public List<vAnimatorStateInfos> stateInfos = new List<vAnimatorStateInfos>();       
        public event OnStateTrigger onStateEnter;
        public event OnStateTrigger onStateExit;
        public void AddStateInfoListener(vAnimatorStateInfos stateInfo)
        {
            if (!stateInfos.Contains(stateInfo))
            {
                stateInfos.Add(stateInfo);
            }
        }
        public void RemoveStateInfoListener(vAnimatorStateInfos stateInfo)
        {
            if (stateInfos.Contains(stateInfo))
            {
                stateInfos.Remove(stateInfo);
            }
        }
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
            base.OnStateEnter(animator, stateInfo, layerIndex);
            if (stateInfos != null)
            {
                for (int i = 0; i < tags.Length; i++)
                {
                    for (int a = 0; a < stateInfos.Count; a++)
                    {
                        stateInfos[a].AddStateInfo(tags[i], layerIndex);                      
                    } 
                }
            }
            if (onStateEnter != null)
                onStateEnter(tags.vToList());
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfos != null)
            {
                for (int i = 0; i < tags.Length; i++)
                {
                    for (int a = 0; a < stateInfos.Count; a++)
                        stateInfos[a].RemoveStateInfo(tags[i], layerIndex);                  
                }
            }
            base.OnStateExit(animator, stateInfo, layerIndex);
            if (onStateExit != null)
                onStateExit(tags.vToList());
        }
    }
}


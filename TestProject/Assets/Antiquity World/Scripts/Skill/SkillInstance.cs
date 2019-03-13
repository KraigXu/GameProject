using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Skill
{
    public class SkillInstance : MonoBehaviour
    {
        public SkillGroup CurrentGroup;          //当前行为
       // public float ContinuedTime = 5f;       //持续时间   持续时间应是外界赋值
        public GameObject[] AillTarget = new GameObject[3];   //所有目标  数组长度应是外界赋值
        public string[] ImpactLabel;  //影响标签  凡是元素集内的标签 才会被技能影响
        public Collider ColliderSelf;
        public Transform[] AllTarget;
        [SerializeField]
        public float _currentTime = 0;

        public Vector3 TargetPos;
        public vCharacter Character;
        
        void Start()
        {
            //1裂 2击 诅 3诅咒 4多重 5
        }
        void Update()
        {
            CurrentGroup.UpdateBehaviors(this);
            _currentTime += Time.deltaTime;
            if (_currentTime >= CurrentGroup.ContinuedTime)
            {
                _currentTime = 0;
                CurrentGroup.Reset();
                Destroy(this);
                return;
            }
        }
        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Enemy")
            {
                for (int i = 0; i < AillTarget.Length; i++)
                {
                    if (AillTarget[i] == null)
                    {
                        AillTarget[i] = collider.gameObject;
                        break;
                    }
                }
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.tag == "Enemy")
            {
                for (int i = 0; i < AillTarget.Length; i++)
                {
                    if (AillTarget[i] == collider.gameObject)
                    {
                        AillTarget[i] = null;
                        break;
                    }
                }
            }
        }
    }

}


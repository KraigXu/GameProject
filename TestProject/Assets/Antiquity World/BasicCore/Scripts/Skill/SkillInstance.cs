using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Skill
{
    /// <summary>
    /// 技能实例
    /// </summary>
    public class SkillInstance : MonoBehaviour
    {
        public SkillGroup CurrentGroup;           //当前效果
        public Vector3 EffectPosition;          //作用位置
        public Transform EffectNode;           //作用节点

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
            }
        }
       

    }

}


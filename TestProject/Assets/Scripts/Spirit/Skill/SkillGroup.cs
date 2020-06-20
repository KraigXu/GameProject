using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;

namespace GameSystem.Skill
{
    public enum SkillRunStatus
    {
        NotTriggered,
        TriggeredIng,
        Restoreing,
    }

    public class SkillGroup 
    {
        public int Id;                                                          //编号
        public string Name;                                                     //名称
        public string Description;                                              //说明
        public float ContinuedTime =5;
        public float CoolingTime=3;                                               //冷却
        public Sprite Icon;                                                     //图标
        public Color sceneGizmoColor = Color.gray;                              //拿来渲染eyes的Gizmos颜色
        public List<SkillTrigger> Behaviors=new List<SkillTrigger>();
        
        public bool m_IsUsed = false;

        public SkillRunStatus CurRunStatus= SkillRunStatus.NotTriggered;
        public vCharacter Character;
        private float _continued = 0;


        //public SkillGroup CurrentGroup;           //当前效果
        //public Vector3 EffectPosition;          //作用位置
        //public Transform EffectNode;           //作用节点

        //public GameObject[] AillTarget = new GameObject[3];   //所有目标  数组长度应是外界赋值
        //public string[] ImpactLabel;  //影响标签  凡是元素集内的标签 才会被技能影响
        //public Collider ColliderSelf;
        //public Transform[] AllTarget;
        //[SerializeField]
        //public float _currentTime = 0;

        //public Vector3 TargetPos;
        //public vCharacter Character;

        public float CurCooling = 0;

        public SkillGroup() { }

        public SkillGroup(SkillGroup other)
        {
            for (int i = 0; i < other.Behaviors.Count; i++)
            {
                //Behaviors.Add(other.Behaviors[i].Clone());
            }
        }

        

        public string GetDescription()
        {
            return "";
        }


        /// <summary>
        /// 顺序执行所有效果
        /// </summary>
        /// <param name="controller"></param>
        private void DoBehaviors(vCharacter controller)
        {
            for (int i = 0; i < Behaviors.Count; i++)
            {
                Behaviors[i].Execute(Behaviors[i], _continued, controller);
            }
        }
        public void Reset()
        {
            foreach (ISkillTrigger trigger in Behaviors)
            {
                trigger.Reset();
            }
        }

        public int GetTriggerCount(string typeName)
        {
            int count = 0;
            foreach (ISkillTrigger trigger in Behaviors)
            {
                if (trigger.GetTypeName() == typeName)
                    ++count;
            }
            return count;
        }

        /// <summary>
        /// Skill更新
        /// </summary>
        public void Change()
        {
            switch (CurRunStatus)
            {
                case SkillRunStatus.TriggeredIng:
                    _continued += Time.deltaTime;
                    DoBehaviors(Character);
                    if (_continued >= ContinuedTime)
                    {
                        _continued = 0;
                        CurCooling = CoolingTime;
                        Reset();
                        CurRunStatus = SkillRunStatus.Restoreing;
                    }
                    break;
                case SkillRunStatus.NotTriggered:
                    break;
                case SkillRunStatus.Restoreing:
                    CurCooling -=Time.deltaTime;
                    if (CurCooling <= 0)
                    {
                        CurCooling = 0;
                        CurRunStatus = SkillRunStatus.NotTriggered;
                    }
                    break;
            }
        }

        public void ChangeStatus(SkillRunStatus targetStatus,vCharacter character)
        {
            if (targetStatus == SkillRunStatus.TriggeredIng)
            {
                if (CurRunStatus == SkillRunStatus.NotTriggered)
                {
                    _continued = 0;
                    Character = character;
                    CurRunStatus = SkillRunStatus.TriggeredIng;
                }else if (CurRunStatus == SkillRunStatus.TriggeredIng)
                {
                    Debug.Log("释放中");
                }else if (CurRunStatus == SkillRunStatus.Restoreing)
                {
                    Debug.Log("CD中");
                }
            }
        }
    }

}


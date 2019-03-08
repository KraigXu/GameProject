using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Skill
{
    public class SkillGroup 
    {
        public int Id;                                                          //编号
        public string Name;                                                     //名称
        public string Description;                                              //说明
        public float CoolingTime;                                               //冷却
        public Sprite Icon;                                                     //图标
        public Color sceneGizmoColor = Color.gray;                              //拿来渲染eyes的Gizmos颜色
        public List<SkillBehavior> Behaviors=new List<SkillBehavior>();

        public bool m_IsUsed = false;
        public SkillGroup() { }

        public SkillGroup(SkillGroup other)
        {
            for (int i = 0; i < other.Behaviors.Count; i++)
            {
                //Behaviors.Add(other.Behaviors[i].Clone());
            }
        }
        public void UpdateBehaviors(SkillInstance controller)
        {
            DoBehaviors(controller);
        }

        /// <summary>
        /// 顺序执行所有效果
        /// </summary>
        /// <param name="controller"></param>
        private void DoBehaviors(SkillInstance controller)
        {
            for (int i = 0; i < Behaviors.Count; i++)
            {
                Behaviors[i].Execute(Behaviors[i], controller._currentTime,controller);
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
    }

}


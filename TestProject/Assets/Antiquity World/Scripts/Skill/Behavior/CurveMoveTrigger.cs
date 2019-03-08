using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Skill
{
    /// <summary>
    ///  曲线移动
    /// </summary>
    public class CurveMoveTrigger : SkillBehavior
    {
        private float m_StartPlayTime = 0;
        private int m_AnimationId = 0;


        public override ISkillTrigger Clone()
        {
            return new CurveMoveTrigger();
        }

        public override bool Execute(ISkillTrigger instance, float curTime, SkillInstance controller)
        {
            if (curTime >= m_StartTime && m_IsExected == false)
            {
                m_IsExected = true;
                Debug.Log("CurveMove");
                return true;
            }
            return false;
        }

        public override void Init(string args)
        {
            string[] values = args.Split(',');
            m_StartTime = float.Parse(values[0]);
            m_StartPlayTime = float.Parse(values[1]);
            m_AnimationId = int.Parse(values[2]);
        }
    }
}
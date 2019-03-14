using System.Collections;
using System.Collections.Generic;
using GameSystem.Skill;
using UnityEngine;

namespace GameSystem.Skill
{
    /// <summary>
    /// 播放指定动画 
    /// </summary>
    public class PlayAnimationTrigger : SkillTrigger
    {
        private float m_StartPlayTime = 0;
        private int m_AnimationId = 0;

        public override ISkillTrigger Clone()
        {
            return new PlayAnimationTrigger();
        }

        public override bool Execute(ISkillTrigger instance, float curTime, SkillInstance controller)
        {
            if (curTime >= m_StartTime && m_IsExected == false)
            {
                Debug.Log("PlayAnimation");
                //controller.Character.animator=
                m_IsExected = true;
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

        public override void Reset()
        {
            base.Reset();
        }
    }
}
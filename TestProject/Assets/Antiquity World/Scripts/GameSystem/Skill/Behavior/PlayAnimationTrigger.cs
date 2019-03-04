using System.Collections;
using System.Collections.Generic;
using GameSystem.Skill;
using UnityEngine;

namespace GameSystem.Skill
{
    public class PlayAnimationTrigger : SkillBehavior
    {
        private float m_StartPlayTime = 0;
        private int m_AnimationId = 0;

        public override void Act(SkillInstance controller)
        {
        }

        public override ISkillTrigger Clone()
        {
            ISkillTrigger iTrigger = new PlayAnimationTrigger();
            return iTrigger;
        }

        public override bool Execute(ISkillTrigger instance, float curTime, SkillInstance controller)
        {
            if (curTime >= m_StartTime && m_IsExected==false)
            {
                Debug.Log("PlayAnimation");
                m_IsExected = true;
                return true;
            }

            
            return false ;
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
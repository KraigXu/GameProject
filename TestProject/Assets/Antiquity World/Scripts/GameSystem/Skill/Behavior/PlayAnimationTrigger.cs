﻿using System.Collections;
using System.Collections.Generic;
using GameSystem.Skill;
using UnityEngine;

namespace GameSystem.Skill
{
    [CreateAssetMenu(menuName = "GameSystem/Skill/SkillBehavior/PlayAnimationTrigger")]
    public class PlayAnimationTrigger : SkillBehavior
    {
        private float m_StartPlayTime = 0;
        private int m_AnimationId = 0;

        public override void Act(SkillController controller)
        {

        }

        public override ISkillTrigger Clone()
        {
            ISkillTrigger iTrigger = new PlayAnimationTrigger();
            return iTrigger;
        }

        public override bool Execute(ISkillTrigger instance, float curTime)
        {
            Debug.Log("PlayAnimation");
            return true;
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
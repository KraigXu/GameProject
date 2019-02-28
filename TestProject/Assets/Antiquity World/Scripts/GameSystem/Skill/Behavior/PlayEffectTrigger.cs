using System.Collections;
using System.Collections.Generic;
using GameSystem.Skill;
using UnityEngine;

namespace GameSystem.Skill
{
    [CreateAssetMenu(menuName = "GameSystem/Skill/SkillBehavior/PlayEffectTrigger")]
    public class PlayEffectTrigger : SkillBehavior
    {
        public int EffectId;


        public override void Act(SkillInstance controller)
        {
        }

        public override ISkillTrigger Clone()
        {
            throw new System.NotImplementedException();
        }

        public override bool Execute(ISkillTrigger instance, float curTime)
        {
            if (curTime >= m_StartTime)
            {



                return true;
            }

            return false;
        }

        /// <summary>
        ///    
        /// </summary>  
        /// <param name="args">  0,0,108</param>
        public override void Init(string args)
        {
            string[] values = args.Split(',');

            m_StartTime = float.Parse(values[1]);
            EffectId = int.Parse(values[2]);

        }
    }
}
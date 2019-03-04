using System.Collections;
using System.Collections.Generic;
using GameSystem.Skill;
using UnityEngine;

namespace GameSystem.Skill
{
    public class PlayEffectTrigger : SkillBehavior
    {
        public int EffectId;


        public override void Act(SkillInstance controller)
        {
        }

        public override ISkillTrigger Clone()
        {
            return new PlayEffectTrigger();
        }

        public override bool Execute(ISkillTrigger instance, float curTime, SkillInstance controller)
        {
            Debug.Log("PlayEffect");
            if (curTime >= m_StartTime && m_IsExected == false)
            {
               SkillData skillData=  WXSkillController.instance.GetSkillData(EffectId);
                WXPoolManager.Pools[Define.PoolName].Spawn(skillData.Prefab);
                m_IsExected = true;
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
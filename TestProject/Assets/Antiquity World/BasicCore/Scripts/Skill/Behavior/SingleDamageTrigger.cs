using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;

namespace GameSystem.Skill
{
    /// <summary>
    ///  单一伤害
    /// </summary>
    public class SingleDamageTrigger : SkillTrigger
    {
        public override void Reset()
        {
            base.Reset();
        }

        public override ISkillTrigger Clone()
        {
            return new SingleDamageTrigger();
        }

        public override bool Execute(ISkillTrigger instance, float curTime, vCharacter controller)
        {
            if (curTime >= m_StartTime && m_IsExected == false)
            {
                m_IsExected = true;
                Debug.Log("CurveMove");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="args">SingleDamage(1,1,1,0,0,1,0,0,0);</param>///
        public override void Init(string args)
        {
            string[] values = args.Split(',');
            m_StartTime = float.Parse(values[0]);

        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;

namespace GameSystem.Skill
{
    public class AppendHealthTrigger : SkillTrigger
    {
        public int Value;
        public override ISkillTrigger Clone()
        {
            return  new AppendHealthTrigger();
        }

        public override bool Execute(ISkillTrigger instance, float curTime, vCharacter controller)
        {
            if (curTime >= m_StartTime && m_IsExected == false)
            {
                controller.ChangeHealth(Value);
                m_IsExected = true;
                return m_IsExected;
            }
            return m_IsExected;
        }

        /// <summary>
        /// AppendHealth(0,0.3,0,50)
        /// </summary>
        /// <param name="args"></param>
        public override void Init(string args)
        {
            string[] values = Define.SkillDataSplit(args);
            m_StartTime = float.Parse(values[1]);
            Value = int.Parse(values[2]);
        }

        public override void Reset()
        {
            base.Reset();
        }
    }

}
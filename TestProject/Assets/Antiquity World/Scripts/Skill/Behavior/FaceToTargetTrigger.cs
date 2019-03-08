using System.Collections;
using System.Collections.Generic;
using GameSystem.Skill;
using UnityEngine;

namespace GameSystem.Skill
{
    public class FaceToTargetTrigger : SkillBehavior
    {
        public override ISkillTrigger Clone()
        {
            return new FaceToTargetTrigger();
        }

        public override bool Execute(ISkillTrigger instance, float curTime, SkillInstance controller)
        {
            return false;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="args">FaceToTarget(0,0,1);</param>
        public override void Init(string args)
        {
            string[] values = Define.SkillDataSplit(args);
            m_StartTime = float.Parse(values[1]);
        }

    }
}
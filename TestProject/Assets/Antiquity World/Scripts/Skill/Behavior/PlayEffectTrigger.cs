using System.Collections;
using System.Collections.Generic;
using GameSystem.Skill;
using UnityEngine;

namespace GameSystem.Skill
{
    public class PlayEffectTrigger : SkillBehavior
    {
        public int EffectId;
        public int EffectType;    //0:原点 1:目标
        public Transform Effecttf;

        public override void Act(SkillInstance controller) { }
        public override ISkillTrigger Clone()
        {
            return new PlayEffectTrigger();
        }

        public override bool Execute(ISkillTrigger instance, float curTime, SkillInstance controller)
        {
            Debug.Log("PlayEffect");
            if (curTime >= m_StartTime && m_IsExected == false)
            {
                SkillData skillData = WXSkillController.instance.GetSkillData(EffectId);

                if (EffectType == 0)
                {
                    Effecttf = WXPoolManager.Pools[Define.PoolName].Spawn(skillData.Prefab,controller.transform, controller.transform.position, controller.transform.rotation);
                }
                else if (EffectType == 1)
                {
                    Effecttf = WXPoolManager.Pools[Define.PoolName].Spawn(skillData.Prefab, controller.TargetPos);
                }
                m_IsExected = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 初始化
        /// </summary>  
        /// <param name="args">  0,0,108,1</param>
        public override void Init(string args)
        {
            string[] values = args.Split(',');

            m_StartTime = float.Parse(values[1]);
            EffectId = int.Parse(values[2]);
            EffectType = int.Parse(values[3]);
        }
    }
}
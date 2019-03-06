using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Skill
{
    public class PlayEffectLoopTrigger : SkillBehavior
    {
        public int EffectId;
        public int EffectNumber;   //0无限：

        private Transform Effectgo;
        

        public override void Act(SkillInstance controller)
        {
        }

        public override ISkillTrigger Clone()
        {
            return new PlayEffectLoopTrigger();
        }

        public override bool Execute(ISkillTrigger instance, float curTime, SkillInstance controller)
        {

            if (curTime >= m_StartTime && m_IsExected == false)
            {
                SkillData skillData = WXSkillController.instance.GetSkillData(EffectId);

                if (EffectNumber == 0)
                {
                    Effectgo= WXPoolManager.Pools[Define.PoolName].Spawn(skillData.Prefab,controller.transform, controller.transform.position, controller.transform.rotation);
                    
                    
                }
                else 
                {

                }
                m_IsExected = true;
                return true;
            }
            return false;

        }

        public override void Init(string args)
        {
            string[] values = args.Split(',');

            m_StartTime = float.Parse(values[1]);
            EffectId = int.Parse(values[2]);
            EffectNumber = int.Parse(values[3]);
        }

    }

}
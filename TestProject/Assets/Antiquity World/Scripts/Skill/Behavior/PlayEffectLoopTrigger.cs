using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Skill
{
    public class PlayEffectLoopTrigger : SkillBehavior
    {
        public int EffectId;
        public int EffectNumber;   //0无限：

        private Transform Effecttf;

        public override ISkillTrigger Clone()
        {
            return new PlayEffectLoopTrigger();
        }

        public override bool Execute(ISkillTrigger instance, float curTime, SkillInstance controller)
        {
            if (curTime >= m_StartTime)
            {
                if (Effecttf == null)
                {
                    m_IsExected = true;
                    SkillData skillData = FightingScene.Instance.GetSkillData(EffectId);
                    Effecttf = WXPoolManager.Pools[Define.PoolName].Spawn(skillData.Prefab);
                }

                if (EffectNumber == 0)
                {
                    Effecttf.transform.position = controller.transform.position;
                }
                else
                {
                    Effecttf.transform.position = controller.TargetPos;
                }
            }
            return m_IsExected;
        }

        public override void Reset()
        {
            base.Reset();
            Effecttf = null;
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
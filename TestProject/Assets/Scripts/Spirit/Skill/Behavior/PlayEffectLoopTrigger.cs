using System.Collections;
using System.Collections.Generic;
using AntiquityWorld;
using Invector.vCharacterController;
using UnityEngine;

namespace GameSystem.Skill
{
    public class PlayEffectLoopTrigger : SkillTrigger
    {
        public int EffectId;
        public int EffectNumber;   //0无限：

        private Transform Effecttf;

        public override ISkillTrigger Clone()
        {
            return new PlayEffectLoopTrigger();
        }

        public override bool Execute(ISkillTrigger instance, float curTime, vCharacter controller)
        {
            if (curTime >= m_StartTime)
            {
                if (Effecttf == null)
                {
                    m_IsExected = true;
                    ParticleItem skillData = FightingController.Instance.GetSkillData(EffectId);
                    Effecttf = WXPoolManager.Pools[Define.ParticlePool].Spawn(skillData.Prefab);
                }

                if (EffectNumber == 0)
                {
                    Effecttf.transform.position = controller.transform.position;
                }
                else
                {
                    Effecttf.transform.position = controller.transform.forward * 4;
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
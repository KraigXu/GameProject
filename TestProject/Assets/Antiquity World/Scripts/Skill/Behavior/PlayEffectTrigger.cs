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
        public int EffectNumber;   //0无限：
        public Transform Effecttf;

        public override ISkillTrigger Clone()
        {
            return new PlayEffectTrigger();
        }

        public override void Reset()
        {
            base.Reset();
            WXPoolManager.Pools[Define.PoolName].Despawn(Effecttf);
            Effecttf = null;
           // EffectId = 0;
           // EffectType = 0;
          //  EffectNumber = 0;

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

                    // WXDespawn despawn= Effecttf.transform.GetComponent<WXDespawn>();
                    //if (despawn == null)
                    //    despawn = Effecttf.gameObject.AddComponent<WXDespawn>();
                    //despawn.DespawnDelay = controller.ContinuedTime - controller._currentTime;
                    //Debug.Log(despawn.DespawnDelay);
                }


                if (EffectType == 0)
                {
                    Effecttf.transform.position = controller.transform.position;
                }
                else
                {
                    Effecttf.transform.position = controller.TargetPos;
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

        /// <summary>
        /// 初始化
        /// </summary>  
        /// <param name="args"> 0,0,108,1,0</param>
        public override void Init(string args)
        {
            string[] values = args.Split(',');

            m_StartTime = float.Parse(values[1]);
            EffectId = int.Parse(values[2]);
            EffectType = int.Parse(values[3]);
            EffectNumber = int.Parse(values[4]);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Skill
{
    public class AreaDamageTrigger : SkillBehavior
    {


        public float Radius=3;
        public float Limit=360;

        public Transform _hitEffect;

        public override void Act(SkillInstance controller)
        {
        }

        public override ISkillTrigger Clone()
        {
            return new AreaDamageTrigger();
        }

        public override bool Execute(ISkillTrigger instance, float curTime, SkillInstance controller)
        {
            Debug.Log("AreaDamageTrigger");
            if (curTime >= m_StartTime && m_IsExected == false)
            {
                m_IsExected = true;

                Vector3 point = controller.TargetPos;

                List<Transform> enemy = FightingScene.Instance.Enemy;

                for (int i = 0; i < enemy.Count; i++)
                {
                    float distance = Vector3.Distance(enemy[i].position, point);
                    if (distance < Radius)
                    {
                        Debug.Log("Hit >>>"+enemy[i].name);
                        WXPoolManager.Pools[Define.PoolName].Spawn(_hitEffect);
                    }
                }
               


               // controller
               //SkillData skillData = WXSkillController.instance.GetSkillData(EffectId);
               //WXPoolManager.Pools[Define.PoolName].Spawn(skillData.Prefab);

                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">(0,3,-1,0,0,3,5,360,0,0,0,211)
        /// 3:
        /// -1:开始时间
        /// 0:结束时间
        ///0:次数
        /// 3,
        /// 5,
        /// 360,
        /// 3,
        /// 0,
        /// 0,
        /// 211, :击中后特效ID
        /// </param>
        public override void Init(string args)
        {
            string[] values = args.Split(',');
            m_StartTime =float.Parse(values[1]);
            Limit = float.Parse(values[7]);
            Radius = float.Parse(values[8]);
            _hitEffect = WXSkillController.instance.GetSkillData(int.Parse(values[11])).Prefab;

        }
    }

}
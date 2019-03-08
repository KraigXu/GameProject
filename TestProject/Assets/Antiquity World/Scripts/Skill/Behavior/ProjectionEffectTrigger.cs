using System.Collections;
using System.Collections.Generic;
using GameSystem.Skill;
using UnityEngine;
namespace GameSystem.Skill
{
    /// <summary>
    /// 发射弹道
    /// </summary>
    public class ProjectionEffectTrigger : SkillBehavior
    {
        public int ProjectileEffectId;
        public int EffectImpactId;
        public Transform Effecttf;
        

        public override ISkillTrigger Clone()
        {
            return new ProjectionEffectTrigger();
        }

        public override bool Execute(ISkillTrigger instance, float curTime, SkillInstance controller)
        {
            if (curTime >= m_StartTime && m_IsExected == false)
            {
                SkillData skillData = FightingScene.Instance.GetSkillData(ProjectileEffectId);
                Effecttf = WXPoolManager.Pools[Define.PoolName].Spawn(skillData.Prefab, controller.transform, controller.transform.position, controller.transform.rotation);

                WXProjectile projectile = Effecttf.GetComponent<WXProjectile>();
                projectile.EffectImpactId = EffectImpactId;
                m_IsExected = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="args">ProjectionEffect(0,0.5,15,0,16,0,3,360,10,0,0,3);</param>
        ///0,
        ///0.5,
        ///0,       第一ID
        ///0,
        ///1,
        ///0,
        /// 3,
        /// 360,
        /// 
        ///
        /// 
        public override void Init(string args)
        {
            string[] values = Define.SkillDataSplit(args);
            m_StartTime = int.Parse(values[1]);
            ProjectileEffectId = int.Parse(values[2]);
            EffectImpactId = int.Parse(values[4]);


        }
    }

}

using System.Collections;
using System.Collections.Generic;
using Invector;
using UnityEngine;
using Invector.vCharacterController;
using Invector.vMelee;
using MapMagic;

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

                WXProjectile projectile = Effecttf.gameObject.AddComponent<WXProjectile>();
                projectile.EffectImpactId = EffectImpactId;
                m_IsExected = true;
                return true;
            }

            return false;
        }

        public override void Init(string args)
        {
            Debug.Log(args);
            string[] values = args.Split(',');
            m_StartTime = float.Parse(values[1]);
            ProjectileEffectId = int.Parse(values[2]);
            EffectImpactId = int.Parse(values[4]);
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
    }

}

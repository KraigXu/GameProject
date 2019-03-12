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
        public int ProjectileNumber;
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
                if (Effecttf == null)
                {
                    SkillData skillData = FightingScene.Instance.GetSkillData(ProjectileEffectId);
                    Vector3[] angles=new Vector3[ProjectileNumber];

                    float roation2 = ProjectileNumber * 15f/2f;
                    for (int i = 0; i < angles.Length; i++)
                    {
                        Vector3 angle = controller.transform.eulerAngles+new Vector3(0,15*i-roation2,0);
                        Effecttf = WXPoolManager.Pools[Define.PoolName].Spawn(skillData.Prefab, controller.transform.position + new Vector3(0, 1.5f, 0), controller.transform.rotation);
                        Effecttf.Rotate(angle);

                        WXProjectile projectile = Effecttf.GetComponent<WXProjectile>();
                        projectile.EffectImpactId = EffectImpactId;
                    }

                }

                m_IsExected = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 0,0,15,3,16,0,3,360,10,0,0,3
        /// </summary>
        /// <param name="args"></param>
        public override void Init(string args)
        {
            string[] values = Define.SkillDataSplit(args);

            m_StartTime = float.Parse(values[1]);
            ProjectileEffectId = int.Parse(values[2]);
            ProjectileNumber = int.Parse(values[3]);
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

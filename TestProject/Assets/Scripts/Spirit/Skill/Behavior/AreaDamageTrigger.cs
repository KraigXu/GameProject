using System.Collections;
using System.Collections.Generic;
using AntiquityWorld;
using Invector;
using UnityEngine;
using Invector.vCharacterController;
using Invector.vMelee;

namespace GameSystem.Skill
{
    public class AreaDamageTrigger : SkillTrigger
    {

        public float Radius = 3;
        public float Limit = 360;

        public Transform _hitEffect;

        public Vector3 center;
        public override ISkillTrigger Clone()
        {
            return new AreaDamageTrigger();
        }

        public override bool Execute(ISkillTrigger instance, float curTime, vCharacter controller)
        {
            if (curTime >= m_StartTime && m_IsExected == false)
            {
                m_IsExected = true;

                Vector3 point = controller.transform.forward * 4;

                List<Transform> enemy = FightingController.Instance.Enemy;
                vCharacter enemyCr = null;
                for (int i = 0; i < enemy.Count; i++)
                {
                    float distance = Vector3.Distance(enemy[i].position, point);
                    if (distance < Radius)
                    {
                        enemyCr = enemy[i].GetComponent<vCharacter>();
                        int hurt = controller.AttackValue - enemyCr.DefenseValue;
                        if (hurt <= 0)
                        {
                            hurt = 0;
                        }

                        vDamage _damage = new vDamage();
                        _damage.sender = controller.transform;
                        _damage.receiver = enemyCr.transform;
                        _damage.damageValue = (int)Mathf.RoundToInt(((float)(controller.AttackValue + 2) * (((float)10) * 0.01f)));
                        _damage.hitPosition = enemy[i].position;
                        enemyCr.gameObject.ApplyDamage(_damage);

                        //GameObject go=new GameObject();
                        //BoxCollider collider= go.AddComponent<BoxCollider>();
                        //vHitBox box= go.AddComponent<vHitBox>();
                        //enemyCr.ChangeHealth(-hurt);

                        Debug.Log("Hit >>>" + enemy[i].name + ">>>造成" + _damage.damageValue);
                        WXPoolManager.Pools[Define.ParticlePool].Spawn(_hitEffect);
                    }
                }
                // controller
                //SkillData skillData = WXSkillController.instance.GetSkillData(EffectId);
                //WXPoolManager.Pools[Define.PoolName].Spawn(skillData.Prefab);

                return true;
            }
            return false;
        }

        void OnDrawGizmos()
        {
            if (m_IsExected == true)
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
                Gizmos.DrawSphere(center, Radius);
            }
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
            m_StartTime = float.Parse(values[1]);
            Limit = float.Parse(values[7]);
            Radius = float.Parse(values[8]);
            _hitEffect = FightingController.Instance.GetSkillData(int.Parse(values[11])).Prefab;

        }
    }

}
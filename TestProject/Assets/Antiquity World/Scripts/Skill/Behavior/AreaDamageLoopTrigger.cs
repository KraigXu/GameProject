using System.Collections;
using System.Collections.Generic;
using Invector;
using UnityEngine;
using Invector.vCharacterController;
using Invector.vMelee;

namespace GameSystem.Skill
{
    public class AreaDamageLoopTrigger : SkillBehavior
    {
        public int TargetType;
        public float Radius = 3;
        public float Limit = 360;
        public float Interval = 0.3f;
        public Transform _hitEffect;
        

        public Vector3 center;
        private float cd=0;
        public override void Act(SkillInstance controller)
        {
        }
        public override ISkillTrigger Clone()
        {
            return new AreaDamageTrigger();
        }

        public override bool Execute(ISkillTrigger instance, float curTime, SkillInstance controller)
        {
            Debug.Log("AreaDamageLoopTrigger");
            if (curTime >= m_StartTime)
            {
                m_IsExected = true;
                Vector3 point = controller.transform.position;
                if (TargetType == 0)
                {
                    point = controller.transform.position;
                }else if (TargetType == 1)
                {
                    point = controller.TargetPos;
                }

                bool flag = false;
                cd += Time.deltaTime;
                if (cd >= Interval)
                {
                    cd = 0;
                    flag = true;
                }

                if (flag == false)
                    return true;

                List<Transform> enemy = FightingScene.Instance.Enemy;
                vCharacter enemyCr = null;
                for (int i = 0; i < enemy.Count; i++)
                {
                    float distance = Vector3.Distance(enemy[i].position, point);
                    if (distance < Radius)
                    {
                        enemyCr = enemy[i].GetComponent<vCharacter>();
                        int hurt = controller.Character.AttackValue - enemyCr.DefenseValue;
                        if (hurt <= 0)
                        {
                            hurt = 0;
                        }
                        vDamage _damage = new vDamage();
                        _damage.sender = controller.transform;
                        _damage.receiver = enemyCr.transform;
                        _damage.damageValue = (int)Mathf.RoundToInt(((float)(controller.Character.AttackValue + 2) * (((float)10) * 0.01f)));
                        _damage.hitPosition = enemy[i].position;
                        enemyCr.gameObject.ApplyDamage(_damage);
                        //enemyCr.ChangeHealth(-hurt);
                        Debug.Log("Hit >>>" + enemy[i].name + ">>>造成" + _damage.damageValue);
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

        void OnDrawGizmos()
        {
            if (m_IsExected == true)
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
                Gizmos.DrawSphere(center, Radius);
            }
        }




        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="args">(0,0.5,0.3,1,0,0,3,3,360,0.5,0,0,3)
        /// 0:目标类型                       //0
        /// 0.5:开始时间             //1
        /// 0.3:伤害率               //2
        /// 1;伤害类型               //3
        /// 0:是否可暴击             //4
        /// 0;是否可打断             //5
        /// 3,                        //6
        /// 5,半径                    //7
        /// 360;角度                 //8
        /// 0.5:间隔秒                //9
        /// 0:                       //10
        /// 0:                        //11
        /// 3:击中后特效ID         //12
        /// </param>
        public override void Init(string args)
        {
            string[] values = args.Split(',');
            TargetType = int.Parse(values[0]);
            m_StartTime = float.Parse(values[1]);
            Limit = float.Parse(values[8]);
            Radius = float.Parse(values[7]);
            Interval = float.Parse(values[9]);
            _hitEffect = WXSkillController.instance.GetSkillData(int.Parse(values[12])).Prefab;

        }
    }

}
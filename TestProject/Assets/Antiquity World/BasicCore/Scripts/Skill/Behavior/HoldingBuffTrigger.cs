using Invector.vCharacterController;
using UnityEngine;
namespace GameSystem.Skill
{
    public class HoldingBuff : SkillTrigger
    {
        public GameObject Effect;
        public GameObject HitEffect;
        public float Range;
        public float Frequency = 0.3f;                  //频率

        private GameObject _currentEffect;
        private vCharacter skillController;


        public override void Reset()
        {
            base.Reset();
            //GameObject = null;
        }


        public override ISkillTrigger Clone()
        {
            return new HoldingBuff();
        }

        public override bool Execute(ISkillTrigger instance, float curTime, vCharacter controller)
        {
            if (_currentEffect == null)
            {
                _currentEffect = GameObject.Instantiate(Effect, controller.transform);
                _currentEffect.transform.position = controller.transform.position;
                WXTime.time.AddTimer(Frequency, 10, Dot);
                skillController = controller;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Init(string args)
        {
            string[] values = Define.SkillDataSplit(args);
            m_StartTime = float.Parse(values[1]);

        }

        void Dot()
        {
            Ray ray = new Ray(skillController.transform.position, skillController.transform.up);
            RaycastHit[] hit = Physics.SphereCastAll(ray, Range, 10);
            Transform[] targets = new Transform[hit.Length];
            for (int i = 0; i < hit.Length; i++)
            {
                targets[i] = hit[i].collider.transform;

                //  GameObject go = Instantiate(HitEffect, targets[i]);
                //go.transform.position = targets[i].position;
            }
            //skillController.AllTarget = targets;
        }
    }

}


using System.Collections;
using System.Collections.Generic;
using AntiquityWorld;
using Invector.vCharacterController;
using UnityEngine;

namespace GameSystem.Skill
{
    /// <summary>
    ///  曲线移动
    /// </summary>
    public class CurveMoveTrigger : SkillTrigger
    {
        private int CurveId;
        private CurveData data;
        private float time = 0;

        public override ISkillTrigger Clone()
        {
            return new CurveMoveTrigger();
        }

        public override void Reset()
        {
            base.Reset();
            time = 0;
            data = null;
        }

        public override bool Execute(ISkillTrigger instance, float curTime, vCharacter controller)
        {
            if (curTime >= m_StartTime)
            {
                if (m_IsExected == false)
                {
                    data = FightingController.Instance.CurveDatas[CurveId];
                }

                if (time <= data.Time)
                {
                    time += Time.deltaTime;
                    float x = data.X.Evaluate(time);
                    float y = data.Y.Evaluate(time);
                    float z = data.Z.Evaluate(time);
                    controller.transform.position = controller.transform.position + new Vector3(x * Time.deltaTime, y * Time.deltaTime, z * Time.deltaTime);
                }
                m_IsExected = true;
                return true;
            }
            return false;
        }

        public override void Init(string args)
        {
            string[] values = args.Split(',');
            m_StartTime = float.Parse(values[1]);
            CurveId = int.Parse(values[2]);
        }
    }
}
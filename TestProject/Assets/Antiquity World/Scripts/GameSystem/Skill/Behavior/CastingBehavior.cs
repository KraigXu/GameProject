using System.Collections;
using System.Collections.Generic;
using GameSystem.Skill;
using UnityEngine;

namespace GameSystem.Skill
{
    public class CastingBehavior : SkillBehavior
    {
        public GameObject Effect;
        public float Continued=1;

        private GameObject _currentEffect;

        public override void Act(SkillInstance controller)
        {
            if (_currentEffect == null)
            {
              //  _currentEffect = Instantiate(Effect, controller.transform);
              //  _currentEffect.transform.position = controller.transform.position;
            }

            if (controller._currentTime > Continued)
            {
                if (_currentEffect != null)
                {
                 //   Destroy(_currentEffect);
                }
                  
            }
        }

        public override ISkillTrigger Clone()
        {
            return new CastingBehavior();
        }

        public override bool Execute(ISkillTrigger instance, float curTime, SkillInstance controller)
        {
            throw new System.NotImplementedException();
        }

        public override void Init(string args)
        {
            throw new System.NotImplementedException();
        }
    }

}
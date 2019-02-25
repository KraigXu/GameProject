using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Skill
{

    [CreateAssetMenu(menuName = "GameSystem/Skill/Behavior/PlayAnimationBehavior")]
    public class PlayAnimationBehavior : SkillBehavior
    {
        public float StartTime;
        public int AnimationId;

        private Animator _animator;

        public override void Act(SkillController controller)
        {
            if (_animator == null)
            {
                _animator = controller.GetComponent<Animator>();
            }


        }

    }
}
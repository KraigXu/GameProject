using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Skill
{
    [CreateAssetMenu(menuName = "GameSystem/Skill/SkillEffect/RoundRangeEffect")]
    public class RoundRangeEffect : SkillEffect
    {
        public override bool Decide(SkillController controller)
        {
            return true;
        }
    }
}


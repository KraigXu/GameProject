using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Skill
{
    public abstract  class SkillBehavior : ScriptableObject
    {
        public abstract void Act(SkillController controller);
    }
}


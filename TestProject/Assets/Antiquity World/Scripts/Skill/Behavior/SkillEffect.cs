using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Skill
{
    /// <summary>
    /// 决定下一个状态
    /// </summary>
    public abstract class SkillEffect : ScriptableObject
    {
        public abstract bool Decide(SkillInstance controller);
    }
}
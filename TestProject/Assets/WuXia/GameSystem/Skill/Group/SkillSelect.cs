namespace GameSystem.Skill
{
    /// <summary>
    /// 状态转换。通过决定，选中两种状态中其中一个
    /// </summary>
    [System.Serializable]
    public class SkillSelect
    {
        public SkillEffect decision;
        public SkillBehavior trueState;
        public SkillBehavior falseState;
    }
}

using GameSystem.Skill;

public interface ISkillTrigger
{
    void Init(string args);
    void Reset();
    ISkillTrigger Clone();
    bool Execute(ISkillTrigger instance, float curTime, SkillInstance controller);
    float GetStartTime();
    bool IsExecuted();
    SkillTriggerExecuteType GetExecuteType();
    string GetTypeName();
}

public enum SkillTriggerExecuteType
{
    STET_SKILL_START


}
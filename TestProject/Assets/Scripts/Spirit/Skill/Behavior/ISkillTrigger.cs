
using GameSystem.Skill;
using Invector.vCharacterController;

public interface ISkillTrigger
{
    void Init(string args);
    void Reset();
    ISkillTrigger Clone();
    bool Execute(ISkillTrigger instance, float curTime, vCharacter controller);
    float GetStartTime();
    bool IsExecuted();
    SkillTriggerExecuteType GetExecuteType();
    string GetTypeName();
}

public enum SkillTriggerExecuteType
{
    STET_SKILL_START=0,
    STET_SkILL_OVER = 1,
    STET_SKILL_INJURED =2,
   
}
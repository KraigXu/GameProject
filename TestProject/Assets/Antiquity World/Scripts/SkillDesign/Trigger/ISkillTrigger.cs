﻿
public interface ISkillTrigger
{
    void Init(string args);
    void Reset();
    ISkillTrigger Clone();
    bool Execute(ISkillTrigger instance, float curTime);
    float GetStartTime();
    bool IsExecuted();
    SkillTriggerExecuteType GetExecuteType();
    string GetTypeName();
}


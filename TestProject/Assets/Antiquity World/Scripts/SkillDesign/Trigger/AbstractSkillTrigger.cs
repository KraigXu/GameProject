﻿
public abstract class AbstractSkillTrigger : ISkillTrigger
{
    protected float m_StartTime = 0;
    protected bool m_IsExected = false;
    protected string m_TypeName;
    protected SkillTriggerExecuteType m_ExecuteType = SkillTriggerExecuteType.STET_SKILL_START;

    public abstract void Init(string args);

    public virtual void Reset()
    {
        m_IsExected=false;
    }
    public abstract ISkillTrigger Clone();
    public abstract bool Execute(ISkillTrigger instance, float curTime);

    public float GetStartTime() { return m_StartTime;}

    public bool IsExecuted() { return m_IsExected;}
    public SkillTriggerExecuteType GetExecuteType(){return m_ExecuteType;}

    public string GetTypeName() { return m_TypeName;}

}

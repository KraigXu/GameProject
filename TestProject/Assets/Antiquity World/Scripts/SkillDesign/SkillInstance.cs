using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInstance
{
    public bool m_IsUsed = false;
    public List<ISkillTrigger> m_SkillTrigers = new List<ISkillTrigger>();

    public SkillInstance() { }

    public SkillInstance(SkillInstance other)
    {
        foreach (ISkillTrigger trigger in other.m_SkillTrigers)
        {
            m_SkillTrigers.Add(trigger.Clone());
        }
    }

    public void Reset()
    {
        foreach (ISkillTrigger trigger in m_SkillTrigers)
        {
            trigger.Reset();
        }
    }

    public int GetTriggerCount(string typeName)
    {
        int count = 0;
        foreach (ISkillTrigger trigger in m_SkillTrigers)
        {
            if (trigger.GetTypeName() == typeName)
                ++count;
        }
        return count;
    }
}

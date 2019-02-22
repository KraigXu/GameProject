using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTriggerFactory<T> where T:AbstractSkillTrigger
{

}

public class SkillTriggerMgr : MonoBehaviour
{
    private static SkillTriggerMgr _instance;

    public static SkillTriggerMgr Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    public void RegisterTriggerFactory<T>(string skillCode, SkillTriggerFactory<T> skillTrigger) where T:AbstractSkillTrigger
    {

    }
}



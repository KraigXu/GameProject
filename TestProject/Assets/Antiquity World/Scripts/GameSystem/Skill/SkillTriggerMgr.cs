using System;
using System.Collections;
using System.Collections.Generic;
using GameSystem.Skill;
using UnityEngine;

public class SkillTriggerMgr
{
    private static SkillTriggerMgr _instance;

    public static SkillTriggerMgr Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance=new SkillTriggerMgr();
            }
            return _instance;
        }
    }

    public Dictionary<string, Type> DicSkillTriggerRegister = new Dictionary<string, Type>();

    /// <summary>
    /// 注册
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="skillCode"></param>
    /// <param name="skillTrigger"></param>
    public void RegisterTriggerFactory<T>(string skillCode, SkillTriggerFactory<T> skillTrigger) where T : ISkillTrigger
    {
        if (DicSkillTriggerRegister.ContainsKey(skillCode) == false)
        {
            DicSkillTriggerRegister.Add(skillCode, typeof(T));
        }
        else
        {

        }
    }

    //public ISkillTrigger CreateTrigger(string type, string args)
    //{
    //    //if (DicSkillTriggerRegister.ContainsKey(type) == true)
    //    //{

    //    //}
    //    Type t = DicSkillTriggerRegister[type];
    //    object o = System.Activator.CreateInstance(t);  //创建实例
    //    System.Reflection.MethodInfo mi = t.GetMethod("Init");
    //    mi.Invoke(o, new object[] { args });
    //    Debug.Log(">>>>>>>>>");
    //    return (ISkillTrigger)o;
    //}

    public SkillBehavior CreateTrigger(string type, string args)
    {
        //if (DicSkillTriggerRegister.ContainsKey(type) == true)
        //{

        //}
        Type t = DicSkillTriggerRegister[type];
        object o = System.Activator.CreateInstance(t);  //创建实例
        System.Reflection.MethodInfo mi = t.GetMethod("Init");
        mi.Invoke(o, new object[] { args });
        Debug.Log(">>>>>>>>>");
        return (SkillBehavior)o;
    }
}

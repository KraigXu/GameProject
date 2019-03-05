using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GameSystem.Skill;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

public class SkillTriggerFactory<T>
{
    public T data;
}

public sealed class SkillSystem
{
    public static Dictionary<int, SkillGroup> DicSkillInstancePool = new Dictionary<int, SkillGroup>();
  
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Initialize()
    {
        Instance.RegisterTriggerFactory("PlayEffect", new SkillTriggerFactory<PlayEffectTrigger>());
        Instance.RegisterTriggerFactory("PlaySound", new SkillTriggerFactory<PlaySoundTrigger>());
        Instance.RegisterTriggerFactory("PlayAnimation", new SkillTriggerFactory<PlayAnimationTrigger>());

        Instance.RegisterTriggerFactory("SingleDamage", new SkillTriggerFactory<PlayAnimationTrigger>());
        Instance.RegisterTriggerFactory("AreaDamage",new SkillTriggerFactory<AreaDamageTrigger>());
        Instance.RegisterTriggerFactory("CurveMove",new SkillTriggerFactory<PlayEffectTrigger>());

        ParseScript(Application.streamingAssetsPath + "/SkillScript.txt");

    }
    private static SkillSystem _instance;

    public static SkillSystem Instance
    {
        get
        {
            if (_instance == null)
            {
               _instance=new SkillSystem();
            }

            return _instance;
        }
    }

    public Dictionary<string, Type> DicSkillTriggerRegister = new Dictionary<string, Type>();

    private static bool ParseScript(string filename)
    {
        bool ret = false;
        try
        {
            StreamReader sr = FileReaderProxy.ReadFile(filename);
            if (sr != null)
                ret = LoadScriptFromStream(sr);
        }
        catch (Exception e)
        {
            string err = "Exception:" + e.Message + "\n" + e.StackTrace + "\n";
            Debuger.LogError(err);
        }
        return ret;
    }

    /// <summary>
    /// 加载脚本
    /// </summary>
    /// <param name="sr"></param>
    /// <returns></returns>
    private static bool LoadScriptFromStream(StreamReader sr)
    {
        bool bracket = false;
        SkillGroup skill = null;
        do
        {
            string line = sr.ReadLine();
            if (line == null)
                break;

            line = line.Trim();

            if (line.StartsWith("//") || line == "")
                continue;

            if (line.StartsWith("skill"))
            {
                int start = line.IndexOf("(");
                int end = line.IndexOf(")");
                if (start == -1 || end == -1)
                    Debuger.LogError("ParseScript Error, start == -1 || end == -1  {0}", line);

                int length = end - start - 1;
                if (length <= 0)
                {
                    Debuger.LogError("ParseScript Error, length <= 1, {0}", line);
                    return false;
                }

                string args = line.Substring(start + 1, length);
                int skillId = (int)Convert.ChangeType(args, typeof(int));
                skill = new SkillGroup();
                AddSkillInstanceToPool(skillId, skill, true);
            }
            else if (line.StartsWith("{"))
            {
                bracket = true;
            }
            else if (line.StartsWith("}"))
            {
                bracket = false;

                // 按时间排序
                skill.Behaviors.Sort((left, right) =>
                {
                    if (left.GetStartTime() > right.GetStartTime())
                    {
                        return -1;
                    }
                    else if (left.GetStartTime() == right.GetStartTime())
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                });
            }
            else
            {
                // 解析trigger
                if (skill != null && bracket == true)
                {
                    int start = line.IndexOf("(");
                    int end = line.IndexOf(")");
                    if (start == -1 || end == -1)
                        Debuger.LogError("ParseScript Error, {0}", line);

                    int length = end - start - 1;
                    if (length <= 0)
                    {
                        Debuger.LogError("ParseScript Error, length <= 1, {0}", line);
                        return false;
                    }

                    string type = line.Substring(0, start);
                    string args = line.Substring(start + 1, length);
                    args = args.Replace(" ", "");
                    SkillBehavior trigger = Instance.CreateTrigger(type, args);
                    if (trigger != null)
                    {
                        skill.Behaviors.Add(trigger);
                    }
                    else
                    {
                        Debug.Log("Not "+type+"  Type");
                    }
                }
            }
        } while (true);


        return true;
    }

    public static void AddSkillInstanceToPool(int skillId, SkillGroup skill, bool v)
    {
        if (DicSkillInstancePool.ContainsKey(skillId) == false)
        {
            DicSkillInstancePool.Add(skillId, skill);
        }

    }


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

    public SkillBehavior CreateTrigger(string type, string args)
    {
        Debug.Log(type);
        //if (DicSkillTriggerRegister.ContainsKey(type) == true)
        //{

        //}

       
        Type t = DicSkillTriggerRegister[type];
        object o = System.Activator.CreateInstance(t);  //创建实例
       
        System.Reflection.MethodInfo mi = t.GetMethod("Init");
        mi.Invoke(o, new object[] { args });
        return (SkillBehavior)o;
    }

    public SkillInstance NewSkillInstance(int skillId)
    {
        SkillInstance skillInstance=new SkillInstance();
        return skillInstance;
    }

    public SkillGroup NewSkillGroup(int skillId)
    {
        SkillGroup group = null;
        if (DicSkillInstancePool.ContainsKey(skillId))
        {
            group = DicSkillInstancePool[skillId];
        }
        return group;
    }

}

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

public sealed class SkillSystem : ScriptableSingleton<SkillSystem>
{

    public static Dictionary<int, SkillGroup> DicSkillInstancePool = new Dictionary<int, SkillGroup>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Initialize()
    {
        SkillTriggerMgr.Instance.RegisterTriggerFactory("PlayAnimation", new SkillTriggerFactory<PlayAnimationTrigger>());
        SkillTriggerMgr.Instance.RegisterTriggerFactory("SingleDamage", new SkillTriggerFactory<PlayAnimationTrigger>());
        ParseScript(Application.streamingAssetsPath + "/SkillScript1000.txt");

        //Debug.Log(DicSkillInstancePool.Count);
        //DicSkillTrigger["PlayAnimation"].Execute(instance)
    }

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
                    SkillBehavior trigger = SkillTriggerMgr.Instance.CreateTrigger(type, args);
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

}

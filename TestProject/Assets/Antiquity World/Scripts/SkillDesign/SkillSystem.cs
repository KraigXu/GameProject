using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SkillTriggerFactory<T> where T : AbstractSkillTrigger
{
    public T Instance;

    public AbstractSkillTrigger GetIntance()
    {
        AbstractSkillTrigger trigger = Instance;
        return trigger;
    }

    public SkillTriggerFactory()
    {

    }
}

public sealed class SkillSystem : ScriptableSingleton<SkillSystem>
{
    public static Dictionary<string, AbstractSkillTrigger> DicSkillTrigger=new Dictionary<string, AbstractSkillTrigger>();

    public static Dictionary<int, SkillInstance> DicSkillInstancePool = new Dictionary<int, SkillInstance>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
       RegisterTriggerFactory("PlayAnimation",new SkillTriggerFactory<PlayAnimationTrigger>());

        //DicSkillTrigger["PlayAnimation"].Execute(instance)
    }

    private bool ParseScript(string filename)
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

    private bool LoadScriptFromStream(StreamReader sr)
    {
        bool bracket = false;
        SkillInstance skill = null;
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
                skill = new SkillInstance();
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
                skill.m_SkillTrigers.Sort((left, right) =>
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
                    ISkillTrigger trigger = CreateTrigger(type, args);
                    if (trigger != null)
                    {
                        skill.m_SkillTrigers.Add(trigger);
                    }
                }
            }
        } while (true);


        return true;
    }

    public ISkillTrigger CreateTrigger(string type, string args)
    {
        ISkillTrigger s=new PlayAnimationTrigger();

        return s;

    }


    /// <summary>
    /// 注册
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="skillCode"></param>
    /// <param name="skillTrigger"></param>
    public static void RegisterTriggerFactory<T>(string skillCode, SkillTriggerFactory<T> skillTrigger) where T : AbstractSkillTrigger
    {
        if (DicSkillTrigger.ContainsKey(skillCode) == false)
        {
            DicSkillTrigger.Add(skillCode, skillTrigger.GetIntance());
        }
        else
        {

        }





    }


    public void AddSkillInstanceToPool(int skillId, SkillInstance skill, bool v)
    {
        if (DicSkillInstancePool.ContainsKey(skillId) == false)
        {
            DicSkillInstancePool.Add(skillId,skill);
        }
    }

}

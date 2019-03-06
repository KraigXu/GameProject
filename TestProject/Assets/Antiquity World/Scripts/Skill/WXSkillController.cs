using System;
using System.Collections;
using System.Collections.Generic;
using GameSystem.Skill;
using UnityEngine;

[Serializable]
public class SkillData
{
    public int Id;
    public Transform Prefab;
}

public class WXSkillController : MonoBehaviour
{

    public static WXSkillController instance;

    public List<SkillData> SkillDatas = new List<SkillData>();

    void Awake()
    {
        instance = this;
    }
    public SkillData GetSkillData(int id)
    {
        for (int i = 0; i < SkillDatas.Count; i++)
        {
            if (SkillDatas[i].Id == id)
            {
                return SkillDatas[i];
            }
        }

        return SkillDatas[0];
    }

    public void ShowSkill(Vector3 pos, Transform tf, Transform parent)
    {
        SkillInstance node = WXPoolManager.Pools[Define.PoolName].Spawn(tf, parent, pos).GetComponent<SkillInstance>();
    }

}

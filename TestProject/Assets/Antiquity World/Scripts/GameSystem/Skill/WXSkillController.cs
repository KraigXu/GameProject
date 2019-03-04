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

    public List<SkillData> SkillDatas=new List<SkillData>();
    public Transform SkillSource;

    void Awake()
    {
        instance = this;
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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

    public void ShowSkill(Vector3 pos, Transform tf,Transform parent)
    {
        SkillInstance node = WXPoolManager.Pools[Define.PoolName].Spawn(tf, parent, pos).GetComponent<SkillInstance>();


    }

    public void ShowSkill(vSkillManger manger, Transform parent,Vector3 pos, int skillId)
    {
        SkillInstance controller = WXPoolManager.Pools[Define.PoolName].Spawn(SkillSource, parent, pos).GetComponent<SkillInstance>();
        controller.CurrentGroup = SkillSystem.Instance.NewSkillGroup(skillId);
       // SkillControllers.Add(controller);
    }
}

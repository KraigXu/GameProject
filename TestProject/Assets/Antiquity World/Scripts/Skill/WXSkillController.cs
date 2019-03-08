using System;
using System.Collections;
using System.Collections.Generic;
using GameSystem.Skill;
using UnityEngine;



public class WXSkillController : MonoBehaviour
{

    public static WXSkillController instance;

    

    void Awake()
    {
        instance = this;
    }
   

    public void ShowSkill(Vector3 pos, Transform tf, Transform parent)
    {
        SkillInstance node = WXPoolManager.Pools[Define.PoolName].Spawn(tf, parent, pos).GetComponent<SkillInstance>();
    }

}

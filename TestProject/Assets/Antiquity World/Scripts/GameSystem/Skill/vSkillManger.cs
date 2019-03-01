using System.Collections;
using System.Collections.Generic;
using GameSystem;
using GameSystem.Skill;
using Invector;
using UnityEngine;

public class vSkillManger : vMonoBehaviour
{
    /// <summary>
    /// ALL skill
    /// </summary>
    public List<SkillGroup> SkillGroups=new List<SkillGroup>();
    public List<int> SkillId=new List<int>();

    public List<KeyValuePair<int, float>> SkillCD = new List<KeyValuePair<int, float>>();
    public List<SkillInstance> SkillControllers = new List<SkillInstance>();

    public bool IsFast = false;
    public bool IsReadying;
   
    void Start () {

        //InitSkill
	    for (int i = 0; i < SkillId.Count; i++)
	    {
	        SkillGroups.Add(SkillSystem.Instance.NewSkillGroup(SkillId[i]));
        }

	}
	
	void Update () {

	    if (Input.GetKey(KeyCode.LeftShift))
	    {
	        IsFast = true;
        }
	    else
	    {
	        IsFast = false;
	    }

        if (Input.GetKeyUp(KeyCode.Alpha2))
	    {
	        if (Check(SkillGroups[0].Id) == false)
	        {
	            if (IsFast == true)
	            {
	                SkillInstance controller = WXPoolManager.Pools[Define.PoolName].Spawn(FightingScene.Instance.SkillPrefab, transform).GetComponent<SkillInstance>();
	                controller.CurrentGroup = SkillSystem.Instance.NewSkillGroup(SkillId[0]);
	                SkillControllers.Add(controller);
	            }
	            else
	            {



	            }
	        }
	        else
	        {
	            Debug.Log("正在CD中");
	        }


         //   if (SkillGroups[0] != null)
	        //{
	            
         //   }
	    }


	    if (Input.GetKeyUp(KeyCode.Alpha3))
	    {
            Debug.Log(">>Skill3");
	    }
	}

    void FixedUpdate()
    {
        List<int> count=new List<int>();

        for (int i = 0; i < SkillCD.Count; i++)
        {
            var node = SkillCD[i];

            float value = node.Value- Time.deltaTime;
            if (value > 0)
            {
                SkillCD[i]=new KeyValuePair<int, float>(node.Key,value);
            }
            else
            {
                count.Add(i);
            }
        }

        for (int i = 0; i < count.Count; i++)
        {
            count.RemoveAt(count[i]);
        }
       
    }

    bool Check(int id)
    {
        for (int i = 0; i < SkillCD.Count; i++)
        {
            if (SkillCD[i].Key == id)
            {
                return true;
            }
        }

        return false;
    }
}

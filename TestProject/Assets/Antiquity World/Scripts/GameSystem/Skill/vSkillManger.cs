using System.Collections;
using System.Collections.Generic;
using GameSystem.Skill;
using Invector;
using UnityEngine;



public class vSkillManger : vMonoBehaviour
{
    /// <summary>
    /// ALL skill
    /// </summary>
    public List<SkillGroup> SkillGroups=new List<SkillGroup>();
    public List<KeyValuePair<int,float>> SkillCD=new List<KeyValuePair<int, float>>();

    public List<SkillController> SkillControllers=new List<SkillController>();
    
	
	void Start () {
		
	}
	
	void Update () {
	    if (Input.GetKeyUp(KeyCode.Alpha2))
	    {
            Debug.Log(">>Skill2");
	        if (SkillGroups[0] != null)
	        {
	            if (Check(SkillGroups[0].Id) == false)
	            {
	                SkillController controller = transform.gameObject.AddComponent<SkillController>();
	                controller.CurrentGroup = SkillGroups[0];
	                SkillControllers.Add(controller);
                }
	            else
	            {
                    Debug.Log("正在CD中");
	            }
            }
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

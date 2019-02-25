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

	
	void Start () {
		
	}
	
	void Update () {
	    if (Input.GetKeyUp(KeyCode.Alpha2))
	    {
            Debug.Log(">>Skill2");
	    }
	}
}

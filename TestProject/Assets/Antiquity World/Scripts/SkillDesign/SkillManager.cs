using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {



    void Awake()
    {

        SkillTriggerMgr.Instance.RegisterTriggerFactory("PlayAnimation",new SkillTriggerFactory<PlayAnimationTrigger>());
       // SkillTriggerMgr.Instance.RegisterTriggerFactory();
    }

	void Start () {
		
	}

	void Update () {
		
	}
}

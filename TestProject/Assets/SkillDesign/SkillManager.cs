using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {



    void Awake()
    {

        SkillTriggerMgr.Instance.RegisterTriggerFactory("PlayAnimation",new SkillTriggerFactory<PlayAnimationTrigger>());
    }

	void Start () {
		
	}

	void Update () {
		
	}
}

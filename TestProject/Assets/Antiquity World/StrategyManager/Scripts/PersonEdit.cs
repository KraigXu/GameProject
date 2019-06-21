using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataAccessObject;

public class PersonEdit : MonoBehaviour {






    void Awake()
    {
        SQLService.GetInstance("TD.db");
    }


	void Start () {
		
	}
	
	void Update () {
		
	}
}

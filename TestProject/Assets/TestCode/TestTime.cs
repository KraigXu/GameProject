using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTime : MonoBehaviour
{

    public string time = "190-12-1";
	// Use this for initialization
	void Start ()
	{
        Debug.Log(Convert.ToDateTime(time).ToString("yyy-MM-dd"));
	    
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

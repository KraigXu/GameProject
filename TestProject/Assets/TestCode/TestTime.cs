using System;
using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Newtonsoft.Json;
using UnityEngine;

public class TestTime : MonoBehaviour
{

    public string time = "190-12-1";
    
	// Use this for initialization
	void Start ()
	{
        Debug.Log(Convert.ToDateTime(time).ToString("yyy-MM-dd"));
        //Dictionary<string,string> Map=new Dictionary<string, string>();
	   // Map.Add("KDS_S");

        List<string> Map=new List<string>();
        Map.Add("KDS_8");
	    Map.Add("KDS_7");
	    Map.Add("KDS_6");
	    Map.Add("KDS_5");
	    Map.Add("KDS_4");
	    Map.Add("KDS_3");
	    Map.Add("KDS_2");
	    Map.Add("KDS_1");

        Debug.Log(JsonConvert.SerializeObject(Map));
	    List<BiologicalData> biologicalModels = SqlData.GetWhereDatas<BiologicalData>(" IsDebut=? ", new object[] { 1 });

    }


	
	// Update is called once per frame
	void Update () {
		
	}
}


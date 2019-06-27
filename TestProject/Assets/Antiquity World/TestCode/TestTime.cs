using System.Collections;
using System.Collections.Generic;
using GameSystem;
using UnityEngine;
using UnityEngine.UI;

public class TestTime : MonoBehaviour
{
    public Text txt1;
    public Text txt2;
    public Text txt3;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    txt1.text = WorldTimeManager.Instance.YearS;
	    txt2.text = WorldTimeManager.Instance.MonthS;
	    txt3.text = WorldTimeManager.Instance.DayS;
	}

    public void AddTime()
    {
        WorldTimeManager.AddDay(10);

    }
}

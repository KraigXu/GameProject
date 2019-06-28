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
    public Text txt4;

    void Start () {
		
	}
	
	void Update ()
	{
	    txt1.text = WorldTime.Year.ToString();
	    txt2.text = WorldTime.Month.ToString();
	    txt3.text = WorldTime.Day.ToString();
	    txt4.text = WorldTime.ShiChen.ToString();
	}

    public void AddTime()
    {
        WorldTime.AddDay(10);

    }
}

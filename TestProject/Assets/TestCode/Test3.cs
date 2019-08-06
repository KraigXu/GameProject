using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test3 : MonoBehaviour
{

    public Text text1;
    public Text text2;
    public Text text3;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
	{
	    text1.text = WorldTime.Instance.CurTime.Year.ToString();
	    text2.text = WorldTime.Instance.CurTime.Month.ToString();
	    text3.text = WorldTime.Instance.CurTime.Day.ToString();

	}
}

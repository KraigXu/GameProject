using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameDebug : MonoBehaviour
{

    public bool IsShow;
    public GameObject Content;
    public InputField InputContent;
	void Start () {
		
	}
	
	void Update ()
	{


	    if (Input.GetKeyDown(KeyCode.BackQuote))
	    {
	        IsShow = !IsShow;
	        Content.SetActive(IsShow);
            if (IsShow == true)
            {
                InputContent.ActivateInputField();
                InputContent.text = "";
            }
        }

        if (IsShow == true)
	    {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                string[] items = InputContent.text.Split('.');
                GameObject.Find(items[0]).SendMessage(items[1],items[2]);
	            InputContent.text = "";
	            InputContent.ActivateInputField();
	        }
	    }
	}

    public void Test(int value)
    {
        Debug.Log(value);
    }
}

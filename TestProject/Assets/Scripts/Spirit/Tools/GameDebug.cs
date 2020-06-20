using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDebug : MonoBehaviour
{

    public bool IsShow;
    public GameObject Content;
    public InputField InputContent;
	void Start ()
	{
	    string strs = "GameDebug.Test(11,11,11,11)";
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
                if (InputContent.text.Length <= 0)
                    return;

                string strs = InputContent.text;

                string gamename = strs.Substring(0, strs.IndexOf('.'));
                string mainname = strs.Substring(strs.IndexOf('.') + 1, strs.IndexOf('(') - strs.IndexOf('.') - 1);
                Debug.Log(gamename);
                Debug.Log(mainname);
                string content = strs.Substring(strs.IndexOf('(') + 1, strs.IndexOf(')') - 1 - strs.IndexOf('('));
                string[] values = content.Split(',');
                GameObject.Find(gamename).SendMessage(mainname, values);
	            InputContent.text = "";
	            InputContent.ActivateInputField();
	        }
	    }
	}

    public void Test(object value)
    {
        string[] values = (string[])value;

        for (int i = 0; i < values.Length; i++)
        {
            Debug.Log(values[i]);
        }
        Debug.Log(values.ToString());
    }

    public void ShowSkillCount(object value)
    {
        Debug.Log(SkillSystem.DicSkillInstancePool.Count);

    }
}

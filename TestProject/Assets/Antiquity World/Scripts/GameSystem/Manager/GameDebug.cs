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

	    string io = "Player.Skill.AI 100 100 100";
	    string strs = "Game.Skill(11,11,11,11)";

        //string text=string.Format({},)


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
                Debug.Log(content);

                string[] values = content.Split(',');
               // GameObject.Find(gamename.SendMessage(str[1]);
                // SceneManager.

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

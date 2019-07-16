using System;
using System.Collections;
using System.Collections.Generic;
using GameSystem.Ui;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestWindowScript : MonoBehaviour
{

    public string inputvalue = "";

    public bool IsEdit=false;

    public int Id;
    // Use this for initialization
    void Start()
    {
        //UICenterMasterManager.Instance.ShowWindow(WindowID.MessageWindow);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.BackQuote) )
        {
            IsEdit = !IsEdit;
        }
       
    }

    



    void OnGUI()
    {

        //inputvalue = GUI.TextArea(new Rect(0, 700, 200, 60), inputvalue);
        //if (GUI.Button(new Rect(0, 500, 200, 60), "测试Message"))
        //{
        //    UICenterMasterManager.Instance.GetGameWindowScript<MessageWindow>(WindowID.MessageWindow).Log(inputvalue);
        //    //UICenterMasterManager.Instance.GetGameWindowScript<MessageWindow>(WindowID.MessageWindow).Log(inputvalue);
        //    //UICenterMasterManager.Instance.GetGameWindowScript<MessageWindow>(WindowID.MessageWindow).Log(inputvalue);
        //    //UICenterMasterManager.Instance.GetGameWindowScript<MessageWindow>(WindowID.MessageWindow).Log(inputvalue);
        //    //UICenterMasterManager.Instance.GetGameWindowScript<MessageWindow>(WindowID.MessageWindow).Log(inputvalue);
        //    //UICenterMasterManager.Instance.GetGameWindowScript<MessageWindow>(WindowID.MessageWindow).Log(inputvalue);
        //    //UICenterMasterManager.Instance.GetGameWindowScript<MessageWindow>(WindowID.MessageWindow).Log(inputvalue);
        //    //UICenterMasterManager.Instance.GetGameWindowScript<MessageWindow>(WindowID.MessageWindow).Log(inputvalue);
        //    //UICenterMasterManager.Instance.get
        //}

        //if (GUI.Button(new Rect(0, 200, 100, 30), "测试SocialDialog"))
        //{
        //    UICenterMasterManager.Instance.ShowWindow(WindowID.SocialDialogWindow);
        //    // UICenterMasterManager.Instance.GetGameWindowScript<SocialDialogWindow>(WindowID.SocialDialogWindow);
        //}
        //if (GUI.Button(new Rect(0, 230, 100, 30), "关闭SocialDialog"))
        //{
        //    UICenterMasterManager.Instance.DestroyWindow(WindowID.SocialDialogWindow);
        //    // UICenterMasterManager.Instance.GetGameWindowScript<SocialDialogWindow>(WindowID.SocialDialogWindow);
        //}

        if (IsEdit == true)
        {
            float windth = Screen.width * 0.6f;
            float height = Screen.height * 0.4f;

            GUI.Window(0, new Rect(Screen.width - windth, height, windth, height), Debuger.OnDebugWindow, "Debug");
        }

    }

}

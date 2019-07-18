using System;
using System.Collections;
using System.Collections.Generic;
using AntiquityWorld.StrategyManager;
using DataAccessObject;
using GameSystem.Ui;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[Serializable]
public struct AttributeGroup
{
    public ENUM_ITEM_ATTRIBUTE Key;
    public string Value;
}

public class TestWindowScript : MonoBehaviour
{

    public string inputvalue = "";

    public bool IsEdit=false;

    public ArticleData ArticleData;
    public GUISkin Skin;
    public int windth = 200;
    public int height = 200;


    public Vector2 position;
    public Dictionary<ENUM_ITEM_TEXT, string> ValuesDic;
    public List<AttributeGroup> ValuePairs1;



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

        GUI.skin = Skin;
        height = ValuePairs1.Count * 18;
        GUILayout.BeginArea(new Rect(Screen.width / 2, Screen.height / 2, windth, height),Skin.GetStyle("window"));
        GUILayout.BeginVertical();
        
        for (int i = 0; i < ValuePairs1.Count; i++)
        {
            switch (ValuePairs1[i].Key)
            {
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_CLASS:
                    GUILayout.Label(ValuePairs1[i].Value,Skin.GetStyle("articleclass"));
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_NAME:
                    GUILayout.Label(ValuePairs1[i].Value,Skin.GetStyle("articlename"));
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_TYPE:
                    GUILayout.Label(ValuePairs1[i].Value,Skin.GetStyle("articletype"));
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_MAX_ATTACK:
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_MIN_ATTACK:
                    GUILayout.Label("Min Attack"+ValuePairs1[i].Value,Skin.GetStyle("lableh"));
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_EXPAIN:
                    GUILayout.Label(ValuePairs1[i].Value, Skin.GetStyle("normallable"));
                    break;

            }

        }
        GUILayout.EndVertical();




        GUILayout.EndArea();
    }

    //void OnGUI()
    //{

    //    //inputvalue = GUI.TextArea(new Rect(0, 700, 200, 60), inputvalue);
    //    //if (GUI.Button(new Rect(0, 500, 200, 60), "测试Message"))
    //    //{
    //    //    UICenterMasterManager.Instance.GetGameWindowScript<MessageWindow>(WindowID.MessageWindow).Log(inputvalue);
    //    //    //UICenterMasterManager.Instance.GetGameWindowScript<MessageWindow>(WindowID.MessageWindow).Log(inputvalue);
    //    //    //UICenterMasterManager.Instance.GetGameWindowScript<MessageWindow>(WindowID.MessageWindow).Log(inputvalue);
    //    //    //UICenterMasterManager.Instance.GetGameWindowScript<MessageWindow>(WindowID.MessageWindow).Log(inputvalue);
    //    //    //UICenterMasterManager.Instance.GetGameWindowScript<MessageWindow>(WindowID.MessageWindow).Log(inputvalue);
    //    //    //UICenterMasterManager.Instance.GetGameWindowScript<MessageWindow>(WindowID.MessageWindow).Log(inputvalue);
    //    //    //UICenterMasterManager.Instance.GetGameWindowScript<MessageWindow>(WindowID.MessageWindow).Log(inputvalue);
    //    //    //UICenterMasterManager.Instance.GetGameWindowScript<MessageWindow>(WindowID.MessageWindow).Log(inputvalue);
    //    //    //UICenterMasterManager.Instance.get
    //    //}

    //    //if (GUI.Button(new Rect(0, 200, 100, 30), "测试SocialDialog"))
    //    //{
    //    //    UICenterMasterManager.Instance.ShowWindow(WindowID.SocialDialogWindow);
    //    //    // UICenterMasterManager.Instance.GetGameWindowScript<SocialDialogWindow>(WindowID.SocialDialogWindow);
    //    //}
    //    //if (GUI.Button(new Rect(0, 230, 100, 30), "关闭SocialDialog"))
    //    //{
    //    //    UICenterMasterManager.Instance.DestroyWindow(WindowID.SocialDialogWindow);
    //    //    // UICenterMasterManager.Instance.GetGameWindowScript<SocialDialogWindow>(WindowID.SocialDialogWindow);
    //    //}

    //    //if (IsEdit == true)
    //    //{
    //    //    float windth = Screen.width * 0.6f;
    //    //    float height = Screen.height * 0.4f;

    //    //    GUI.Window(0, new Rect(Screen.width - windth, height, windth, height), Debuger.OnDebugWindow, "Debug");
    //    //}

    //    if (EventSystem.current.IsPointerOverGameObject() == false)
    //    {
    //        ArticleData = new ArticleData
    //        {
    //            Id = 1,
    //            Name = "XX",
    //            Desc = "*********************************************************",
    //            AvatarId = 1,
    //            Count = 1,
    //            Type1 = 1,

    //        };
    //        List<string> s= new List<string>();


    //        ValuesDic=new Dictionary<ENUM_ITEM_TEXT, string>()
    //        {
    //            {ENUM_ITEM_TEXT.ITEM_TEXT_NAME,"XX" },
    //            {ENUM_ITEM_TEXT.ITEM_TEXT_EXPAIN,"****************************************************" },
    //        };

    //        ValuePairs1 = new List<KeyValuePair<ENUM_ITEM_TEXT, string>>
    //        {

    //        };

    //        List<KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>> valuePairs=new List<KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>>
    //        {
    //            //new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.)
    //        };




    //        position = Input.mousePosition;

    //        string title="";

    //        if (ArticleData.Type1 > 0 && ArticleData.Type1<3)
    //        {
    //            title = "道具";
    //        }


    //        GUI.Window(1, new Rect(position.x, Screen.height-position.y, windth,height), OnWindowId, title);

    //    }


    //}

    public void OnWindowId(int id)
    {
        ArticleData articleData = ArticleData;

        GUILayout.BeginVertical();

        GUILayout.Label(ValuesDic[ENUM_ITEM_TEXT.ITEM_TEXT_NAME], Skin.GetStyle("lable1"));

        GUILayout.Label(ValuesDic[ENUM_ITEM_TEXT.ITEM_TEXT_EXPAIN], Skin.GetStyle("lableh"));

        GUILayout.EndVertical();



    }



}

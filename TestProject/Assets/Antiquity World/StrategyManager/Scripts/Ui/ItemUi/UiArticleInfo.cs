﻿using DataAccessObject;
using System.Collections;
using System.Collections.Generic;
using AntiquityWorld.StrategyManager;
using GameSystem;
using Newtonsoft.Json;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 信息展示
/// </summary>
public class UiArticleInfo : MonoBehaviour
{

    public RectTransform ContentRect1;  //名称
    public Text NameTxt;


    public RectTransform ContentRect2;  //属性
    public Text TypeTxt;

    public RectTransform ContentRect3;  //凹槽技能


    public RectTransform ContentRect4;  //说明
    public Text ExplainTxt;
    public RectTransform ContentRect5;  //价格
    public Text PriceTxt;

    public ArticleData CurData;
    public GUISkin Skin;

    public Texture Texture;

    private Entity _curEntity;

    public Entity CurEntity
    {
        get { return _curEntity; }
        set
        {
            _curEntity = value;
            ChangeEntity();
        }
    }

    void Awake()
    {

    }

    private Text _nameTxt;
    
    


    /// <summary>
    /// 更新界面数据
    /// </summary>
    void ChangeEntity()
    {
        EntityManager entityManager = World.Active.GetOrCreateManager<EntityManager>();

        //解析参数

        ArticleItemFixed item = GameStaticData.ArticleDictionary[CurEntity];

        if (item == null)
        {
            Debug.LogError("数据错误！");
            return;
        }

        int width = 300;
        int height = 300;

        
        _nameTxt.text = item.Name;

        Dictionary<ENUM_ITEM_TEXT, string> textDic = JsonConvert.DeserializeObject<Dictionary<ENUM_ITEM_TEXT, string>>(CurData.Text);
        NameTxt.text = textDic[ENUM_ITEM_TEXT.ITEM_TEXT_NAME];
        ExplainTxt.text = textDic[ENUM_ITEM_TEXT.ITEM_TEXT_EXPAIN];

        Dictionary<ENUM_ITEM_ATTRIBUTE, string> attributeDic = JsonConvert.DeserializeObject<Dictionary<ENUM_ITEM_ATTRIBUTE, string>>(CurData.Value);


        ArticleTypeData typeData = SQLService.Instance.QueryUnique<ArticleTypeData>(" Id=?", CurData.Type1);
        TypeTxt.text = typeData.Text;

        switch ((ENUM_ITEM_CLASS)typeData.Id)
        {
            case ENUM_ITEM_CLASS.ITEM_CLASS_SKILL_BOOK:
                ContentRect3.gameObject.SetActive(true);

                ContentRect4.gameObject.SetActive(true);

                ContentRect5.gameObject.SetActive(true);

                break;
            case ENUM_ITEM_CLASS.ITEM_CLASS_BOX:


                break;
            case ENUM_ITEM_CLASS.ITEM_CLASS_EQUIPMENT:
                ContentRect2.gameObject.SetActive(true);

                //GameObject item = new GameObject();
                //RectTransform rect1 = (RectTransform)item.transform;
                //rect1.SetParent(ContentRect2);
                //rect1.anchorMin = Vector2.zero;
                //rect1.anchorMax = Vector2.one;

                //Text text1 = item.AddComponent<Text>();
                //text1.text = attributeDic[ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_MIN_ATTACK] + "-" + attributeDic[ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_MAX_ATTACK];
                //ContentRect3.gameObject.SetActive(true);

                //ContentRect4.gameObject.SetActive(true);

                //ContentRect5.gameObject.SetActive(true);

                break;
            case ENUM_ITEM_CLASS.ITEM_CLASS_RESOURCE:
                break;
            case ENUM_ITEM_CLASS.ITEM_CLASS_JEWEL:
                break;
            case ENUM_ITEM_CLASS.ITEM_CLASS_RUNE:
                break;
            case ENUM_ITEM_CLASS.ITEM_CLASS_STONE:
                break;
            case ENUM_ITEM_CLASS.ITEM_CLASS_TASK:
                break;
            case ENUM_ITEM_CLASS.ITEM_CLASS_DRAW:
                break;
            case ENUM_ITEM_CLASS.ITEM_CLASS_WEAPON:
                break;
        }



        string content1;
        List<KeyValuePair<ENUM_ITEM_TEXT, string>> valuetext = JsonConvert.DeserializeObject<List<KeyValuePair<ENUM_ITEM_TEXT, string>>>(CurData.Text);

        NameTxt.text = CurData.Text;
        List<KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>> valuePairs = JsonConvert.DeserializeObject<List<KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>>>(CurData.Value);



        for (int i = 0; i < valuePairs.Count; i++)
        {



        }

    }


    // Use this for initialization
    void Start()
    {
        //List<KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>> valuePairs = new List<KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>>();
        ////valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_ATTACKSPEED, "10"));
        ////valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_CRIT, "0"));
        ////valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_DODGE, "0"));
        ////valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_JEWEL_2, "30"));
        ////valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_JEWEL_3, "0"));

        //valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_MIN_ATTACK, "30"));
        //valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_MAX_ATTACK, "35"));
        //valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_CRIT, "20"));
        //valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_HIT, "90"));
        //valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_SKILL_ATTACK, "1"));

        //string json = JsonConvert.SerializeObject(valuePairs);

        Dictionary<int, string> valuePairs = new Dictionary<int, string>();
        valuePairs.Add(1, "30");
        valuePairs.Add(2, "35");
        valuePairs.Add(3, "20");
        valuePairs.Add(4, "90");
        valuePairs.Add(5, "1");
        string json = JsonConvert.SerializeObject(valuePairs);
        Debug.Log(json);
        Dictionary<ENUM_ITEM_ATTRIBUTE, string> v = JsonConvert.DeserializeObject<Dictionary<ENUM_ITEM_ATTRIBUTE, string>>(json);

        Debug.Log(JsonConvert.SerializeObject(v));
        //valuePairs = JsonConvert.DeserializeObject<List<KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>>>(json);

        ////  Debug.Log(JsonConvert.SerializeObject(valuePairs));

        List<KeyValuePair<ENUM_ITEM_TEXT, string>> valuetext = new List<KeyValuePair<ENUM_ITEM_TEXT, string>>();
        //valuetext.Add(new KeyValuePair<string, string>(">>>>1",">>>>1"));
        //valuetext.Add(new KeyValuePair<string, string>(""));


    }

    

    void Update()
    {

    }

    public void OnWindowNew(int id)
    {
        GUI.Label(new Rect(0,0,461,72),"摇篮",Skin.GetStyle("label"));
        GUI.Label(new Rect(0, 72, 461,16),"护符",Skin.GetStyle("normallable"));
        GUI.Label(new Rect(0,88,461,42),"物理防御999",Skin.GetStyle("lableMax") );


        GUI.Label(new Rect(10,130,26,26), Texture);

        GUI.Label(new Rect(42,130,461,26),"+2 智力",Skin.GetStyle("lableblue") );
        GUI.Label(new Rect(42, 156, 461, 26), "+1 烈火学派", Skin.GetStyle("lableblue"));
        GUI.Label(new Rect(42, 182, 461, 26), "+2 大气学派", Skin.GetStyle("lableblue"));
        GUI.Label(new Rect(42, 208, 461, 26), "+1 领袖", Skin.GetStyle("lableblue"));
        GUI.Label(new Rect(42, 234, 461, 26), "+1 坚毅", Skin.GetStyle("lableblue"));

        GUI.Label(new Rect(10,260,26,26), Texture);
        GUI.Label(new Rect(42,260,461,26),"等级21",Skin.GetStyle("lableh") );

        GUI.Label(new Rect(10, 286, 26, 26), Texture);
        GUI.Label(new Rect(42, 286, 461, 26), "巨型火焰威能符文", Skin.GetStyle("lable1"));
        GUI.Label(new Rect(42, 312, 461, 26), "智力 + 3", Skin.GetStyle("lable1"));
        GUI.Label(new Rect(42, 338, 461, 26), "暴击率 +12%", Skin.GetStyle("lable1"));

        //  GUI.Label(new Rect(20,), );

        GUI.Label(new Rect(20,411,345,50),"神圣",Skin.GetStyle("lablem") );
        GUI.Label(new Rect(280,411,65,50),"9999999Y",Skin.GetStyle("lablem") );
    }



}

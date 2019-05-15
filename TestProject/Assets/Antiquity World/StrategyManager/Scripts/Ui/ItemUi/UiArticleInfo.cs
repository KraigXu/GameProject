using DataAccessObject;
using System.Collections;
using System.Collections.Generic;
using AntiquityWorld.StrategyManager;
using Newtonsoft.Json;
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

    void Awake()
    {

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
        Dictionary<ENUM_ITEM_ATTRIBUTE,string> v = JsonConvert.DeserializeObject<Dictionary<ENUM_ITEM_ATTRIBUTE, string>>(json);

        Debug.Log(JsonConvert.SerializeObject(v));
        //valuePairs = JsonConvert.DeserializeObject<List<KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>>>(json);

        ////  Debug.Log(JsonConvert.SerializeObject(valuePairs));

        List<KeyValuePair<ENUM_ITEM_TEXT, string>> valuetext = new List<KeyValuePair<ENUM_ITEM_TEXT, string>>();
        //valuetext.Add(new KeyValuePair<string, string>(">>>>1",">>>>1"));
        //valuetext.Add(new KeyValuePair<string, string>(""));


    }


    public void Change()
    {


        Dictionary<ENUM_ITEM_TEXT,string> textDic=JsonConvert.DeserializeObject<Dictionary<ENUM_ITEM_TEXT, string>>(CurData.Text);
        NameTxt.text = textDic[ENUM_ITEM_TEXT.ITEM_TEXT_NAME];
        ExplainTxt.text = textDic[ENUM_ITEM_TEXT.ITEM_TEXT_EXPAIN];


        ArticleTypeData typeData= SQLService.Instance.QueryUnique<ArticleTypeData>(" Id=?", CurData.Type1);
        TypeTxt.text = typeData.Text;

        switch ((ENUM_ITEM_CLASS)typeData.Id)
        {
            case ENUM_ITEM_CLASS.ITEM_CLASS_SKILL_BOOK:
                ContentRect3.gameObject.SetActive(true);



                break;
            case ENUM_ITEM_CLASS.ITEM_CLASS_BOX:
                break;
            case ENUM_ITEM_CLASS.ITEM_CLASS_EQUIPMENT:
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
        }



        string content1;
        List<KeyValuePair<ENUM_ITEM_TEXT, string>> valuetext = JsonConvert.DeserializeObject<List<KeyValuePair<ENUM_ITEM_TEXT, string>>>(CurData.Text);


        NameTxt.text = CurData.Text;





        List<KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>> valuePairs = JsonConvert.DeserializeObject<List<KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>>>(CurData.Value);



        for (int i = 0; i < valuePairs.Count; i++)
        {



        }



    }



    void Update()
    {

    }
}

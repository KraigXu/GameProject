using System;
using GameSystem;
using UnityEngine;
using UnityEngine.EventSystems;

public class StrategyMouseInfo : MonoBehaviour
{

    public static StrategyMouseInfo Instance
    {
        get { return _instance; }
    }
    private static StrategyMouseInfo _instance;


    public GameObject CurObject
    {
        get { return _curObject; }
    }

    [SerializeField]
    private GameObject _curObject;

    public bool IsShowArticleUi;

    private string _target;

    [SerializeField]
    private GUISkin Skin;
    [SerializeField]
    private Texture Texture;



    void Awake()
    {
        _instance = this;
    }


    public void InObject(GameObject go)
    {

        if (_curObject == null)    //当前对象为空时
        {
            _curObject = go;
        }else if (go.GetInstanceID() != _curObject.GetInstanceID())   //当前对象!=时
        {
            _curObject = go;
        }
        else
        {
            return;
        }


        if (go.GetComponent<UiArticleBox>() != null)
        {
            IsShowArticleUi = true;
            _target = "ArticleInfo";
        }
    }

    public void PutObject(GameObject go)
    {
        if (_curObject.GetInstanceID() == go.GetInstanceID())
        {
            _curObject = null;
            IsShowArticleUi = false;
            _target = string.Empty;
        }

    }

	void Start () {
	}
	
	void Update () {
	}

    void OnGUI()
    {
        if (IsShowArticleUi)
        {
            var point = Input.mousePosition;
            GUI.Window(0, new Rect(point.x,Screen.height-point.y, 345, 461), ArticleInfoUi, "", Skin.GetStyle("window"));
        }

    }

    private void ArticleInfoUi (int id)
    {
        if (_curObject != null)
        {
            UiArticleBox articleBox = _curObject.GetComponent<UiArticleBox>();
        }
           

        GUI.Label(new Rect(0, 0, 461, 72), "摇篮", Skin.GetStyle("label"));
        GUI.Label(new Rect(0, 72, 461, 16), "护符", Skin.GetStyle("normallable"));
        GUI.Label(new Rect(0, 88, 461, 42), "物理防御999", Skin.GetStyle("lableMax"));


        GUI.Label(new Rect(10, 130, 26, 26), Texture);

        GUI.Label(new Rect(42, 130, 461, 26), "+2 智力", Skin.GetStyle("lableblue"));
        GUI.Label(new Rect(42, 156, 461, 26), "+1 烈火学派", Skin.GetStyle("lableblue"));
        GUI.Label(new Rect(42, 182, 461, 26), "+2 大气学派", Skin.GetStyle("lableblue"));
        GUI.Label(new Rect(42, 208, 461, 26), "+1 领袖", Skin.GetStyle("lableblue"));
        GUI.Label(new Rect(42, 234, 461, 26), "+1 坚毅", Skin.GetStyle("lableblue"));

        GUI.Label(new Rect(10, 260, 26, 26), Texture);
        GUI.Label(new Rect(42, 260, 461, 26), "等级21", Skin.GetStyle("lableh"));

        GUI.Label(new Rect(10, 286, 26, 26), Texture);
        GUI.Label(new Rect(42, 286, 461, 26), "巨型火焰威能符文", Skin.GetStyle("lable1"));
        GUI.Label(new Rect(42, 312, 461, 26), "智力 + 3", Skin.GetStyle("lable1"));
        GUI.Label(new Rect(42, 338, 461, 26), "暴击率 +12%", Skin.GetStyle("lable1"));


        GUI.Label(new Rect(20, 411, 345, 50), "神圣", Skin.GetStyle("lablem"));
        GUI.Label(new Rect(280, 411, 65, 50), "9999999Y", Skin.GetStyle("lablem"));


    }


    
}

using System.Collections;
using System.Collections.Generic;
using Strategy;
using TinyFrameWork;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// LivingArea主视图
/// </summary>
public class LivingAreaMainWindow : UIWindowBase
{
    [Header("LivingArea")]
    [SerializeField]
    private Text _name;
    private LivingArea _currentLivingArea;


    [Header("Building")]
    public List<GameObject> BuildingsGo = new List<GameObject>();
    [SerializeField]
    private RectTransform _buildingContent;
    private BuildingObject[] _buildings ;
    private bool _buildingViewStatus = false;

    protected override void SetWindowId()
    {
        this.ID = WindowID.LivingAreaMainWindow;
    }

    protected override void InitWindowCoreData()
    {
        windowData.windowType = UIWindowType.NormalLayer;
        windowData.showMode = UIWindowShowMode.DoNothing;
        windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
        windowData.colliderMode = UIWindowColliderMode.None;
        windowData.closeModel = UIWindowCloseModel.Destory;
        windowData.animationType = UIWindowAnimationType.None;
        windowData.playAnimationModel = UIWindowPlayAnimationModel.Stretching;
    }

    public override void InitWindowOnAwake()
    {
        _buildingContent.Find("Exit").GetComponent<Button>().onClick.AddListener(CloseBuiding);
        Debuger.Log(">>>");
        _buildingContent.gameObject.SetActive(_buildingViewStatus);
    }

    protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
    {
        WindowContextLivingAreaNodeData nodeData = (WindowContextLivingAreaNodeData)contextData;
        if(nodeData==null) return;

        _currentLivingArea = nodeData.Node;
        //resolve LivingArea Data

        //resolve Building Data , building图生成
        GameObject buildingTitlePrefab = Define.Value.UiLivingAreaBuilding;
         _buildings = _currentLivingArea.BuildingObjects;
        for (int i = 0; i < _buildings.Length; i++)
        {
            GameObject go = UGUITools.AddChild(gameObject, buildingTitlePrefab);
            go.name = _buildings[i].Name;
            RectTransform goRect=   go.GetComponent<RectTransform>();
            goRect.anchoredPosition =new Vector2(i*20f,i*30);

            go.transform.GetChild(0).GetComponent<Text>().text = _buildings[i].Name;
           // go.transform.GetChild(1).GetComponent<Button>().interactable = _buildings[i].BuildingStatus == 0;
            go.transform.GetChild(1).GetComponent<Button>().name = _buildings[i].Name;
            //go.transform.GetChild(2).GetComponent<Image>().overrideSprite=
            go.transform.GetChild(3).GetComponent<Text>().text = _buildings[i].BuildingLevel.ToString();

            UIEventTriggerListener.Get(go).onClick += AccessBuilding;
            BuildingsGo.Add(go);

        }
    }

    private void AccessBuilding(GameObject go)
    {
        for (int i = 0; i < _buildings.Length; i++)
        {
            if (go.name == _buildings[i].Name)
            {
                OpenBuiding(_buildings[i]);
                break;
            }
        }
    }

    private void OpenBuiding(BuildingObject building)
    {
        _buildingContent.gameObject.SetActive(true);
    }


    private void CloseBuiding()
    {
        _buildingContent.gameObject.SetActive(false);

    }


    
}


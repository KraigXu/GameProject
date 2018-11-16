using System.Collections;
using System.Collections.Generic;
using WX;
using TinyFrameWork;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// LivingArea主视图
/// </summary>
public class LivingAreaMainWindow : UIWindowBase
{
    [SerializeField]
    private Text _name;
    [SerializeField]
    private Text _powerName;
    [SerializeField]
    private Text _personName;
    [SerializeField]
    private Text _money;
    [SerializeField]
    private Text _iron;
    [SerializeField]
    private Text _wood;
    [SerializeField]
    private Text _food;
    [SerializeField]
    private Text _level;
    [SerializeField]
    private Text _type;
    [SerializeField]
    private Text _strength;
    [SerializeField]
    private Text _stable;


    
    [Header("Building")]
    public List<GameObject> BuildingsGo = new List<GameObject>();
    [SerializeField]
    private RectTransform _buildingContent;
    private bool _buildingViewStatus = false;

    [SerializeField]
    private List<BaseCorrespondenceByModelControl> _buildingBilling;
    private LivingAreaWindowCD _currentLivingArea;

    



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
        _buildingContent.gameObject.SetActive(_buildingViewStatus);
    }

    protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
    {
        if(contextData==null) return;
        _currentLivingArea =(LivingAreaWindowCD)contextData;

        ChangeData();
    }

    private void ChangeData()
    {
        for (int i = 0; i < _buildingBilling.Count; i++)
        {
            _buildingBilling[i].gameObject.SetActive(false);
        }

        _name.text = GameText.NameDic[_currentLivingArea.OnlyEntity];
        _powerName.text = GameText.NameDic[_currentLivingArea.OnlyEntity];
        _personName.text = GameText.NameDic[_currentLivingArea.OnlyEntity];
        _money.text = _currentLivingArea.Money + "/" + _currentLivingArea.MoneyMax;
        _iron.text = _currentLivingArea.Iron + "/" + _currentLivingArea.IronMax;
        _wood.text = _currentLivingArea.Wood + "/" + _currentLivingArea.WoodMax;
        _food.text = _currentLivingArea.Food + "/" + _currentLivingArea.FoodMax;
        _level.text = GameText.LivingAreaLevel[_currentLivingArea.LivingAreaLevel];
        _type.text = GameText.LivingAreaType[_currentLivingArea.LivingAreaType];

        for (int i = 0; i < _currentLivingArea.BuildingiDataItems.Count; i++)
        {
            _buildingBilling[i].gameObject.SetActive(true);
            _buildingBilling[i].GetComponentInChildren<Text>().text=GameText.BuildingNameDic[_currentLivingArea.BuildingiDataItems[i].OnlyEntity];
            _buildingBilling[i].Init(StrategySceneInit.Settings.ModelCamera,UICenterMasterManager.Instance._Camera,_currentLivingArea.BuildingiDataItems[i].Point);
            UIEventTriggerListener.Get(_buildingBilling[i].gameObject).onClick += AccessBuilding;
        }



        //resolve Building Data , building图生成
        //GameObject buildingTitlePrefab = Define.Value.UiLivingAreaBuilding;
        //// _buildings = _currentLivingArea.BuildingObjects;
        //for (int i = 0; i < _buildings.Length; i++)
        //{
        //    GameObject go = UGUITools.AddChild(gameObject, buildingTitlePrefab);
        //    go.name = _buildings[i].Name;
        //    RectTransform goRect = go.GetComponent<RectTransform>();
        //    goRect.anchoredPosition = new Vector2(i * 20f, i * 30);

        //    go.transform.GetChild(0).GetComponent<Text>().text = _buildings[i].Name;
        //    // go.transform.GetChild(1).GetComponent<Button>().interactable = _buildings[i].BuildingStatus == 0;
        //    go.transform.GetChild(1).GetComponent<Button>().name = _buildings[i].Name;
        //    //go.transform.GetChild(2).GetComponent<Image>().overrideSprite=
        //    go.transform.GetChild(3).GetComponent<Text>().text = _buildings[i].BuildingLevel.ToString();

        //    UIEventTriggerListener.Get(go).onClick += AccessBuilding;
        //    BuildingsGo.Add(go);
        //}
    }

    private void AccessBuilding(GameObject go)
    {
        //for (int i = 0; i < _buildingBilling.Count; i++)
        //{
        //}
        //for (int i = 0; i < _buildings.Length; i++)
        //{
        //    if (go.name == _buildings[i].Name)
        //    {
        //        OpenBuiding(_buildings[i]);
        //        break;
        //    }
        //}
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
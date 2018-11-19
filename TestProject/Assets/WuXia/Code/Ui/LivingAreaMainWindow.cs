using System;
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
    private Text _person;
    [SerializeField]
    private Text _level;
    [SerializeField]
    private Text _type;
    [SerializeField]
    private Text _stable;

    [SerializeField]
    private List<TogglePanel> _toggleArray;
    [SerializeField]
    private List<BaseCorrespondenceByModelControl> _buildingBilling;
    [SerializeField]
    private GameObject _buildingGo;
    [SerializeField]
    private GameObject _buildingImage;
    [SerializeField]
    private GameObject _buildingContent;

    [SerializeField]
    private Button _buildingExit;



    private bool _buildingFlag=false;

    private LivingAreaWindowCD _currentLivingArea;

    [Serializable]
    class TogglePanel
    {
        public Toggle Toggle;
        public RectTransform Panel;
    }

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
        _buildingExit.onClick.AddListener(BuildingExit);
    }

    void Update()
    {
        if (_toggleArray.Count > 0)
        {
            for (int i = 0; i < _toggleArray.Count; i++)
            {
                _toggleArray[i].Panel.gameObject.SetActive(_toggleArray[i].Toggle.isOn);
            }
        }

        _buildingGo.SetActive(_buildingFlag);
    }

    protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
    {
        if (contextData == null) return;
        _currentLivingArea = (LivingAreaWindowCD)contextData;

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
        _person.text = _currentLivingArea.PersonNumber.ToString();
        _stable.text = _currentLivingArea.DefenseStrength.ToString();
        _level.text = GameText.LivingAreaLevel[_currentLivingArea.LivingAreaLevel];
        _type.text = GameText.LivingAreaType[_currentLivingArea.LivingAreaType];

        for (int i = 0; i < _currentLivingArea.BuildingiDataItems.Count; i++)
        {
            _buildingBilling[i].gameObject.SetActive(true);
            _buildingBilling[i].GetComponentInChildren<Text>().text = GameText.BuildingNameDic[_currentLivingArea.BuildingiDataItems[i].OnlyEntity];
            _buildingBilling[i].Init(StrategySceneInit.Settings.MainCamera, UICenterMasterManager.Instance._Camera, _currentLivingArea.BuildingiDataItems[i].Point);
            UIEventTriggerListener.Get(_buildingBilling[i].gameObject).onClick += AccessBuilding;
        }
    }

    private void AccessBuilding(GameObject go)
    {
        for (int i = 0; i < _buildingBilling.Count; i++)
        {
            if (go == _buildingBilling[i].gameObject)
            {
                _buildingFlag = true;
                
                _currentLivingArea.BuildingiDataItems[i].OnOpen(_currentLivingArea.BuildingiDataItems[i].OnlyEntity,_currentLivingArea.BuildingiDataItems[i].Id);
                return;
            }
            
        }
    }

    private void BuildingExit()
    {
        _buildingFlag = false;
    }

    private void OpenBuiding(BuildingObject building)
    {

    }

    private void CloseBuiding()
    {

    }



}
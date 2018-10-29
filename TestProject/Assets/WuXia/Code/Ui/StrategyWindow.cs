using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyFrameWork;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 主UI
/// </summary>
public class StrategyWindow : UIWindowBase
{
    public bool IsShowPlayer;
    public bool IsShowTime;
    public bool IsShowMap;
    public bool IsShowMessage;

    [Header("Player")]
    [SerializeField]
    private Text _playerName;
    [SerializeField]
    private Text _playerMoney;
    [SerializeField]
    private Text _playerSW;

    [Header("Time")]
    [SerializeField]
    private Text _year;
    [SerializeField]
    private Text _month;
    [SerializeField]
    private Text _day;
    [SerializeField]
    private  Text _season;
    [SerializeField]
    private  Text _shiChen;

    [Header("MiMap")]
    public Text text6;

    [Header("Message")]
    [SerializeField]
    private RectTransform _messageContent;
    [SerializeField]
    private Text _messageText;


    [Header("Buttom")]
    [SerializeField]
    private Button _characterInformationBtn;
    [SerializeField]
    private Button _wugongBtn;
    [SerializeField]
    private Button _technologyBtn;
    [SerializeField]
    private Button _logBtn;
    [SerializeField]
    private Button _mapBtn;

    protected override void SetWindowId()
    {
        this.ID = WindowID.StrategyWindow;
    }
    protected override void InitWindowCoreData()
    {
        windowData.windowType = UIWindowType.ForegroundLayer;
        windowData.showMode = UIWindowShowMode.DoNothing;
        windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
        windowData.colliderMode = UIWindowColliderMode.None;
        windowData.closeModel = UIWindowCloseModel.Destory;
        windowData.animationType = UIWindowAnimationType.None;
        windowData.playAnimationModel = UIWindowPlayAnimationModel.Stretching;
    }
    public override void InitWindowOnAwake()
    {
        _characterInformationBtn.onClick.AddListener(CharacterInformationClick);
        _wugongBtn.onClick.AddListener(WugongClick);
        _technologyBtn.onClick.AddListener(TechnologyClick);
        _logBtn.onClick.AddListener(LogClick);
        _mapBtn.onClick.AddListener(MapClcik);
    }


    void Update()
    {
        UpdateTime();
    }

    //----日期
    private void UpdateTime()
    {
        _year.text = TimeManager.Instance.curYera.ToString();
        _month.text = TimeManager.Instance.curMonth.ToString();
        _day.text = TimeManager.Instance.curDay.ToString();
        _shiChen.text = TimeManager.Instance.curGd;

        if (TimeManager.Instance.curMonth == 2 || TimeManager.Instance.curMonth == 3 || TimeManager.Instance.curMonth == 4)
        {
            _season.text = "春";
        }
        else if (TimeManager.Instance.curMonth == 5 || TimeManager.Instance.curMonth == 6 || TimeManager.Instance.curMonth == 7)
        {
            _season.text = "夏";
        }
        else if (TimeManager.Instance.curMonth == 8 || TimeManager.Instance.curMonth == 9 || TimeManager.Instance.curMonth == 10)
        {
            _season.text = "秋";
        }
        else if (TimeManager.Instance.curMonth == 11 || TimeManager.Instance.curMonth == 12 || TimeManager.Instance.curMonth == 1)
        {
            _season.text = "冬";
        }
    }
    public void ShowMessage(string message)
    {
        
    }



    //----------Bttom

    private void CharacterInformationClick()
    {
        StrategySceneControl.Instance.OpenWXCharacterPanelWidow();
    }

    private void WugongClick()
    {

    }
    private void TechnologyClick()
    {

    }
    private void LogClick()
    {

    }
    private void MapClcik()
    {
        
    }
}

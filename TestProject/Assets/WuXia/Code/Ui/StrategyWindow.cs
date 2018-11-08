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
        //UpdateTime();
    }

    public void ShowMessage(string message)
    {
        
    }



    //----------Bttom

    private void CharacterInformationClick()
    {
     //   StrategySceneControl.Instance.OpenWXCharacterPanelWidow();
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

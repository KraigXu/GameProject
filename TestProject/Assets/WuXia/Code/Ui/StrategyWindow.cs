﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyFrameWork;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using WX;

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

    }
    protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
    {
        base.BeforeShowWindow(contextData);
        if (contextData != null)
        {
           StrategyWindowInData data = (StrategyWindowInData)contextData;
            _characterInformationBtn.onClick.AddListener(data.CharavterEvent);
            _wugongBtn.onClick.AddListener(data.WugongEvent);
            _technologyBtn.onClick.AddListener(data.TechnologyEvent);
            _logBtn.onClick.AddListener(data.LogEvent);
            _mapBtn.onClick.AddListener(data.MapEvent);
        }
    }

    //protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
    //{
    //    this.BeforeShowWindow(contextData);
    //    if (contextData != null)
    //    {
    //        StrategyWindowInData data = (StrategyWindowInData)contextData;
    //        //_characterInformationBtn.onClick.AddListener(data.CharavterEvent);
    //        //_wugongBtn.onClick.AddListener(data.WugongEvent);
    //        //_technologyBtn.onClick.AddListener(data.TechnologyEvent);
    //        //_logBtn.onClick.AddListener(data.LogEvent);
    //        //_mapBtn.onClick.AddListener(data.MapEvent);
    //    }
    //}
}

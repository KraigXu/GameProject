using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TinyFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class WXCharacterPanelWidow : UIWindowBase
{
    [Header("Character")]
    public Toggle CharacterTog;
    public GameObject CharacterPanel;
    public Text CTRaceType;
    public Text CTName;
    public Text CTDescription;
    public Text CTSex;
    
    [Header("Log")]
    public Toggle LogTog;
    public GameObject LogPanel;

    [Header("Skill")]
    public Toggle SkillTog;
    public GameObject SkillPanel;

    [Header("Technique")]
    public Toggle TechniqueTog;
    public GameObject TechniquePanel;

    [SerializeField]
    private Button _exitBtn;

    [Header("Property")]
    [SerializeField]
    private Text _tizhitxt;
    [SerializeField]
    private Text _lidaotxt;
    [SerializeField]
    private Text _jingshentxt;
    [SerializeField]
    private Text _lingdongtxt;
    [SerializeField]
    private Text _wuxingtxt;

    [SerializeField]
    private Text _jingtxt;
    [SerializeField]
    private Text _qitxt;
    [SerializeField]
    private Text _shentxt;

    protected override void SetWindowId()
    {
        this.ID = WindowID.WXCharacterPanelWindow;
    }

    protected override void InitWindowCoreData()
    {
        windowData.windowType = UIWindowType.ForegroundLayer;
        windowData.showMode = UIWindowShowMode.DoNothing;
        windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
        windowData.colliderMode = UIWindowColliderMode.None;
        windowData.closeModel = UIWindowCloseModel.Destory;
    }

    public override void InitWindowOnAwake()
    {
        _exitBtn.onClick.AddListener(Exit);
    }

    protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
    {
        base.BeforeShowWindow(contextData);
        if (contextData != null)
        {
            BiologicalUiInData data = (BiologicalUiInData)contextData;
            _tizhitxt.text = data.Tizhi.ToString();
            _lidaotxt.text = data.Lidao.ToString();
            _jingshentxt.text = data.Jingshen.ToString();
            _lingdongtxt.text = data.Lingdong.ToString();
            _wuxingtxt.text = data.Wuxing.ToString();
            _jingtxt.text = data.Jing.ToString();
            _qitxt.text = data.Qi.ToString();
            _shentxt.text = data.Shen.ToString();

        }

    }

    private void CharacterTogMain(bool flag)
    {
        CharacterPanel.SetActive(flag);
    }

    private void LogTogMain(bool flag)
    {
        LogPanel.SetActive(flag);
    }

    private void SkillTogMain(bool flag)
    {
        SkillPanel.SetActive(flag);
    }

    private void TechniqueTogMain(bool flag)
    {
        TechniquePanel.SetActive(flag);
        //if (string.IsNullOrEmpty(value)==false)
        //{
        //}
    }

    private void Exit()
    {
        UICenterMasterManager.Instance.CloseWindow(this.ID);
    }
}

using System.Collections;
using System.Collections.Generic;
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
        windowData.animationType = UIWindowAnimationType.None;
    }

    public override void InitWindowOnAwake()
    {
        //CharacterTog.onValueChanged.AddListener(CharacterTogMain);
        //LogTog.onValueChanged.AddListener(LogTogMain);
        //SkillTog.onValueChanged.AddListener(SkillTogMain);
        //TechniqueTog.onValueChanged.AddListener(TechniqueTogMain);
    }

    protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
    {
        base.BeforeShowWindow(contextData);
    }

    private void CharacterTogMain(bool flag)
    {
        CharacterPanel.SetActive(flag);
    }

    ///// <summary>
    ///// 参数写至面板
    ///// </summary>
    ///// <param name="propertys"></param>
    //public void ChanageCharacterPanel(Biological biological)
    //{
    //  //  this.CTName.text = biological.Model.Name;
    //   // this.CTDescription.text = biological.Model.Description;
    //   // this.CTRaceType.text = biological.Model.RaceType.ToString();
    //    this.CTSex.text = "1";

    //}


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
    }

}

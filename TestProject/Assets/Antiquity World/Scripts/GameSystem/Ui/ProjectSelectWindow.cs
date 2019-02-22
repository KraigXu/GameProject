using System.Collections;
using System.Collections.Generic;
using GameSystem.Ui;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// 选择项目界面
/// </summary>
public class ProjectSelectWindow : UIWindowBase
{
    public Button StartBtn;

    private class  ProjectInfo
    {
        public int Id;
        public string Name;
        public string StartTime;
        public string EndTime;
    }
    protected override void InitWindowData()
    {
        this.ID = WindowID.StrategyWindow;

        this.windowData.windowType = UIWindowType.NormalLayer;
        this.windowData.showMode = UIWindowShowMode.DoNothing;
        this.windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
        this.windowData.colliderMode = UIWindowColliderMode.None;
        this.windowData.closeModel = UIWindowCloseModel.Destory;
        this.windowData.animationType = UIWindowAnimationType.None;
        this.windowData.playAnimationModel = UIWindowPlayAnimationModel.Stretching;
    }


    public override void InitWindowOnAwake()
    {
        StartBtn.onClick.AddListener(StartMain);

    }

    private void StartMain()
    {
        SceneManager.LoadScene(1);
    }

}

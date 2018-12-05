using System.Collections;
using System.Collections.Generic;
using GameSystem.Ui;
using UnityEngine;

public class TeamWindow : UIWindowBase
{
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

    protected override void SetWindowId()
    {
        this.ID = WindowID.TeamWindow;
    }

    public override void InitWindowOnAwake()
    {
       

    }


}

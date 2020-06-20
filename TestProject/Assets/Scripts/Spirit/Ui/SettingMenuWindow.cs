using System.Collections;
using System.Collections.Generic;
using GameSystem.Ui;
using UnityEngine;

public class SettingMenuWindow : UIWindowBase
{

    protected override void InitWindowData()
    {
        this.ID = WindowID.SettingMenuWindow;

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
        
    }
}

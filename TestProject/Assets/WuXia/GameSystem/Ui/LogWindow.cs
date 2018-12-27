using System.Collections;
using System.Collections.Generic;
using GameSystem.Ui;
using UnityEngine;

public class LogWindow : UIWindowBase
{

    protected override void InitWindowData()
    {
        this.ID = WindowID.LogWindow;

        windowData.windowType = UIWindowType.NormalLayer;
        windowData.showMode = UIWindowShowMode.DoNothing;
        windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
        windowData.colliderMode = UIWindowColliderMode.None;
        windowData.closeModel = UIWindowCloseModel.Destory;
        windowData.animationType = UIWindowAnimationType.None;
    }
    public override void InitWindowOnAwake()
    {
    }


}

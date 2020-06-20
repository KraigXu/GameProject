using System.Collections;
using System.Collections.Generic;
using GameSystem.Ui;
using UnityEngine;

public class CombatReadyWindow : UIWindowBase
{

    protected override void InitWindowData()
    {
        this.ID = WindowID.CombatReadyWindow;

        windowData.windowType = UIWindowType.ForegroundLayer;
        windowData.showMode = UIWindowShowMode.DoNothing;
        windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
        windowData.colliderMode = UIWindowColliderMode.None;
        windowData.closeModel = UIWindowCloseModel.Destory;
    }

    public override void InitWindowOnAwake()
    {

    }

}

using System.Collections;
using System.Collections.Generic;
using TinyFrameWork;
using UnityEngine;

public class MapWindow : UIWindowBase
{
    

    public override void InitWindowOnAwake()
    {

    }

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
        this.ID = WindowID.MapWindow;
    }

}
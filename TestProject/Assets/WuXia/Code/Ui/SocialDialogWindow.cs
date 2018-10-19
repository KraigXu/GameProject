using System.Collections;
using System.Collections.Generic;
using TinyFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class SocialDialogWindow : UIWindowBase
{

    protected override void SetWindowId()
    {
        this.ID = WindowID.SocialDialogWindow;
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

    public override void InitWindowOnAwake()
    {

    }
}

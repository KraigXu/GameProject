using System.Collections;
using System.Collections.Generic;
using TinyFrameWork;
using UnityEngine;


public class LivingAreaMainWindow : UIWindowBase
{
    
    protected override void SetWindowId()
    {
        this.ID = WindowID.LivingAreaMainWindow;
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

    protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
    {
        WindowContextLivingAreaNodeData nodeData = (WindowContextLivingAreaNodeData)contextData;
        if(nodeData==null) return;
        //resolve LivingArea Data


        //resolve Building Data , building图生成




        
    }

    
}


using System.Collections;
using System.Collections.Generic;
using LivingArea;
using TinyFrameWork;
using UnityEngine;

/// <summary>
/// 这个Target 与Game 中的Target 对应
/// </summary>
public enum ElementTarget
{
    LivingArea
}

/// <summary>
/// 视图状态
/// </summary>
public enum ViewStatus
{
    CityMainView, 
    WorldMapView,

}

public class StrategySceneControl : MonoBehaviour {

    public static StrategySceneControl Instance
    {
        get
        {
            return _instance;
        }
    }
    private static StrategySceneControl _instance = null;

    public Camera Main3DCamera;
    public Camera Main2DCamera;

    public MousePointingInfo MousePointingControl;           //鼠标信息控制


    //----常在UI
    public ExtendedMenuWindow ExtendedMenuControl;

    //---Info
    public ViewStatus CurViewStatus; 
    public LivingAreaNode LivingAreaTarget;
    

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {

        MousePointingControl.MouseEnterEvents.Add("Player", MouseEnter_PlayerMain);
        MousePointingControl.MouseExitEvents.Add("Player", MouseExit_PlayerMain);
        MousePointingControl.MouseOverEvents.Add("Player", MouseOver_PlayerMain);
        MousePointingControl.Mouse0ClickEvents.Add("Player", Mouse0Click_PlayerMain);
        MousePointingControl.Mouse1ClickEvents.Add("Player", Mouse1Click_PlayerMain);

        MousePointingControl.MouseEnterEvents.Add("LivingArea", MouseEnter_LivingAreaMain);
        MousePointingControl.MouseExitEvents.Add("LivingArea", MouseExit_LivingAreaMain);
        MousePointingControl.MouseOverEvents.Add("LivingArea", MouseOver_LivingAreaMain);
        MousePointingControl.Mouse0ClickEvents.Add("LivingArea", Mouse0Click_LivingAreaMain);
        MousePointingControl.Mouse1ClickEvents.Add("LivingArea", Mouse1Click_LivingAreaMain);
   }



    void Update()
    {

    }

    #region ViewStatusChange


    public void WorldMapViewToCityMainView()
    {
        
        ShowWindowData data=new ShowWindowData();
        data.contextData=new WindowContextLivingAreaNodeData(LivingAreaTarget);
        UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, data);
        
    }

    public void CityMainViewToWorldMapView()
    {
        UICenterMasterManager.Instance.CloseWindow(WindowID.LivingAreaMainWindow);  
    }

    #endregion

    #region InitModel
    #endregion 


    #region MouseEvents
    public void MouseEnter_PlayerMain(Transform tf)
    {
        Debug.Log(tf.name + ">>MouseEnter");
    }
    public void MouseExit_PlayerMain(Transform tf)
    {
        Debug.Log(tf.name + ">>MouseExit");
    }
    public void MouseOver_PlayerMain(Transform tf)
    {
        Debug.Log(tf.name + ">> MouseOver");
    }
    public void Mouse0Click_PlayerMain(Transform tf, Vector3 point)
    {
        Debug.Log(tf.name + ">>Mouse0Click");
    }
    public void Mouse1Click_PlayerMain(Transform tf, Vector3 point)
    {
        Debug.Log(tf.name + ">>Mouse1Click");
    }

    public void MouseEnter_LivingAreaMain(Transform tf)
    {
        Debug.Log(tf.name + ">>MouseEnter");


    }
    public void MouseExit_LivingAreaMain(Transform tf)
    {
        Debug.Log(tf.name + ">>MouseExit");
    }
    public void MouseOver_LivingAreaMain(Transform tf)
    {
        Debug.Log(tf.name + ">> MouseOver");
    }
    public void Mouse0Click_LivingAreaMain(Transform tf, Vector3 point)
    {
        Debug.Log(tf.name + ">>Mouse0Click");


    }
    public void Mouse1Click_LivingAreaMain(Transform tf, Vector3 point)
    {
        Debug.Log(tf.name + ">>Mouse1Click");

        ShowWindowData showMenuData=new ShowWindowData();
        showMenuData.contextData=new WindowContextExtendedMenu(tf.GetComponent<LivingAreaNode>(),point);
        UICenterMasterManager.Instance.ShowWindow(WindowID.ExtendedMenuWindow, showMenuData);


    }



    #endregion 
}

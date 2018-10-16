using System.Collections;
using System.Collections.Generic;
using LivingArea;
using TinyFrameWork;
using UnityEngine;
using MapMagicDemo;
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

    public Camera Main3DCamera;                                                                             //大地图相机
    public Camera Main2DCamera;                                                                             //UI相机
    public Camera LivingfAreaCamera;                                                                        //生活区相机

    //----CamerControl
    public OverLookCameraController LivingfAreaCameraControl;

    public MousePointingInfo MousePointingControl;           //鼠标信息控制

    //----常在UI
    public StrategyWindow StrategyControl;
    public ExtendedMenuWindow ExtendedMenuControl;


    //---Info
    public ViewStatus CurViewStatus; 
    public LivingAreaNode LivingAreaTarget;
    
    //---Manager          --m前缀
    public LivingAreaManager M_LivingArea;

    public CharController charController;
    public CameraController cameraController;
    public FlybyController demoController;

    void Awake()
    {
        _instance = this;
        Debuger.EnableLog = true;
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


        //Ui 初始化
        StrategyControl= UICenterMasterManager.Instance.ShowWindow(WindowID.StrategyWindow).GetComponent<StrategyWindow>();
        ExtendedMenuControl=UICenterMasterManager.Instance.ShowWindow(WindowID.ExtendedMenuWindow).GetComponent<ExtendedMenuWindow>();


        charController.enabled = true;
        charController.gravity = false;
        charController.speed = 50;
        charController.acceleration = 150;
        demoController.enabled = false;
        cameraController.follow = 0;

    }



    void Update()
    {

    }

    #region 生活区

    /// <summary>
    /// 进入生活区
    /// </summary>
    public void LivingAreaEnter(LivingAreaNode livingArea)
    {
        if (livingArea == null)
        {
            Debuger.LogError("进入生活区时，数据为NULL");
            return;
        }

        
        //先对角色的属性进行检测 是否可以进入城市，1，角色位置是否在城市附近，2 角色与城市势力关系 3角色与城市首领关系 4城市的异常状态 


        //关闭当前开启的界面 ---
       
        UICenterMasterManager.Instance.CloseWindow(WindowID.LivingAreaBasicWindow);


        ShowWindowData showWindowData = new ShowWindowData();
        showWindowData.contextData = new WindowContextLivingAreaNodeData(livingArea);
        UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, showWindowData);

        //初始化LivingArea

    //    StaticValue.Instance.EnterLivingAreaId = livingArea.Id;


        //更新相机
        Renderer[] renderers= livingArea.LivingAreaM.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            Bounds mapBounds = renderers[0].bounds;
            for (int i = 0; i < renderers.Length; i++)
            {
                mapBounds.Encapsulate(renderers[i].bounds);
            }
            LivingfAreaCameraControl.SetBounds(mapBounds);
            LivingfAreaCameraControl.SetPosition(mapBounds.center);
        }
        //  M_LivingArea.EnterLivingArea(livingArea);

    }



    #endregion


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
    
    #region Message

    //显示Message
    public void MessageShow(string[] values)
    {
    }

    public void MessageShow(string value)
    {
        
    }


    #endregion
}

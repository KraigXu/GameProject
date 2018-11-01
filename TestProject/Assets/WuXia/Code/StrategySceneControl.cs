using System.Collections;
using System.Collections.Generic;
using LivingArea;
using TinyFrameWork;
using UnityEngine;
using MapMagicDemo;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

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

    public StrategyMainCamera Main3DCamera;                                                                    //大地图相机                                                                               
    public Camera Main2DCamera;                                                                                //UI相机
    public Camera LivingfAreaCamera;                                                                           //生活区相机
    public Camera Cur3DMainCamera;
    //----CamerControl
    public OverLookCameraController LivingfAreaCameraControl;
    public MousePointingInfo MousePointing;           //鼠标信息控制

    //----常在UI
    public StrategyWindow StrategyControl;
    public ExtendedMenuWindow ExtendedMenuControl;
    public SocialDialogWindow SocialDialogControl;

    //---Info
    public ViewStatus CurViewStatus; 
    public LivingAreaNode LivingAreaTarget;
    
    //---Manager          --m前缀
    public TimeManager M_Time;
    public BiologicalManager M_Biological;
    public StrategyManager M_Strategy;
    public FactionManager M_Faction;
    //----Player
    /// <summary>
    /// 记录当前进入的LivingArea，如果没有则为-1
    /// </summary>
    public int EnterLivingAreaId = -1;
    public Biological CurPlayer;
    public GameObject CurMouseEffect;

    [SerializeField]
    private Transform _livingAreasSelect;

    void Awake()
    {
        _instance = this;
        Debuger.EnableLog = true;
    }

    void Start()
    {
        StrategyInit();
        BiologicalInit();
        PlayerDataInit();
        UiInit();
        OverInit();
    }

    #region 1：省份区域数据获取

    public void StrategyInit()
    {
        M_Strategy.InitStrategyData();
       
    }
    #endregion
    #region 2:

    public void BiologicalInit()
    {
        M_Biological.InitBiological();
    }
    #endregion
    #region 3:
    #endregion
    #region 4:
    #endregion
    #region 5:
    #endregion
    #region 6:
    #endregion
    #region 7:
    #endregion
    #region 8:
    #endregion
    #region 9:

    public void PlayerDataInit()
    {
      //  CurPlayer = M_Biological.GetPlayer(Define.Value.PlayerId);  //选择角色
    }

    #endregion
    #region 10:

    public void UiInit()
    {
        UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaTitleWindow);

        StrategyControl = UICenterMasterManager.Instance.ShowWindow(WindowID.StrategyWindow).GetComponent<StrategyWindow>();
    }

    #endregion

    #region 11:over Init

    public void OverInit()
    {
        MousePointing.MouseEnterEvents.Add("Player", MouseEnter_PlayerMain);
        MousePointing.MouseExitEvents.Add("Player", MouseExit_PlayerMain);
        MousePointing.MouseOverEvents.Add("Player", MouseOver_PlayerMain);
        MousePointing.Mouse0ClickEvents.Add("Player", Mouse0Click_PlayerMain);
        MousePointing.Mouse1ClickEvents.Add("Player", Mouse1Click_PlayerMain);

        MousePointing.MouseEnterEvents.Add("LivingArea", MouseEnter_LivingAreaMain);
        MousePointing.MouseExitEvents.Add("LivingArea", MouseExit_LivingAreaMain);
        MousePointing.MouseOverEvents.Add("LivingArea", MouseOver_LivingAreaMain);
        MousePointing.Mouse0ClickEvents.Add("LivingArea", Mouse0Click_LivingAreaMain);
        MousePointing.Mouse1ClickEvents.Add("LivingArea", Mouse1Click_LivingAreaMain);

        MousePointing.MouseEnterEvents.Add("Terrain", MouseEnter_Terrain);
        MousePointing.MouseExitEvents.Add("Terrain", MouseExit_Terrain);
        MousePointing.MouseOverEvents.Add("Terrain", MouseOver_Terrain);
        MousePointing.Mouse0ClickEvents.Add("Terrain", Mouse0Click_Terrain);
        MousePointing.Mouse1ClickEvents.Add("Terrain", Mouse1Click_Terrain);

        MousePointing.MouseEnterEvents.Add("Biological", MouseEnter_Biological);
        MousePointing.MouseExitEvents.Add("Biological", MouseExit_Biological);
        MousePointing.MouseOverEvents.Add("Biological", MouseOver_Biological);
        MousePointing.Mouse0ClickEvents.Add("Biological", Mouse0Click_Biological);
        MousePointing.Mouse1ClickEvents.Add("Biological", Mouse1Click_Biological);

        MousePointing.enabled = true;
        MousePointing.gravity = false;
        MousePointing.speed = 50;
        MousePointing.acceleration = 150;
        MousePointing.follow = 0;

        Cur3DMainCamera= Camera.main;
    }


    #endregion

    #region LivingArea

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
    public void MouseOver_LivingAreaMain(Transform target)
    {
        Debug.Log(target.name + ">> MouseOver");
    }
    public void Mouse0Click_LivingAreaMain(Transform target, Vector3 point)
    {
        Debug.Log(target.name + ">>Mouse0Click");
        LivingAreaNode node = target.GetComponent<LivingAreaNode>();
        _livingAreasSelect.position = node.LivingAreaRender.bounds.center;
        //  MessageBoxInstance.Instance.MessageBoxShow("");

        //判断逻辑

        if (CurPlayer != null)
        {
            Debuger.Log("Enter LivingAreas");
            CurPlayer.transform.position = node.transform.position;
            M_Strategy.InstanceLivingArea(node);

            ShowWindowData showWindowData=new ShowWindowData();
            showWindowData.contextData=new WindowContextLivingAreaNodeData(node);
            UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, showWindowData);

            if (CurPlayer.GroupId == -1)
            {
                M_Strategy.EnterLivingAreas(node, CurPlayer);
            }
            else
            {
                M_Strategy.EnterLivingAreas(node,M_Biological.GroupsDic[CurPlayer.GroupId].Partners);
            }
        }
    }
    public void Mouse1Click_LivingAreaMain(Transform target, Vector3 point)
    {
        Debug.Log(target.name + ">>Mouse1Click");

        ShowWindowData showMenuData=new ShowWindowData();
        showMenuData.contextData=new WindowContextExtendedMenu(target.GetComponent<LivingAreaNode>(),point);
        UICenterMasterManager.Instance.ShowWindow(WindowID.ExtendedMenuWindow, showMenuData);
    }

    public void MouseEnter_Terrain(Transform tf)
    {
    }

    public void MouseExit_Terrain(Transform tf)
    {

    }

    public void MouseOver_Terrain(Transform tf)
    {

    }

    public void Mouse0Click_Terrain(Transform tf, Vector3 point)
    {
        CurMouseEffect.transform.position = point;
    }

    public void Mouse1Click_Terrain(Transform tf, Vector3 point)
    {
        Debuger.Log(">>>>>>>Terrain");
        CurMouseEffect.transform.position = point;
        CurPlayer.GetComponent<AICharacterControl>().SetTarget(CurMouseEffect.transform);
        NavMeshAgent agent = CurPlayer.GetComponent<NavMeshAgent>();
        LineRenderer moveLine = gameObject.GetComponent<LineRenderer>();
        if (moveLine == null)
        {
            moveLine = gameObject.AddComponent<LineRenderer>();
        }

        //设置路径的点，
        //路径  导航。
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(point, path);
        //线性渲染设置拐点的个数。数组类型的。
        moveLine.positionCount = path.corners.Length;
        //线性渲染的拐点位置，数组类型，
        agent.SetDestination(point);
        moveLine.SetPositions(path.corners);
    }

    public void MouseEnter_Biological(Transform tf)
    {

    }

    public void MouseExit_Biological(Transform tf)
    {
      
    }

    public void MouseOver_Biological(Transform tf)
    {
      
    }

    public void Mouse0Click_Biological(Transform tf, Vector3 point)
    {

    }

    public void Mouse1Click_Biological(Transform tf, Vector3 point)
    {

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


    #region UiOpen

    public void OpenWXCharacterPanelWidow()
    {
        UICenterMasterManager.Instance.ShowWindow(WindowID.WXCharacterPanelWindow);
    }
    #endregion
}

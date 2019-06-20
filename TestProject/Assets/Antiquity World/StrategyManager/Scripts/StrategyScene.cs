using System.Collections;
using System.Collections.Generic;
using GameSystem;
using GameSystem.Ui;
using Manager;
using Unity.Entities;
using UnityEngine;

public class StrategyScene : MonoBehaviour
{
    private static StrategyScene _instance;

    public static StrategyScene Instance
    {
        get { return _instance; }
    }

    public bool IsInitOver = false;

    public StrategyPlayer Player;


    //---------Map
    public HexGrid hexGrid;
    public HexMapGenerator mapGenerator;
   
    //---------Message
    public Canvas messageCanvas;
    public LoadingView loadingViewCom;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        StartCoroutine(StrategySceneInit());
    }

    IEnumerator StrategySceneInit()
    {

        loadingViewCom.Open();

        GameSceneInit.InitializeWithScene();

        InitMapInfo();

        InitGameSystem();

        InitStartUi();

        InitCamera();

        loadingViewCom.Close();

        yield return null;
    }


    void InitMapInfo()
    {
        
#if UNITY_EDITOR
        GameSceneInit.CurOpeningInfo.TestValue();
#endif

        OpeningInfo openingInfo = GameSceneInit.CurOpeningInfo;

        hexGrid.seed = openingInfo.Mapseed;

        if (openingInfo.GenerateMaps)
        {
            mapGenerator.GenerateMap(openingInfo.Mapx, openingInfo.Mapz, openingInfo.Wrapping);
        }
        else
        {
            hexGrid.CreateMap(openingInfo.Mapx, openingInfo.Mapz, openingInfo.Wrapping);
        }
        HexMapCamera.ValidatePosition();

    }

    void InitGameSystem()
    {
        var entityManager = World.Active.GetOrCreateManager<EntityManager>();

        SystemManager.Get<ArticleSystem>().SetupComponentData(entityManager);

        SystemManager.Get<LivingAreaSystem>().SetupComponentData(entityManager);

        SystemManager.Get<DistrictSystem>().SetupComponentData(entityManager);

        SystemManager.Get<BiologicalSystem>().SetupComponentData(entityManager);

        SystemManager.Get<TechniquesSystem>().SetupComponentData(entityManager);

        SystemManager.Get<RelationSystem>().SetupComponentData(entityManager);

        SystemManager.Get<SocialDialogSystem>().SetupComponentData(entityManager);

        SystemManager.Get<PrestigeSystem>().SetupComponentData(entityManager);

        SystemManager.Get<RelationSystem>().SetupComponentData(entityManager);

        SystemManager.Get<FactionSystem>().SetupComponentData(entityManager);

        SystemManager.Get<FamilySystem>().SetupComponentData(entityManager);

    }


    /// <summary>
    /// 生成开局UI
    /// </summary>
    void InitStartUi()
    {
        UICenterMasterManager.Instance.ShowWindow(WindowID.WorldTimeWindow);

        UICenterMasterManager.Instance.ShowWindow(WindowID.MenuWindow);

        UICenterMasterManager.Instance.ShowWindow(WindowID.PlayerInfoWindow);

        UICenterMasterManager.Instance.ShowWindow(WindowID.MessageWindow);

        UICenterMasterManager.Instance.ShowWindow(WindowID.MapWindow);

        UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaTitleWindow);

    }

    void InitCamera()
    {
       // StrategyCameraManager.Instance.SetTarget(new Vector3(-54.42019f, 50.3085f, 40.11046f));
    }

    /// <summary>
    /// 删除开局UI
    /// </summary>
    public void RemoveStartUi()
    {
        UICenterMasterManager.Instance.DestroyWindow(WindowID.WorldTimeWindow);
        UICenterMasterManager.Instance.DestroyWindow(WindowID.MenuWindow);
        UICenterMasterManager.Instance.DestroyWindow(WindowID.PlayerInfoWindow);
        UICenterMasterManager.Instance.DestroyWindow(WindowID.MessageWindow);
        UICenterMasterManager.Instance.DestroyWindow(WindowID.MapWindow);
        UICenterMasterManager.Instance.DestroyWindow(WindowID.LivingAreaTitleWindow);

    }
}

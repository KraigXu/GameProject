using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    public int PlayerId = 1;
    public Camera MainCamera;
    public Camera FixedCamera;
    public Camera UiCamera;
    public GameObject go;
  
    public GameObject ArticleInfoPerfab;

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

#if UNITY_EDITOR
        Debuger.EnableLog = true;
        GameSceneInit.CurOpeningInfo.TestValue();
#endif
        GameSceneInit.InitializeWithScene();

        InitGameSystem();
        InitMapInfo();
        InitPlayer();
        InitStartUi();
        InitCamera();

        loadingViewCom.Close();

        yield return null;
    }


    void InitMapInfo()
    {
        
        OpeningInfo openingInfo = GameSceneInit.CurOpeningInfo;

        hexGrid.seed = openingInfo.Mapseed;

        if (openingInfo.IsEditMode == true)
        {
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
        else
        {
            //加载地图
            Debuger.Log(openingInfo.MapFilePath);
            using (BinaryReader reader = new BinaryReader(File.OpenRead(openingInfo.MapFilePath)))
            {
                int header = reader.ReadInt32();
                if (header <= openingInfo.MapFileVersion)
                {
                    hexGrid.Load(reader, header);
                    HexMapCamera.ValidatePosition();
                }
                else
                {
                    Debug.LogWarning("Unknown map format " + header);
                }
            }
        }

        

    }

    void InitPlayer()
    {
        //OpeningInfo openingInfo = GameSceneInit.CurOpeningInfo;

        




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
        //StrategyCameraManager.Instance.SetTarget(new Vector3(-54.42019f, 50.3085f, 40.11046f));
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

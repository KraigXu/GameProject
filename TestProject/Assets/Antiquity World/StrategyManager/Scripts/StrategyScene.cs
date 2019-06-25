using System.Collections;
using System.Collections.Generic;
using System.IO;
using DataAccessObject;
using GameSystem;
using GameSystem.Ui;
using Manager;
using Unity.Entities;
using Unity.Mathematics;
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

        InitMapInfo();
        InitGameSystem();
       
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
        HexCoordinates hexCoordinates;
        HexCell hexCell;


        //------------初始化Living
        List<LivingAreaData> datas = SQLService.Instance.QueryAll<LivingAreaData>();
        for (int i = 0; i < datas.Count; i++)
        {
            var data = datas[i];
            hexCoordinates = new HexCoordinates(data.PositionX, data.PositionZ);

            hexCell = hexGrid.GetCell(hexCoordinates);
            hexCell.SpecialIndex = data.SpecialIndex;

            switch (data.SpecialIndex)
            {
                case 1:
                    SystemManager.Get<CitySystem>().AddCity(data, hexCoordinates);
                    break;
                case 2:
                    SystemManager.Get<OrganizationSystem>().AddOrganization(data, hexCoordinates);
                    break;
                default:
                    SystemManager.Get<CitySystem>().AddCity(data, hexCoordinates);
                    break;
            }
            GameStaticData.LivingAreaName.Add(datas[i].Id, datas[i].Name);
            GameStaticData.LivingAreaDescription.Add(datas[i].Id, datas[i].Description);
        }


        //------------------初始化Biological
        List<BiologicalAvatarData> biologicalAvatarDatas = SQLService.Instance.QueryAll<BiologicalAvatarData>();
        for (int i = 0; i < biologicalAvatarDatas.Count; i++)
        {
            GameStaticData.BiologicalAvatar.Add(biologicalAvatarDatas[i].Id, Resources.Load<Sprite>(biologicalAvatarDatas[i].Path));
        }

        List<BiologicalModelData> biologicalModelDatas = SQLService.Instance.QueryAll<BiologicalModelData>();
        for (int i = 0; i < biologicalModelDatas.Count; i++)
        {
            GameStaticData.BiologicalPrefab.Add(biologicalModelDatas[i].Id, Resources.Load<GameObject>(biologicalModelDatas[i].Path));
        }

        List<BiologicalData> biologicalDatas = SQLService.Instance.QueryAll<BiologicalData>();
        BiologicalData bData;
        for (int i = 0; i < biologicalDatas.Count; i++)
        {
            bData = biologicalDatas[i];

            Transform entityGo = WXPoolManager.Pools[Define.GeneratedPool].Spawn(GameStaticData.BiologicalPrefab[bData.ModelId].transform);

            HexUnit hexUnit = entityGo.GetComponent<HexUnit>();
            hexCoordinates= new HexCoordinates(bData.X, bData.Z);
            hexCell = hexGrid.GetCell(hexCoordinates);

            hexGrid.AddUnit(hexUnit, hexCell,UnityEngine.Random.Range(0f, 360f));

            switch (bData.Identity)
            {
                case 1:
                    SystemManager.Get<BiologicalSystem>().AddBiological(bData, hexUnit.GetComponent<GameObjectEntity>().Entity);
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                default:
                    break;
            }
        }




        SystemManager.Get<ArticleSystem>().SetupComponentData(entityManager);

        SystemManager.Get<DistrictSystem>().SetupComponentData(entityManager);

       // SystemManager.Get<BiologicalSystem>().SetupComponentData(entityManager);

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


    public void SpawnRandomBiological(HexCell cell)
    {
        HexUnit hexUnit = Instantiate(HexUnit.unitPrefab);
        hexGrid.AddUnit(hexUnit, cell, UnityEngine.Random.Range(0f, 360f));
        BiologicalSystem.SpawnRandomBiological(hexUnit.transform);
    }
}

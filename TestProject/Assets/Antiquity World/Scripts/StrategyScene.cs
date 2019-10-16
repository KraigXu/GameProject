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

public enum StrategySceneModel
{
    Map,
    LivingArea,
    Fighting
}

/// <summary>
/// StrategyScene主脚本，控制整个场景的的生命周期
/// </summary>
public class StrategyScene : MonoBehaviour
{
    private static StrategyScene _instance;

    public static StrategyScene Instance
    {
        get { return _instance; }
    }

    public Camera MainCamera;
    public Camera BuildCamera;

    public GameObject RunTimeUI;
    public GameObject FixedUI;
    public GameObject EditUI;

    public bool IsEdit = false;


    //---------Map
    public HexGrid hexGrid;
    public HexMapGenerator mapGenerator;

    //---------Message
    public Canvas messageCanvas;
    public LoadingView loadingViewCom;

    public IEnumeratorLoad IeEnumeratorLoad;

    public StrategySceneModel SceneModel = StrategySceneModel.Map;


    private EntityManager entityManager;
    private HexCoordinates hexCoordinates;
    private HexCell hexCell;


    void Awake()
    {
        _instance = this;
    }

    void Start()
    {

        GameSceneInit.InitializeWithScene();

#if UNITY_EDITOR
        Debuger.EnableLog = true;
        GameSceneInit.CurOpeningInfo.TestValue();

#endif

        OpeningInfo openingInfo = GameSceneInit.CurOpeningInfo;

        if (openingInfo.IsEditMode == false)
        {
            entityManager = World.Active.GetOrCreateManager<EntityManager>();

            //初始地图信息
            IeEnumeratorLoad.AddIEnumerator(InitMapInfo(openingInfo.MapFilePath, openingInfo.MapFileVersion, openingInfo.Mapseed));
            //初始生物信息
            IeEnumeratorLoad.AddIEnumerator(InitBiologicalData());
            //初始玩家信息
            IeEnumeratorLoad.AddIEnumerator(InitPlayerData());
        }
        else
        {
            IeEnumeratorLoad.AddIEnumerator(InitEdit(openingInfo));
        }

    }
    #region  正常开始流程

    IEnumerator InitMapInfo(string filePath, int version, int seed)
    {
        hexGrid.seed = seed;
        //加载地图
        using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath)))
        {
            int header = reader.ReadInt32();
            if (header <= version)
            {
                hexGrid.Load(reader, header);
                HexMapCamera.ValidatePosition();
            }
            else
            {
                Debug.LogWarning("Unknown map format " + header);
            }
        }

        yield return new WaitForFixedUpdate();

        yield return ModelController.Instance.ReadModelFileData();

        List<LivingAreaData> datas = SQLService.Instance.QueryAll<LivingAreaData>();
        for (int i = 0; i < datas.Count; i++)
        {
            var data = datas[i];
            hexCoordinates = new HexCoordinates(data.PositionX, data.PositionZ);

            hexCell = hexGrid.GetCell(hexCoordinates);
            if (hexCell == null)
                continue;
            hexCell.SpecialIndex = data.SpecialIndex;

            switch (data.SpecialIndex)
            {
                case 1:
                    CitySystem.AddCity(entityManager, data, hexCell);
                    break;
                case 2:
                    OrganizationSystem.AddOrganization(entityManager, data, hexCell);
                    break;
                case 3:
                    ZigguratSystem.AddZiggurat(entityManager,data, hexCell);
                    break;
                default:
                    Debug.Log( string.Format("{0}功能尚未完善",data.SpecialIndex));
                    break;
            }
        }
    }

    /// <summary>
    /// 初始化生物数据
    /// </summary>
    IEnumerator InitBiologicalData()
    {
        //------------------初始化Biological
        List<BiologicalData> biologicalDatas = SQLService.Instance.QueryAll<BiologicalData>();
        BiologicalData bData;
        for (int i = 0; i < biologicalDatas.Count; i++)
        {
            bData = biologicalDatas[i];
            hexCoordinates = new HexCoordinates(bData.X, bData.Z);
            hexCell = hexGrid.GetCell(hexCoordinates);
            if (hexCell == null)
                continue;

            HexUnit hexUnit = Instantiate(StrategyAssetManager.GetHexUnitPrefabs(bData.ModelId));
            hexGrid.AddUnit(hexUnit, hexCell, UnityEngine.Random.Range(0f, 360f));
            Entity entity = hexUnit.GetComponent<GameObjectEntity>().Entity;

            switch (bData.Identity)
            {
                case 0:
                case 1:
                case 2:
                default:
                    SystemManager.Get<BiologicalSystem>().AddBiological(bData, entity);
                    SystemManager.Get<EquipmentSystem>().AddEquipment(entity, bData.EquipmentJson);
                    SystemManager.Get<ArticleSystem>().SettingArticleFeature(entity, bData.Id);
                    SystemManager.Get<TechniquesSystem>().SpawnTechnique(entity, bData.Id);

                    break;
            }
        }



        //SystemManager.Get<DistrictSystem>().SetupComponentData(entityManager);
        //SystemManager.Get<RelationSystem>().SetupComponentData(entityManager);
        //SystemManager.Get<SocialDialogSystem>().SetupComponentData(entityManager);
        //SystemManager.Get<PrestigeSystem>().SetupComponentData(entityManager);
        //SystemManager.Get<FamilySystem>().SetupComponentData(entityManager);

        yield return new WaitForFixedUpdate();

      //  EntityManager entityManager = SystemManager.ActiveManager;

        FactionSystem.SetupData();
        //LivingAreaSystem.SetupComponentData(entityManager, hexGrid);

        //SystemManager.Get<WorldTimeSystem>().SetupValue(true);

    }

    IEnumerator InitPlayerData()
    {

        //------------------初始化玩家模板
        List<PlayerData> playerDatas = SQLService.Instance.QueryAll<PlayerData>();
        PlayerData pData;
        for (int i = 0; i < playerDatas.Count; i++)
        {
            if (playerDatas[i].Identity == 1)
            {
                pData = playerDatas[i];
                hexCoordinates = new HexCoordinates(pData.X, pData.Z);
                hexCell = hexGrid.GetCell(hexCoordinates);
                if (hexCell == null)
                    continue;

                HexUnit hexUnit = Instantiate(StrategyAssetManager.GetHexUnitPrefabs(pData.ModelId));
                hexGrid.AddUnit(hexUnit, hexCell, UnityEngine.Random.Range(0f, 360f));
                Entity entity = hexUnit.GetComponent<GameObjectEntity>().Entity;

                switch (pData.Identity)
                {
                    case 0:
                    case 1:
                    case 2:
                    default:
                        SystemManager.Get<BiologicalSystem>().AddBiological(pData, entity);
                        SystemManager.Get<EquipmentSystem>().AddEquipment(entity, pData.EquipmentJson);
                        SystemManager.Get<ArticleSystem>().SettingArticleFeature(entity, pData.Id);
                        SystemManager.Get<TechniquesSystem>().SpawnTechnique(entity, pData.Id);
                        StrategyPlayer.PlayerInit(1, pData.Name, pData.Surname, StrategyAssetManager.GetBiologicalAvatar(1), entity, hexUnit);
                        SystemManager.Get<PlayerControlSystem>().SetupComponentData(entityManager, entity);
                        break;
                }
            }
        }

        yield return new WaitForFixedUpdate();
        HexMapCamera.SetTarget(StrategyPlayer.Unit.transform.position);
        //todo :目前先不初始UI
        //  World.Active.GetOrCreateManager<PlayerMessageUiSystem>().SetupGameObjects();
        //  World.Active.GetOrCreateManager<WorldTimeSystem>().SetupValue(true);

        loadingViewCom.Close();

    }

    #endregion

    #region 编辑开始流传
    IEnumerator InitEdit(OpeningInfo openingInfo)
    {

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

        }

        yield return 0;
    }

    #endregion





    void Update()
    {
        if (Input.GetKey(KeyCode.BackQuote))
        {
            IsEdit = !IsEdit;
        }

        if (IsEdit)
        {
            EditUI.SetActive(true);

            RunTimeUI.SetActive(false);
            FixedUI.SetActive(false);
        }
        else
        {
            EditUI.SetActive(false);

            RunTimeUI.SetActive(true);
            FixedUI.SetActive(true);
        }

    }



    /// <summary>
    /// 退出地图模式
    /// </summary>
    public void ExitMapModel()
    {
        UICenterMasterManager.Instance.CloseWindow(WindowID.MessageWindow);
        Instance.MainCamera.enabled = false;

    }


    /// <summary>
    /// 进入地图模式
    /// </summary>
    public void EnterMapModel()
    {
        UICenterMasterManager.Instance.ShowWindow(WindowID.MessageWindow);
        //  UICenterMasterManager.Instance.ShowWindow(WindowID.PlayerInfoWindow);

        Instance.MainCamera.enabled = true;

        //  PlayerInfoView.Isflag = true;
    }

    public void EnterFightingModel()
    {




    }

    public void EnitFightingModel()
    {

    }


    /// <summary>
    /// 切换模式
    /// </summary>
    public void ChangeModel(StrategySceneModel model)
    {
        if (SceneModel == model)
        {
            return;
        }

        switch (SceneModel)
        {
            case StrategySceneModel.Map:
                // UICenterMasterManager.Instance.CloseWindow(WindowID.CityTitleWindow);
                // UICenterMasterManager.Instance.Cl
                break;
            case StrategySceneModel.Fighting:
                break;
            case StrategySceneModel.LivingArea:
                break;
        }

        switch (model)
        {
            case StrategySceneModel.Fighting:
                break;
            case StrategySceneModel.LivingArea:
                break;
            case StrategySceneModel.Map:
                break;
        }

    }



    public void EnterBuildModel()
    {

        BuildCamera.enabled = true;

    }

    public void ExitBuildModel()
    {
        BuildCamera.enabled = false;
    }

}

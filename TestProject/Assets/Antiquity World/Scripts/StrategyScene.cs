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

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {

#if UNITY_EDITOR
        Debuger.EnableLog = true;
        GameSceneInit.CurOpeningInfo.TestValue();
#endif
        GameSceneInit.InitializeWithScene();

        IeEnumeratorLoad.AddIEnumerator(InitMapInfo());
        IeEnumeratorLoad.AddIEnumerator(InitSystemData());
        IeEnumeratorLoad.AddIEnumerator(InitModel());
        IeEnumeratorLoad.AddIEnumerator(InitGameData());
        IeEnumeratorLoad.AddIEnumerator(WindowSyncOpen());

    }

    /// <summary>
    /// 初始化必要的系统数据
    /// </summary>
    /// <returns></returns>
    IEnumerator InitSystemData()
    {

        EntityManager entityManager = SystemManager.ActiveManager;

        FactionSystem.SetupData();
        //LivingAreaSystem.SetupComponentData(entityManager, hexGrid);

        SystemManager.Get<WorldTimeSystem>().SetupValue(true);

        yield return new WaitForFixedUpdate();

    }

    IEnumerator InitMapInfo()
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
        yield return null;
    }

    IEnumerator InitModel()
    {
        //------------ModelController

        List<ModelFileData> modelFileDatas = SQLService.Instance.QueryAll<ModelFileData>();

        ModelController.Instance.ModelFileDatas = modelFileDatas;

        StartCoroutine(ModelController.Instance.ReadModelFileData());

        yield return null;
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    IEnumerator InitGameData()
    {
        //-------------------初始化Faction

        var entityManager = World.Active.GetOrCreateManager<EntityManager>();

        HexCoordinates hexCoordinates;
        HexCell hexCell;

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
                    SystemManager.Get<CitySystem>().AddCity(data,hexCell);
                    break;
                case 2:
                    SystemManager.Get<OrganizationSystem>().AddOrganization(data, hexCell);
                    break;
                case 3: 
                    SystemManager.Get<ZigguratSystem>().AddZiggurat(data,hexCell);
                    break;
                case 4:
                    SystemManager.Get<ZigguratSystem>().AddZiggurat(data,hexCell);
                    break;
                case 5:
                    SystemManager.Get<ZigguratSystem>().AddZiggurat(data,hexCell);
                    break;
                case 6:
                    SystemManager.Get<ZigguratSystem>().AddZiggurat(data,hexCell);
                    break;
                case 7:
                    SystemManager.Get<ZigguratSystem>().AddZiggurat(data,hexCell);
                    break;
                case 8:
                    SystemManager.Get<ZigguratSystem>().AddZiggurat(data,hexCell);
                    break;
                default:
                    SystemManager.Get<ZigguratSystem>().AddZiggurat(data,hexCell);
                    break;
            }
        }
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

            SystemManager.Get<BiologicalSystem>().AddBiological(bData, entity);

            switch (bData.Identity)
            {
                case 0:
                    break;
                case 1:
                    SystemManager.Get<EquipmentSystem>().AddEquipment(entity, bData.EquipmentJson);
                    SystemManager.Get<ArticleSystem>().SettingArticleFeature(entity, bData.Id);
                    SystemManager.Get<TechniquesSystem>().SpawnTechnique(entity, bData.Id);
                    SystemManager.Get<PlayerControlSystem>().SetupComponentData(entityManager, entity);

                    StrategyPlayer.PlayerInit(1,bData.Name,bData.Surname, StrategyAssetManager.GetBiologicalAvatar(1),entity,hexUnit);

                    break;
                case 2:
                    SystemManager.Get<EquipmentSystem>().AddEquipment(entity, bData.EquipmentJson);

                    break;
                default:
                    Debug.Log("Index 为" + bData.Identity + ">>>>的种类未增加");
                    break;
            }
        }

        SystemManager.Get<DistrictSystem>().SetupComponentData(entityManager);
        SystemManager.Get<RelationSystem>().SetupComponentData(entityManager);
        SystemManager.Get<SocialDialogSystem>().SetupComponentData(entityManager);
        SystemManager.Get<PrestigeSystem>().SetupComponentData(entityManager);
        SystemManager.Get<FamilySystem>().SetupComponentData(entityManager);
        
        yield return null;

    }
    IEnumerator WindowSyncOpen()
    {
        yield return new WaitForFixedUpdate();
        World.Active.GetOrCreateManager<PlayerMessageUiSystem>().SetupGameObjects();

        World.Active.GetOrCreateManager<WorldTimeSystem>().SetupValue(true);
        //World.Active.GetOrCreateManager<MainAssetWindow>
        
        //UICenterMasterManager.Instance.ShowWindow(WindowID.WorldTimeWindow);

        //UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaTitleWindow);
        //UICenterMasterManager.Instance.ShowWindow(WindowID.BuildingWindow);
        //UICenterMasterManager.Instance.ShowWindow(WindowID.BuildingWindow);
        
        HexMapCamera.SetTarget(StrategyPlayer.Unit.transform.position);

        loadingViewCom.Close();
    }

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
    /// 删除开局UI
    /// </summary>
    public void RemoveStartUi()
    {
       // UICenterMasterManager.Instance.DestroyWindow(WindowID.WorldTimeWindow);
        UICenterMasterManager.Instance.DestroyWindow(WindowID.MenuWindow);
     //   UICenterMasterManager.Instance.DestroyWindow(WindowID.PlayerInfoWindow);
        UICenterMasterManager.Instance.DestroyWindow(WindowID.MessageWindow);
        UICenterMasterManager.Instance.DestroyWindow(WindowID.MapWindow);
        UICenterMasterManager.Instance.DestroyWindow(WindowID.LivingAreaTitleWindow);

    }



    /// <summary>
    /// 退出地图模式
    /// </summary>
    public void ExitMapModel()
    {
        UICenterMasterManager.Instance.CloseWindow(WindowID.MessageWindow);
        Instance.MainCamera.enabled = false;

       // PlayerInfoView.Isflag = false;

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

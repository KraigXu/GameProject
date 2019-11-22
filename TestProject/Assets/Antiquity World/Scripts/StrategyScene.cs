﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DataAccessObject;
using GameSystem;
using GameSystem.Ui;
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
                    ZigguratSystem.AddZiggurat(entityManager, data, hexCell);
                    break;
                default:
                    Debug.Log(string.Format("{0}功能尚未完善", data.SpecialIndex));
                    break;
            }
        }
    }

    /// <summary>
    /// 初始化生物数据
    /// </summary>
    IEnumerator InitBiologicalData()
    {
        Dictionary<int, Entity> familyIdMap = new Dictionary<int, Entity>();
        Dictionary<int, Entity> factionIdMap = new Dictionary<int, Entity>();
        Dictionary<int, Entity> biologicalIdMap = new Dictionary<int, Entity>();
        //--------------------FamilyI

        EntityArchetype familyArchetype = entityManager.CreateArchetype(typeof(Family));
        List<FamilyData> familyData = SQLService.Instance.QueryAll<FamilyData>();
        for (int i = 0; i < familyData.Count; i++)
        {
            Entity family = entityManager.CreateEntity(familyArchetype);
            FamilySystem.CreateFamily(entityManager, family, familyData[i]);
            familyIdMap.Add(familyData[i].Id, family);
        }
        familyData.Clear();
        yield return new WaitForFixedUpdate();
        //----------------------生成Fatction
        EntityArchetype factionArchetype = SystemManager.ActiveManager.CreateArchetype(typeof(Faction));
        List<FactionData> factionDatas = SQLService.Instance.QueryAll<FactionData>();

        for (int i = 0; i < factionDatas.Count; i++)
        {
            Entity faction = SystemManager.ActiveManager.CreateEntity(factionArchetype);
            FactionSystem.CreateFaction(entityManager, faction, factionDatas[i]);
            factionIdMap.Add(factionDatas[i].Id, faction);
        }
        factionDatas.Clear();
        yield return new WaitForFixedUpdate();

        //------------------初始化Biological
        List<BiologicalData> biologicalDatas = SQLService.Instance.QueryAll<BiologicalData>();
        BiologicalData bData;
        for (int i = 0; i < biologicalDatas.Count; i++)
        {
            bData = biologicalDatas[i];
            Entity entity = entityManager.CreateEntity();
            BiologicalSystem.CreateBiological(entityManager, entity, bData);

            switch (bData.Identity)
            {
                case 0:
                case 1:
                case 2:
                default:
                    SystemManager.Get<EquipmentSystem>().AddEquipment(entity, bData.EquipmentJson);
                    SystemManager.Get<ArticleSystem>().SettingArticleFeature(entity, bData.Id);
                    SystemManager.Get<TechniquesSystem>().SpawnTechnique(entity, bData.Id);
                    break;
            }

            if (bData.FamilyId != 0)
                FamilySystem.AddFamilyCom(entityManager, familyIdMap[bData.FamilyId], entity);

            if (bData.FactionId != 0)
                FactionSystem.AddFactionCom(entityManager, factionIdMap[bData.FactionId], entity);
            
            if(bData.TeamId!=0)
                TeamSystem.SetupData(entityManager, entity);

            biologicalIdMap.Add(biologicalDatas[i].Id, entity);

        }
        biologicalDatas.Clear();
        bData = null;

        yield return new WaitForFixedUpdate();

        List<RelationData> relationDatas = SQLService.Instance.QueryAll<RelationData>();
        for (int i = 0; i < relationDatas.Count; i++)
        {
            RelationSystem.AddRealtionValue(biologicalIdMap[relationDatas[i].MainId], biologicalIdMap[relationDatas[i].AimsId]);
        }

        relationDatas.Clear();

        yield return new WaitForFixedUpdate();

        List<CampData> campDatas = SQLService.Instance.QueryAll<CampData>();
        CampData camp;
        for (int i = 0; i < campDatas.Count; i++)
        {
            camp = campDatas[i];
            hexCoordinates = new HexCoordinates(camp.X, camp.Y);
            hexCell = hexGrid.GetCell(hexCoordinates);
            if (hexCell == null)
                continue;

            GameObject go=new GameObject();
            HexUnit hexUnit= go.AddComponent<HexUnit>();
            Entity entity = go.AddComponent<GameObjectEntity>().Entity;
         
            if (camp.ModelId != 0)
            {
                ModelSpawnSystem.SetupComponentData(entity,go);
            }

            //HexUnit hexUnit = Instantiate(StrategyAssetManager.GetHexUnitPrefabs(camp.ModelId));
            
            //Entity entity = hexUnit.GetComponent<GameObjectEntity>().Entity;
            //int[] bioid;
            //campDatas[i].Ids.Split(',');

            TeamSystem.SetupData(entityManager,entity);
            hexGrid.AddUnit(hexUnit, hexCell, UnityEngine.Random.Range(0f, 360f));
        }
        campDatas.Clear();

        yield return new WaitForFixedUpdate();

        //初始玩家信息
        OpeningInfo openingInfo = GameSceneInit.CurOpeningInfo;
        if (biologicalIdMap.ContainsKey(openingInfo.PlayerId))
        {
            Entity playerEntity = biologicalIdMap[openingInfo.PlayerId];
            PlayerControlSystem.SetupComponentData(entityManager,playerEntity);
            //StrategyPlayer.PlayerInit(1, pData.Name, pData.Surname, StrategyAssetManager.GetBiologicalAvatar(1), entity, hexUnit);
        }
        else
        {
            Debug.Log("异常错误");
        }


        yield return new WaitForFixedUpdate();

        //  SystemManager.Get<DistrictSystem>().SetupComponentData(entityManager);
        // SystemManager.Get<SocialDialogSystem>().SetupComponentData(entityManager);
        // SystemManager.Get<PrestigeSystem>().SetupComponentData(entityManager);

        // LivingAreaSystem.SetupComponentData(entityManager, hexGrid);

        //SystemManager.Get<WorldTimeSystem>().SetupValue(true);

        // HexMapCamera.SetTarget(StrategyPlayer.Unit.transform.position);
        //  todo :目前先不初始UI
        //  World.Active.GetOrCreateManager<PlayerMessageUiSystem>().SetupGameObjects();
        //  World.Active.GetOrCreateManager<WorldTimeSystem>().SetupValue(true);
        factionIdMap.Clear();
        biologicalIdMap.Clear();
        familyIdMap.Clear();
        GC.Collect();

        loadingViewCom.Close();
    }
    #endregion

    #region 编辑开始流程
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

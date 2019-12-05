using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DataAccessObject;
using GameSystem;
using GameSystem.Ui;
using Newtonsoft.Json;
using Unity.Entities;
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

    public bool IsEdit = false;

    //---------Message
    public LoadingView LoadingViewCom;

    public IEnumeratorLoad IeEnumeratorLoad;
    public StrategySceneModel SceneModel = StrategySceneModel.Map;

    private EntityManager _entityManager;

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
            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
            //初始地图信息
          //  IeEnumeratorLoad.AddIEnumerator(InitMapInfo(openingInfo.MapFilePath, openingInfo.MapFileVersion, openingInfo.Mapseed));
            //初始生物信息
            IeEnumeratorLoad.AddIEnumerator(InitBiologicalData());
        }
        else
        {
            IeEnumeratorLoad.AddIEnumerator(InitEdit(openingInfo));
        }


        UICenterMasterManager.Instance.ShowWindow(WindowID.MenuWindow);


    }
    #region  正常开始流程

    //IEnumerator InitMapInfo(string filePath, int version, int seed)
    //{
    //    hexGrid.seed = seed;
    //    //加载地图
    //    using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath)))
    //    {
    //        int header = reader.ReadInt32();
    //        if (header <= version)
    //        {
    //            hexGrid.Load(reader, header);
    //            HexMapCamera.ValidatePosition();
    //        }
    //        else
    //        {
    //            Debug.LogWarning("Unknown map format " + header);
    //        }
    //    }

    //    yield return new WaitForFixedUpdate();

    //    yield return ModelController.Instance.ReadModelFileData();

    //    List<LivingAreaData> datas = SQLService.Instance.QueryAll<LivingAreaData>();
    //    for (int i = 0; i < datas.Count; i++)
    //    {
    //        var data = datas[i];
    //        hexCoordinates = new HexCoordinates(data.PositionX, data.PositionZ);

    //        hexCell = hexGrid.GetCell(hexCoordinates);
    //        if (hexCell == null)
    //            continue;
    //        hexCell.SpecialIndex = data.SpecialIndex;

    //        switch (data.SpecialIndex)
    //        {
    //            case 1:
    //                CitySystem.AddCity(_entityManager, data, hexCell);
    //                break;
    //            case 2:
    //                OrganizationSystem.AddOrganization(_entityManager, data, hexCell);
    //                break;
    //            case 3:
    //                ZigguratSystem.AddZiggurat(_entityManager, data, hexCell);
    //                break;
    //            default:
    //                Debug.Log(string.Format("{0}功能尚未完善", data.SpecialIndex));
    //                break;
    //        }
    //    }
    //}

    /// <summary>
    /// 初始化生物数据
    /// </summary>
    IEnumerator InitBiologicalData()
    {
        Dictionary<int, Entity> familyIdMap = new Dictionary<int, Entity>();
        Dictionary<int, Entity> factionIdMap = new Dictionary<int, Entity>();
        Dictionary<int, Entity> biologicalIdMap = new Dictionary<int, Entity>();
       
        //--------------------FamilyI

        EntityArchetype familyArchetype = _entityManager.CreateArchetype(typeof(Family));
        List<FamilyData> familyData = SQLService.Instance.QueryAll<FamilyData>();
        for (int i = 0; i < familyData.Count; i++)
        {
            Entity family = _entityManager.CreateEntity(familyArchetype);
            FamilySystem.CreateFamily(_entityManager, family, familyData[i]);
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
            FactionSystem.CreateFaction(_entityManager, faction, factionDatas[i]);
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
            Entity entity = _entityManager.CreateEntity();
            BiologicalSystem.CreateBiological(_entityManager, entity, bData);

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
                FamilySystem.AddFamilyCom(_entityManager, familyIdMap[bData.FamilyId], entity);

            if (bData.FactionId != 0)
                FactionSystem.AddFactionCom(_entityManager, factionIdMap[bData.FactionId], entity);
            
            if(bData.TeamId!=0)
               // TeamSystem.SetupData(_entityManager, entity);

            biologicalIdMap.Add(biologicalDatas[i].Id, entity);

        }
        biologicalDatas.Clear();
        bData = null;

        yield return new WaitForFixedUpdate();

        List<RelationData> relationDatas = SQLService.Instance.QueryAll<RelationData>();
        for (int i = 0; i < relationDatas.Count; i++)
        {
            if (biologicalIdMap.ContainsKey(relationDatas[i].MainId) &&
                biologicalIdMap.ContainsKey(relationDatas[i].AimsId))
            {
                RelationSystem.AddRealtionValue(biologicalIdMap[relationDatas[i].MainId], biologicalIdMap[relationDatas[i].AimsId]);
            }
            else
            {
                Debug.Log(relationDatas[i].MainId+"-----"+relationDatas[i].AimsId);
            }

            
        }

        relationDatas.Clear();

        yield return new WaitForFixedUpdate();

        List<CampData> campDatas = SQLService.Instance.QueryAll<CampData>();
        CampData camp;
        for (int i = 0; i < campDatas.Count; i++)
        {
            camp = campDatas[i];
            //hexCoordinates = new HexCoordinates(camp.X, camp.Y);
            //hexCell = hexGrid.GetCell(hexCoordinates);
            //if (hexCell == null)
            //    continue;

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

           // TeamSystem.SetupData(_entityManager,entity);
           // hexGrid.AddUnit(hexUnit, hexCell, UnityEngine.Random.Range(0f, 360f));
        }
        campDatas.Clear();

        yield return new WaitForFixedUpdate();
        Dictionary<int,GameObject>  teamMap=new Dictionary<int, GameObject>();
        List<TeamData> teamDatas = SQLService.Instance.QueryAll<TeamData>();
        GameObject teamPrefab=new GameObject("TeamModel");
        teamPrefab.transform.position=Vector3.zero;
    
        for (int i = 0; i < teamDatas.Count; i++)
        {

            GameObject itemgo = Instantiate(StrategyAssetManager.TemeModelPrefabs);
            
            GameObjectEntity entityCom = itemgo.gameObject.GetComponent <GameObjectEntity>();
            
            TeamSystem.SetupData(_entityManager,entityCom.Entity,teamDatas[i]);

            string[] memberIds = teamDatas[i].MemberIds.Split(';');

            StatusInfo statusInfo = JsonConvert.DeserializeObject<StatusInfo>(teamDatas[i].StatusInfo);
            itemgo.transform.position = statusInfo.Position;
            itemgo.transform.rotation =Quaternion.Euler( statusInfo.Face);
            itemgo.transform.SetParent(teamPrefab.transform);

            TeamFixed teamFixed=new TeamFixed();
            teamFixed.Id = teamDatas[i].Id;
            teamFixed.Transform = itemgo.transform;

            for (int j = 0; j < memberIds.Length; j++)
            {
                int memberId=  int.Parse(memberIds[j]);
                if (biologicalIdMap.ContainsKey(memberId))
                {
                    teamFixed.Members.Add(biologicalIdMap[memberId]);
                }
            }

            GameStaticData.TeamRunDic.Add(entityCom.Entity,teamFixed);
            teamMap.Add(teamDatas[i].Id,itemgo);
        }

        yield return new WaitForFixedUpdate();


        //初始玩家信息
        OpeningInfo openingInfo = GameSceneInit.CurOpeningInfo;
        if (biologicalIdMap.ContainsKey(openingInfo.PlayerId))
        {
            Entity playerEntity = biologicalIdMap[openingInfo.PlayerId];
            PlayerControlSystem.SetupComponentData(_entityManager,playerEntity);

            OverLookCameraController overLookCamera= Camera.main.gameObject.GetComponent<OverLookCameraController>();
            overLookCamera.m_targetPosition = teamMap[openingInfo.TeamId].transform.position;

            PlayerController.Instance.PlayerGo = teamMap[openingInfo.TeamId];

            //TeamId
            //StrategyPlayer.PlayerInit(1, pData.Name, pData.Surname, StrategyAssetManager.GetBiologicalAvatar(1), entity, hexUnit);
        }
        else
        {
            Debug.Log("异常错误");
        }

        ////初始玩家操作
        //GameObject playgo=new GameObject("PlayerController");
        //playgo.AddComponent<PlayerMouseControl>();
        UICenterMasterManager.Instance.ShowWindow(WindowID.TipsWindow);

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

        //LoadingViewCom.Close();
    }
    #endregion

    #region 编辑开始流程
    IEnumerator InitEdit(OpeningInfo openingInfo)
    {

        if (openingInfo.IsEditMode == true)
        {
            if (openingInfo.GenerateMaps)
            {
                //mapGenerator.GenerateMap(openingInfo.Mapx, openingInfo.Mapz, openingInfo.Wrapping);
            }
            else
            {
                //hexGrid.CreateMap(openingInfo.Mapx, openingInfo.Mapz, openingInfo.Wrapping);
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

    }



    /// <summary>
    /// 退出地图模式
    /// </summary>
    public void ExitMapModel()
    {
        UICenterMasterManager.Instance.CloseWindow(WindowID.MessageWindow);
     //   Instance.MainCamera.enabled = false;

    }


    /// <summary>
    /// 进入地图模式
    /// </summary>
    public void EnterMapModel()
    {
        UICenterMasterManager.Instance.ShowWindow(WindowID.MessageWindow);
        //  UICenterMasterManager.Instance.ShowWindow(WindowID.PlayerInfoWindow);

        //Instance.MainCamera.enabled = true;

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

     //  BuildCamera.enabled = true;

    }

    public void ExitBuildModel()
    {
      //  BuildCamera.enabled = false;
    }

}

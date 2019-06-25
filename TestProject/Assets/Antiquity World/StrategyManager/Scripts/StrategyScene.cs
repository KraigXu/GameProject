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

            BiologicalSystem.SpawnRandomBiological(hexUnit.transform);


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

            


            Entity entity = entityGo.GetComponent<GameObjectEntity>().Entity;
            // Entity entity = entityManager.CreateEntity(BiologicalArchetype);
            entityGo.gameObject.AddComponent<HexUnit>();

            ////entityManager.SetComponentData(entity, new Position
            ////{
            ////    Value = new float3(0, -6, 6)
            ////});

            ////entityManager.SetComponentData(entity, new Rotation
            ////{
            ////    Value = Quaternion.identity
            ////});
            entityManager.AddComponent(entity, typeof(Biological));
            entityManager.SetComponentData(entity, new Biological()
            {
                BiologicalId = datas[i].Id,
                Age = datas[i].Age,
                Sex = datas[i].Sex,
                CharmValue = 0,
                Mobility = 0,
                OperationalAbility = 0,
                LogicalThinking = 0,

                Disposition = (byte)datas[i].Disposition,
                NeutralValue = (byte)UnityEngine.Random.Range(0, 255),

                LuckValue = 100,
                PrestigeValue = 100,

                ExpEmptyHand = 9999,
                ExpLongSoldier = 9999,
                ExpShortSoldier = 9999,
                ExpJones = 9999,
                ExpHiddenWeapone = 9999,
                ExpMedicine = 9999,
                ExpArithmetic = 9999,
                ExpMusic = 9999,
                ExpWrite = 9999,
                ExpDrawing = 9999,
                ExpExchange = 9999,
                ExpTaoism = 9999,
                ExpDharma = 9999,
                ExpPranayama = 9999,

                AvatarId = datas[i].AvatarId,
                ModelId = datas[i].ModelId,
                FamilyId = datas[i].FamilyId,
                FactionId = datas[i].FactionId,
                TitleId = datas[i].TitleId,
                TechniquesId = 0,
                EquipmentId = 0,

                Jing = 100,
                Qi = 100,
                Shen = 100,
                Tizhi = datas[i].Tizhi,
                Lidao = datas[i].Lidao,
                Jingshen = datas[i].Jingshen,
                Lingdong = datas[i].Lingdong,
                Wuxing = datas[i].Wuxing
            });

            //entityManager.SetComponentData(entity, new BodyProperty
            //{
            //    Thought = 100,
            //    Neck = 100,
            //    Heart = 100,
            //    Eye = 100,
            //    Ear = 100,
            //    LeftLeg = 100,
            //    RightLeg = 100,
            //    LeftHand = 100,
            //    RightHand = 100,
            //    Fertility = 100,
            //    Appearance = 100,
            //    Dress = 100,
            //    Skin = 100,

            //    StrategyMoveSpeed = 6,
            //    FireMoveSpeed = 10,

            //});

            //entityManager.SetComponentData(entity, new Equipment
            //{
            //    HelmetId = -1,
            //    ClothesId = -1,
            //    BeltId = -1,
            //    HandGuard = -1,
            //    Pants = -1,
            //    Shoes = -1,
            //    WeaponFirstId = -1,
            //    WeaponSecondaryId = -1
            //});

            //entityManager.SetComponentData(entity, new EquipmentCoat
            //{
            //    SpriteId = 1,
            //    Type = EquipType.Coat,
            //    Level = EquipLevel.General,
            //    Part = EquipPart.All,
            //    BluntDefense = 19,
            //    SharpDefense = 20,
            //    Operational = 100,
            //    Weight = 3,
            //    Price = 1233,
            //});

            //entityManager.AddComponentData(entity, new Knapsack
            //{
            //    UpperLimit = 1000000,
            //    KnapscakCode = datas[i].Id
            //});

            //entityManager.AddComponentData(entity, new Team
            //{
            //    TeamBossId = datas[i].TeamId
            //});



            //if (datas[i].Identity == 0)
            //{
            //    entityManager.AddComponent(entity, ComponentType.Create<NpcInput>());
            //}
            //else if (datas[i].Identity == 1)
            //{
            //    entityManager.AddComponent(entity, ComponentType.Create<PlayerInput>());

            //    //  SystemManager.Get<PlayerControlSystem>().InitPlayerEvent(entityGo.gameObject);
            //}

            //if (datas[i].FactionId != 0)
            //{
            //    entityManager.AddComponentData(entity, new FactionProperty
            //    {

            //    });
            //}

            //entityManager.AddComponent(entity, ComponentType.Create<BehaviorData>());
            //entityManager.SetComponentData(entity, new BehaviorData
            //{
            //    Target = Vector3.zero,
            //});

            //if (string.IsNullOrEmpty(datas[i].JifaJson) == false)
            //{
            //    TechniquesSystem.SpawnTechnique(entity, datas[i].JifaJson);
            //}

            //ArticleSystem.SpawnArticle(SQLService.Instance.SimpleQuery<ArticleData>(" Bid=?", datas[i].Id), entity);

            ////  SystemManager.Get<BiologicalSystem>().InitComponent(entityGo.gameObject);

            ////   ComponentGroup group = new ComponentGroup();
            ////    group.AiCharacter = entityGo.GetComponent<AICharacterControl>();
            ////  group.Animator = entityGo.GetComponent<Animator>();
            ////if (ComponentDic.ContainsKey(entity) == false)
            ////{
            ////    ComponentDic.Add(entity, group);
            ////}

            //entityManager.AddSharedComponentData(entity, StrategyStyle.Instance.BiologicalRenderers[0]);


            //GameStaticData.BiologicalNameDic.Add(datas[i].Id, datas[i].Name);
            //GameStaticData.BiologicalSurnameDic.Add(datas[i].Id, datas[i].Surname);
            //GameStaticData.BiologicalDescription.Add(datas[i].Id, datas[i].Description);

            //    List<ArticleRecordData> articleRecordDatas= SQLService.Instance.Query<ArticleRecordData>(
            //    "select * from ArticleData ad INNER JOIN ArticleRecordData ard ON ad.Id=ard.ArticleId WHERE ard.Bid=?",
            //    datas[i].Id);


            //for (int j = 0; j < articleRecordDatas.Count; j++)
            //{
            //    Entity articleentity = entityManager.CreateEntity(ArticleArchetype);

            //    entityManager.SetComponentData(articleentity,new ArticleItem()
            //    {
            //        BiologicalEntity= entity,
            //        Count = articleRecordDatas[j].Count,
            //        MaxCount = articleRecordDatas[j].MaxCount,
            //        Weight = 1,
            //    });

            //}

            //entityManager.AddComponent(entity, ComponentType.Create<Energy>());
            //entityManager.SetComponentData(entity, new Energy
            //{
            //    Value1 = 100,
            //    Value2 = 300,
            //});

            //entityManager.AddComponent(entity, ComponentType.Create<BiologicalAvatar>());
            //entityManager.SetComponentData(entity, new BiologicalAvatar
            //{
            //    Id =
            //});
            // entityManager.SetComponentData(entity,new Relationship());

            // BiologicalStatus biologicalStatus = new BiologicalStatus();
            // biologicalStatus.BiologicalIdentity = datas[i].Identity;
            // biologicalStatus.TargetId = 0;
            // biologicalStatus.TargetType = 0;
            // biologicalStatus.LocationType = (LocationType)datas[i].LocationType;
            // entityManager.SetComponentData(entity, biologicalStatus);

            //switch ((LocationType)datas[i].LocationType)
            //{
            //    case LocationType.None:
            //        break;
            //    case LocationType.City:
            //        {
            //        }
            //        break;
            //    case LocationType.Field:
            //        {
            //            //entityManager.AddComponent(entity, ComponentType.Create<ModelSpawnData>());
            //            //entityManager.SetComponentData(entity, new ModelSpawnData
            //            //{
            //            //    ModelData = new ModelComponent
            //            //    {
            //            //        Id = SystemManager.Get<ModelMoveSystem>().AddModel(GameStaticData.ModelPrefab[datas[i].ModelId], new Vector3(datas[i].X, datas[i].Y, datas[i].Z)),
            //            //        Target = Vector3.zero,
            //            //        Speed = 6,
            //            //    },
            //            //});

            //            //entityManager.AddComponent(entity, ComponentType.Create<InteractionElement>());
            //            //entityManager.SetComponentData(entity, new InteractionElement
            //            //{
            //            //    Distance = 1,
            //            //    ModelCode = 1,
            //            //});
            //        }
            //        break;
            //}



            //结合GameObject
            //entityManager.AddComponent(entity, ComponentType.Create<AssociationPropertyData>());
            //entityManager.SetComponentData(entity, new AssociationPropertyData
            //{
            //    IsEntityOver = 1,
            //    IsGameObjectOver = 0,
            //    IsModelShow = 0,
            //    Position = new Vector3(datas[i].X, datas[i].Y, datas[i].Z),
            //    ModelUid = datas[i].ModelId
            //});
            //if (datas[i].Id == 1)
            //{
            //    BiologicalItem item = GameObject.Find("TestA").GetComponent<BiologicalItem>();
            //    item.Entity = entity;
            //    item.IsInit = true;
            //}
            // SystemManager.Get<PlayerControlSystem>().InitPlayerEvent(entityGo.gameObject);
        }




        SystemManager.Get<ArticleSystem>().SetupComponentData(entityManager);

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


    public void SpawnRandomBiological(HexCell cell)
    {
        HexUnit hexUnit = Instantiate(HexUnit.unitPrefab);
        hexGrid.AddUnit(hexUnit, cell, UnityEngine.Random.Range(0f, 360f));
        BiologicalSystem.SpawnRandomBiological(hexUnit.transform);
    }
}

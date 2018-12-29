using System;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.ThirdPerson;
using GameSystem.Ui;
using Newtonsoft.Json;
using Object = UnityEngine.Object;

namespace GameSystem
{
    public sealed class StrategySceneInit
    {
        public static EntityArchetype DistrictArchetype;
        public static EntityArchetype BuildingArchetype;
        public static EntityArchetype TechniquesArchetype;
        public static EntityArchetype RelationArchetype;

        public static MeshInstanceRenderer PlayerLook;
        public static MeshInstanceRenderer BiologicalLook;
        public static MeshInstanceRenderer LivingAreaLook;
        public static MeshInstanceRenderer PlayerShotLook;
        public static MeshInstanceRenderer EnemyShotLook;
        public static MeshInstanceRenderer EnemyLook;

        public static DemoSetting Settings;

        /// <summary>
        /// 在场景加载之前初始化数据
        /// 此方法为我们将在此游戏中频繁生成的实体创建原型。
        ///Archetypes是可选的，但可以大大加速实体产生。
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            Debug.Log("Initialize Over");
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            DistrictArchetype = entityManager.CreateArchetype(typeof(District));
            BuildingArchetype = entityManager.CreateArchetype(typeof(Building));
            TechniquesArchetype = entityManager.CreateArchetype(typeof(Techniques));
            RelationArchetype = entityManager.CreateArchetype(typeof(Relation));

        }


        /// <summary>
        /// 在场景加载后初始化数据
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializeAfterSceneLoad()
        {
            Debug.Log("InitializeAfterSceneLoad Over");
            var settingsGo = GameObject.Find("Settings");
            if (settingsGo == null)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                return;
            }
            else
            {
                Settings = settingsGo?.GetComponent<DemoSetting>();
                if (!Settings)
                {
                    return;
                }
            }
            InitializeWithScene();
        }

        // Begin a new game.
        public static void NewGame()
        {
            Debug.Log("NewGame Over!");
        }



        public static void InitializeWithScene()
        {
            var settingsGo = GameObject.Find("Settings");
            if (settingsGo == null)
            {
                return;
            }

            Settings = settingsGo?.GetComponent<DemoSetting>();
            if (!Settings)
            {
                return;
            }

            //TechniqueJsonData techniqueJson=new TechniqueJsonData();
            //techniqueJson.Id = 1;
            //techniqueJson.Content.Add(new KeyValuePair<int, int>(1,500));
            //techniqueJson.Content.Add(new KeyValuePair<int, int>(7, 300));
            //techniqueJson.Content.Add(new KeyValuePair<int, int>(9, 800));

            //Debug.Log(JsonConvert.SerializeObject(techniqueJson));

            //PlayerLook = GetLookFromPrototype("PlayerRenderPrototype");
            //BiologicalLook = GetLookFromPrototype("BiologicalRenderPrototype");
            //LivingAreaLook = GetLookFromPrototype("LivingAreaRenderPrototype");
            //PlayerShotLook = GetLookFromPrototype("PlayerShotRenderPrototype");
            //EnemyShotLook = GetLookFromPrototype("EnemyShotRenderPrototype");
            //EnemyLook = GetLookFromPrototype("EnemyRenderPrototype");
            //EnemySpawnSystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());

            //StrategySystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());

            //LivingAreaBuildingSystem     --暂无开启

            //BiologicalSystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());
            //World.Active.GetOrCreateManager<UpdatePlayerHUD>().SetupGameObjects();
            //World.Active.GetOrCreateManager<StrategySystem>().Update();

            //获取配置
            DemoSetting settings = GameObject.Find("Settings").GetComponent<DemoSetting>();

            // Access the ECS entity manager
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            #region Time

            {

                DateTime time = Convert.ToDateTime("1000-01-01 00:00:00");
                WorldTimeManager.Instance.Init(time);


                GameStaticData.TimeJijie.Add(1, "春");
                GameStaticData.TimeJijie.Add(2, "夏");
                GameStaticData.TimeJijie.Add(3, "秋");
                GameStaticData.TimeJijie.Add(4, "冬");

                GameStaticData.TimeShichen.Add(1, "子时");
                GameStaticData.TimeShichen.Add(2, "丑时");
                GameStaticData.TimeShichen.Add(3, "寅时");
                GameStaticData.TimeShichen.Add(4, "卯时");
                GameStaticData.TimeShichen.Add(5, "辰时");
                GameStaticData.TimeShichen.Add(6, "巳时");
                GameStaticData.TimeShichen.Add(7, "午时");
                GameStaticData.TimeShichen.Add(8, "未时");
                GameStaticData.TimeShichen.Add(9, "申时");
                GameStaticData.TimeShichen.Add(10, "酉时");
                GameStaticData.TimeShichen.Add(11, "戊时");
                GameStaticData.TimeShichen.Add(12, "亥时");
            }

            #endregion

            #region Data
            {
                List<AvatarData> avatarDatas = SqlData.GetAllDatas<AvatarData>();

                for (int i = 0; i < avatarDatas.Count; i++)
                {
                    if (avatarDatas[i].Type == 1)
                    {
                        GameStaticData.BiologicalAvatar.Add(avatarDatas[i].Id, Resources.Load<Sprite>(avatarDatas[i].Path));
                    }
                    else if (avatarDatas[i].Type == 2)
                    {
                        GameStaticData.BuildingAvatar.Add(avatarDatas[i].Id, Resources.Load<Sprite>(avatarDatas[i].Path));
                    }
                }

                List<ModelData> modelDatas = SqlData.GetAllDatas<ModelData>();
                for (int i = 0; i < modelDatas.Count; i++)
                {
                    GameStaticData.ModelPrefab.Add(modelDatas[i].Id, Resources.Load<GameObject>(modelDatas[i].Path));
                }
                SocialDialogSystem.SetupComponentData(entityManager);
                PrestigeSystem.SetupComponentData(entityManager);
            }

            #endregion

            #region DistrictInit
            {
                List<DistrictData> districtDatas = SqlData.GetAllDatas<DistrictData>();

                for (int i = 0; i < districtDatas.Count; i++)
                {
                    GameObject go = GameObject.Instantiate(GameStaticData.ModelPrefab[districtDatas[i].Model], new Vector3(districtDatas[i].X, districtDatas[i].Y, districtDatas[i].Z), Quaternion.identity);

                    DistrictRange districtRange = go.GetComponent<DistrictRange>();
                    districtRange.DistrictId = districtDatas[i].Id;

                    Entity district = go.GetComponent<GameObjectEntity>().Entity;
                    entityManager.AddComponent(district, ComponentType.Create<District>());
                    entityManager.SetComponentData(district, new District
                    {
                        Id = districtDatas[i].Id,
                        Type = districtDatas[i].Type,
                        ProsperityLevel = districtDatas[i].ProsperityLevel,
                        TrafficLevel = districtDatas[i].TrafficLevel,
                        GrowingModulus = districtDatas[i].GrowingModulus,
                        SecurityModulus = districtDatas[i].SecurityModulus
                    });

                    GameStaticData.DistrictName.Add(districtDatas[i].Id, districtDatas[i].Name);
                    GameStaticData.DistrictDescriptione.Add(districtDatas[i].Id, districtDatas[i].Description);
                }


                GameStaticData.BuildingType.Add(0, "BuildingType1");
                GameStaticData.BuildingType.Add(1, "BuildingType2");
                GameStaticData.BuildingType.Add(2, "BuildingType3");
                GameStaticData.BuildingType.Add(3, "BuildingType4");
                GameStaticData.BuildingType.Add(4, "BuildingType5");
                GameStaticData.BuildingType.Add(5, "BuildingType6");
                GameStaticData.BuildingType.Add(6, "BuildingType7");

                GameStaticData.BuildingStatus.Add(0, "空地");
                GameStaticData.BuildingStatus.Add(1, "正常");
                GameStaticData.BuildingStatus.Add(2, "建筑中");

            }
            #endregion

            #region Building
            {
                List<BuildingData> buildingData = SqlData.GetAllDatas<BuildingData>();

                for (int j = 0; j < buildingData.Count; j++)
                {
                    Entity building = entityManager.CreateEntity(BuildingArchetype);
                    entityManager.SetComponentData(building, new Building
                    {
                        Id = buildingData[j].Id,
                        Level = buildingData[j].BuildingLevel,
                        Status = buildingData[j].Status,
                        OwnId = buildingData[j].OwnId,
                        Type = buildingData[j].Type,
                        DurableValue = buildingData[j].DurableValue,
                        ParentId = buildingData[j].ParentId,
                        Position = new Vector3(buildingData[j].X, buildingData[j].Y, buildingData[j].Z)
                    });

                    GameStaticData.BuildingName.Add(buildingData[j].Id, buildingData[j].Name);
                    GameStaticData.BuildingDescription.Add(buildingData[j].Id, buildingData[j].Description);
                }
            }

            #endregion

            #region LivingAreaInit
            {
                List<LivingAreaData> livingAreaDatas = SqlData.GetAllDatas<LivingAreaData>();
                for (int i = 0; i < livingAreaDatas.Count; i++)
                {
                    var go = GameObject.Instantiate(GameStaticData.ModelPrefab[livingAreaDatas[i].ModelBaseId] , new Vector3(livingAreaDatas[i].PositionX, livingAreaDatas[i].PositionY, livingAreaDatas[i].PositionZ), Quaternion.identity);

                    Entity livingArea = go.GetComponent<GameObjectEntity>().Entity;

                    entityManager.AddComponent(livingArea, ComponentType.Create<Position>());
                    entityManager.SetComponentData(livingArea, new Position
                    {
                        Value = new float3(livingAreaDatas[i].PositionX, livingAreaDatas[i].PositionY, livingAreaDatas[i].PositionZ)
                    });

                    entityManager.AddComponent(livingArea, ComponentType.Create<LivingArea>());
                    entityManager.SetComponentData(livingArea, new LivingArea
                    {
                        Id = livingAreaDatas[i].Id,
                        ModelBaseId=livingAreaDatas[i].ModelBaseId,
                        ModelId = livingAreaDatas[i].ModelMain,
                        PersonNumber = livingAreaDatas[i].PersonNumber,
                        CurLevel = livingAreaDatas[i].LivingAreaLevel,
                        MaxLevel = livingAreaDatas[i].LivingAreaMaxLevel,
                        TypeId = livingAreaDatas[i].LivingAreaType,
                        Money = livingAreaDatas[i].Money,
                        MoneyMax = livingAreaDatas[i].MoneyMax,
                        Iron = livingAreaDatas[i].Iron,
                        IronMax = livingAreaDatas[i].IronMax,
                        Wood = livingAreaDatas[i].Wood,
                        WoodMax = livingAreaDatas[i].WoodMax,
                        Food = livingAreaDatas[i].Food,
                        FoodMax = livingAreaDatas[i].FoodMax,
                        DefenseStrength = livingAreaDatas[i].DefenseStrength,
                        StableValue = livingAreaDatas[i].StableValue,
                        Renown = livingAreaDatas[i].StableValue,
                        Position = new Vector3(livingAreaDatas[i].PositionX, livingAreaDatas[i].PositionY, livingAreaDatas[i].PositionZ),
                    });

                    entityManager.AddComponent(livingArea, ComponentType.Create<InteractionElement>());
                    entityManager.SetComponentData(livingArea, new InteractionElement
                    {
                        Position = new Vector3(livingAreaDatas[i].PositionX, livingAreaDatas[i].PositionY, livingAreaDatas[i].PositionZ),
                        Distance = 1,
                        Id = livingAreaDatas[i].Id,
                        InteractionType = LocationType.LivingAreaIn,
                        InteractionExitType = LocationType.LivingAreaExit,
                        InteractionEnterType = LocationType.LivingAreaEnter,
                        Type = ElementType.LivingArea,
                    });



                    GameStaticData.LivingAreaName.Add(livingAreaDatas[i].Id, livingAreaDatas[i].Name);
                    GameStaticData.LivingAreaDescription.Add(livingAreaDatas[i].Id, livingAreaDatas[i].Description);
                }


                GameStaticData.LivingAreaLevel.Add(0, "0级");
                GameStaticData.LivingAreaLevel.Add(1, "1级");
                GameStaticData.LivingAreaLevel.Add(2, "2级");
                GameStaticData.LivingAreaLevel.Add(3, "3级");
                GameStaticData.LivingAreaLevel.Add(4, "4级");
                GameStaticData.LivingAreaLevel.Add(5, "5级");

                GameStaticData.LivingAreaType.Add(0, "城市");
                GameStaticData.LivingAreaType.Add(1, "帮派");
                GameStaticData.LivingAreaType.Add(2, "村落");
                GameStaticData.LivingAreaType.Add(3, "洞窟");
                GameStaticData.LivingAreaType.Add(4, "遗迹");
                GameStaticData.LivingAreaType.Add(5, "奇迹");
            }
            #endregion

            #region Biological
            {
                List<BiologicalData> data = SqlData.GetWhereDatas<BiologicalData>(" IsDebut=? ", new object[] { 1 });

                for (int i = 0; i < data.Count; i++)
                {
                    var go = GameObject.Instantiate(GameStaticData.ModelPrefab[data[i].ModelId], new Vector3(data[i].X, data[i].Y, data[i].Z), quaternion.identity);
                    go.name = data[i].Id.ToString();
                    Entity biologicalEntity = go.GetComponent<GameObjectEntity>().Entity;

                    Biological biological=new Biological();
                    biological.BiologicalId = data[i].Id;
                    biological.AvatarId = data[i].AvatarId;
                    biological.ModelId = data[i].ModelId;
                    biological.FamilyId = data[i].FamilyId;
                    biological.FactionId = data[i].FactionId;
                    biological.TitleId = data[i].TitleId;

                    biological.SexId = data[i].Sex;
                    biological.Age = data[i].Age;
                    biological.AgeMax = data[i].AgeMax;
                    biological.Disposition = data[i].Disposition;
                    biological.PrestigeValue = data[i].PrestigeValue;
                    biological.CharmValue = 0;
                    biological.CharacterValue = 0;
                    biological.NeutralValue = 0;
                    biological.BodyValue = 0;
                    biological.LuckValue = 0;

                    biological.Tizhi = data[i].Tizhi;
                    biological.Lidao = data[i].Lidao;
                    biological.Jingshen = data[i].Jingshen;
                    biological.Lingdong = data[i].Lingdong;
                    biological.Wuxing = data[i].Wuxing;


                    //初始Technique
                    if (string.IsNullOrEmpty(data[i].JifaJson) == false)
                    {
                        TechniqueJsonData jsonData = JsonConvert.DeserializeObject<TechniqueJsonData>(data[i].JifaJson);
                        TechniquesSystem.SetData(jsonData);
                        biological.TechniquesId = jsonData.Id;
                    }

                    //初始Equipment
                    if (string.IsNullOrEmpty(data[i].EquipmentJson) == false)
                    {
                        EquipmentJsonData jsonData=JsonConvert.DeserializeObject<EquipmentJsonData>(data[i].EquipmentJson);
                        EquipmentSystem.SetData(jsonData);
                        biological.EquipmentId = jsonData.Id;
                       
                    }
                    if (string.IsNullOrEmpty(data[i].ArticleJson) == false)
                    {
                    }

                    if (string.IsNullOrEmpty(data[i].GongfaJson) == false)
                    {
                    }
                    
                    entityManager.AddComponent(biologicalEntity, ComponentType.Create<Biological>());
                    entityManager.SetComponentData(biologicalEntity, biological);


                    entityManager.AddComponent(biologicalEntity, ComponentType.Create<BiologicalStatus>());
                    entityManager.SetComponentData(biologicalEntity, new BiologicalStatus
                    {
                        Position = new Vector3(data[i].X, data[i].Y, data[i].Z),
                        TargetId = 0,
                        TargetType = 0,
                        LocationType = LocationType.Field
                    });

                    entityManager.AddComponent(biologicalEntity, ComponentType.Create<InteractionElement>());
                    entityManager.SetComponentData(biologicalEntity, new InteractionElement
                    {
                        Position = new Vector3(data[i].X, data[i].Y, data[i].Z),
                        Distance = 2,
                        Id = data[i].Id,
                        InteractionType = LocationType.SocialDialogIn,
                        InteractionExitType = LocationType.SocialDialogExit,
                        InteractionEnterType = LocationType.SocialDialogEnter,
                        Type = ElementType.Biological
                    });

                    if (data[i].TeamId > 0)
                    {
                        entityManager.AddComponent(biologicalEntity,ComponentType.Create<Team>());
                        entityManager.SetComponentData(biologicalEntity,new Team
                        {
                            TeamBossId=data[i].TeamId,
                        });
                    }

                    if (data[i].Id == settings.PlayerId)
                    {
                        entityManager.AddComponent(biologicalEntity, ComponentType.Create<PlayerInput>());
                        entityManager.SetComponentData(biologicalEntity, new PlayerInput
                        {
                            MousePoint = Vector3.zero
                        });

                        entityManager.AddComponent(biologicalEntity, ComponentType.Create<CameraProperty>());
                        entityManager.SetComponentData(biologicalEntity, new CameraProperty
                        {
                            Target = new Vector3(data[i].X, data[i].Y, data[i].Z),
                            Damping = 3,
                            Offset = new Vector3(0, 1, -15),
                            RoationOffset = new Vector3(50, 0, 0)
                        });
                    }
                    else
                    {
                        entityManager.AddComponent(biologicalEntity, ComponentType.Create<NpcInput>());
                        entityManager.SetComponentData(biologicalEntity, new NpcInput
                        {
                            Movetend = TendType.None,
                            RandomSeed = 1,
                            BehaviorPolicy = BehaviorPolicyType.Cruising,
                            BehaviorTime = 1
                           
                        });
                    }
                    

                    ////初始这个Biological的Relation
                    //if (data[i].RelationId > 0)
                    //{
                    //    List<RelationData> relationDatas= SqlData.GetWhereDatas<RelationData>(" MainId=? ", new object[] { data[i].RelationId });
                    //    //entityManager.AddComponent(biologicalEntity,ComponentType.Create<Relation>());
                    //    //entityManager.SetComponentData(biologicalEntity,new Relation
                    //    //{

                    //    //});
                    //}
                    //entityManager.AddComponent(biologicalEntity, ComponentType.Create<Faction>());
                    //entityManager.SetComponentData(biologicalEntity, new Faction
                    //{
                    //    FactionId = data[i].RelationId,
                    //});


                    //entityManager.AddComponent(biologicalEntity, ComponentType.Create<Family>());
                    //entityManager.SetComponentData(biologicalEntity, new Family
                    //{
                    //    FamilyId = data[i].FamilyId,
                    //    ThisId = data[i].Id
                    //});
                    //entityManager.SetComponentData(biologicalEntity,new Relation());
                    //entityManager.AddComponent(biologicalEntity, ComponentType.Create<Relation>());

                    //Save Text
                    GameStaticData.BiologicalNameDic.Add(data[i].Id, data[i].Name);
                    GameStaticData.BiologicalSurnameDic.Add(data[i].Id, data[i].Surname);
                    GameStaticData.BiologicalDescription.Add(data[i].Id, data[i].Description);
                }
               
                GameStaticData.BiologicalSex.Add(1, "男");
                GameStaticData.BiologicalSex.Add(2, "女");
                GameStaticData.BiologicalSex.Add(3, "未知");

                RelationSystem.SetupComponentData(entityManager);
            }
            #endregion

            #region Relation
            {
                List<RelationData> relationDatas = SqlData.GetAllDatas<RelationData>();
                for (int i = 0; i < relationDatas.Count; i++)
                {
                    Entity entity = entityManager.CreateEntity(RelationArchetype);
                    entityManager.SetComponentData(entity, new Relation
                    {
                        ObjectAid = relationDatas[i].ObjectAid,
                        ObjectBid = relationDatas[i].ObjectBid,
                        Value = relationDatas[i].Value
                    });

                }

            }
            #endregion


            #region TechniquesData
            {
                List<TechniquesData> techniquesDatas = SqlData.GetAllDatas<TechniquesData>();
                for (int i = 0; i < techniquesDatas.Count; i++)
                {
                    Entity techniques = entityManager.CreateEntity(TechniquesArchetype);
                    entityManager.SetComponentData(techniques, new Techniques
                    {
                        Id = techniquesDatas[i].Id,
                        ParentId = techniquesDatas[i].ParentId,
                    });

                    GameStaticData.TechniquesName.Add(techniquesDatas[i].Id,techniquesDatas[i].Name);
                    GameStaticData.TechniquesDescription.Add(techniquesDatas[i].Id,techniquesDatas[i].Description);
                    GameStaticData.TechniqueSprites.Add(techniquesDatas[i].Id,Resources.Load<Sprite>(techniquesDatas[i].AvatarPath));
                }
            }
            #endregion

            #region FactionData
            {

                EntityArchetype factionArchetype = entityManager.CreateArchetype(typeof(Faction));
                List<FactionData> factionDatas = SqlData.GetAllDatas<FactionData>();
                for (int i = 0; i < factionDatas.Count; i++)
                {
                    Entity faction = entityManager.CreateEntity(factionArchetype);
                    entityManager.SetComponentData(faction, new Faction
                    {
                        Id = factionDatas[i].Id,
                        Level = factionDatas[i].FactionLevel,
                        Type = factionDatas[i].FactionType,
                        Food = factionDatas[i].Food,
                        FoodMax = factionDatas[i].FoodMax,
                        Iron = factionDatas[i].Iron,
                        IronMax = factionDatas[i].IronMax,
                        Money = factionDatas[i].Money,
                        MoneyMax = factionDatas[i].MoneyMax,
                        Wood = factionDatas[i].Wood,
                        WoodMax = factionDatas[i].WoodMax
                    });

                    GameStaticData.FactionName.Add(factionDatas[i].Id, factionDatas[i].Name);
                    GameStaticData.FactionDescription.Add(factionDatas[i].Id, factionDatas[i].Description);
                }
            }
            #endregion

            #region FamilySystem
            {
                EntityArchetype familyArchetype = entityManager.CreateArchetype(typeof(Family));
                List<FamilyData> familyData = SqlData.GetAllDatas<FamilyData>();
                for (int i = 0; i < familyData.Count; i++)
                {
                    Entity family = entityManager.CreateEntity(familyArchetype);
                    entityManager.SetComponentData(family,new Family
                    {
                        FamilyId = familyData[i].Id
                    });
                }
            }

            #endregion

            


            #region UiInit
            {
                UICenterMasterManager.Instance.ShowWindow(WindowID.MessageWindow);
                UICenterMasterManager.Instance.ShowWindow(WindowID.MenuWindow);
                UICenterMasterManager.Instance.ShowWindow(WindowID.TipsWindow);
                UICenterMasterManager.Instance.ShowWindow(WindowID.FixedTitleWindow);
                UICenterMasterManager.Instance.ShowWindow(WindowID.StrategyWindow);

              //  WorldTimeManager.Instance.AddTimerNode(DateTime.Now.AddHours(2),Test1, DateTime.Now.AddHours(6),Test2);

            }
            #endregion

            var sceneSwitcher = GameObject.Find("SceneSwitcher");
            if (sceneSwitcher != null)
            {
                NewGame();
            }
        }


        private static void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            InitializeWithScene();
        }

        private static MeshInstanceRenderer GetLookFromPrototype(string protoName)
        {
            var proto = GameObject.Find(protoName);
            var result = proto.GetComponent<MeshInstanceRendererComponent>().Value;
            Object.Destroy(proto);
            return result;
        }

    }

}



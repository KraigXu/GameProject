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
using Manager;
using Newtonsoft.Json;
using Object = UnityEngine.Object;

namespace GameSystem
{
    public sealed class StrategySceneInit
    {
        public static EntityArchetype DistrictArchetype;
        public static EntityArchetype TechniquesArchetype;
        public static EntityArchetype RelationArchetype;
        public static EntityArchetype LivingAreaEnterArchetype;
        public static EntityArchetype BiologicalArchetype;

        public static MeshInstanceRenderer BiologicalNormalLook;
        public static MeshInstanceRenderer BiologicalManLook;
        public static MeshInstanceRenderer BiologicalFemaleLook;


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

            TechniquesArchetype = entityManager.CreateArchetype(typeof(Techniques));
            RelationArchetype = entityManager.CreateArchetype(typeof(Relation));
            BiologicalArchetype = entityManager.CreateArchetype(typeof(Position), typeof(Rotation), typeof(Biological), typeof(BiologicalStatus),typeof(Team));

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

            //BuildingJsonData b=new BuildingJsonData();
            //b.GroupId = 1;
            //b.Item.Add(new BuildingItem(1,1,3,1,1,100));
            //b.Item.Add(new BuildingItem(1, 2, 3, 1, 1, 100));
            //b.Item.Add(new BuildingItem(1, 3, 3, 1, 1, 100));
            //b.Item.Add(new BuildingItem(1, 4, 3, 1, 1, 100));
            //b.Item.Add(new BuildingItem(1, 5, 3, 1, 1, 100));

            //Debug.Log(JsonConvert.SerializeObject(b));
            //TechniqueJsonData techniqueJson=new TechniqueJsonData();
            //techniqueJson.Id = 1;
            //techniqueJson.Content.Add(new KeyValuePair<int, int>(1,500));
            //techniqueJson.Content.Add(new KeyValuePair<int, int>(7, 300));
            //techniqueJson.Content.Add(new KeyValuePair<int, int>(9, 800));

            //Debug.Log(JsonConvert.SerializeObject(techniqueJson));


            BiologicalNormalLook = GetLookFromPrototype("BiologicalNormalLook");
            BiologicalManLook = GetLookFromPrototype("BiologicalManLook");
            BiologicalFemaleLook = GetLookFromPrototype("BiologicalFemaleLook");

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

                DateTime time = Convert.ToDateTime("1000-01-01 00:00:00");
                WorldTimeManager.Instance.Init(time);
                WorldTimeSystem.InitTimeData(time);



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

                    entityManager.AddComponent(district, ComponentType.Create<PeriodTime>());
                    entityManager.SetComponentData(district, new PeriodTime
                    {
                        Value = 0,
                        Type = PeriodType.Shichen
                    });
                }
                GameStaticData.DistrictStatusDsc.Add(0, "11");
                GameStaticData.DistrictStatusDsc.Add(1, "11");
                GameStaticData.DistrictStatusDsc.Add(2, "11");
                GameStaticData.DistrictTypeDsc.Add(0, "1");
            }
            #endregion

            #region LivingAreaInit
            {
                List<LivingAreaData> data = SqlData.GetAllDatas<LivingAreaData>();
                for (int i = 0; i < data.Count; i++)
                {
                    var go = GameObject.Instantiate(GameStaticData.ModelPrefab[data[i].ModelBaseId], new Vector3(data[i].PositionX, data[i].PositionY, data[i].PositionZ), Quaternion.identity);
                    Entity entity = go.GetComponent<GameObjectEntity>().Entity;

                    entityManager.AddComponent(entity, ComponentType.Create<LivingArea>());

                    LivingArea livingArea = new LivingArea();

                    livingArea.Id = data[i].Id;
                    livingArea.PowerId = data[i].PowerId;
                    livingArea.ModelBaseId = data[i].ModelBaseId;
                    livingArea.ModelId = data[i].ModelMain;
                    livingArea.PersonNumber = data[i].PersonNumber;
                    livingArea.CurLevel = data[i].LivingAreaLevel;
                    livingArea.MaxLevel = data[i].LivingAreaMaxLevel;
                    livingArea.TypeId = data[i].LivingAreaType;
                    livingArea.Money = data[i].Money;
                    livingArea.MoneyMax = data[i].MoneyMax;
                    livingArea.Iron = data[i].Iron;
                    livingArea.IronMax = data[i].IronMax;
                    livingArea.Wood = data[i].Wood;
                    livingArea.WoodMax = data[i].WoodMax;
                    livingArea.Food = data[i].Food;
                    livingArea.FoodMax = data[i].FoodMax;
                    livingArea.DefenseStrength = data[i].DefenseStrength;
                    livingArea.StableValue = data[i].StableValue;
                    livingArea.Renown = data[i].StableValue;
                    livingArea.Position = new Vector3(data[i].PositionX, data[i].PositionY, data[i].PositionZ);

                    if (string.IsNullOrEmpty(data[i].BuildingInfoJson) == false)
                    {
                        BuildingJsonData jsonData = JsonConvert.DeserializeObject<BuildingJsonData>(data[i].BuildingInfoJson);
                        BuildingSystem.SetData(entityManager, jsonData, livingArea.Id);
                        livingArea.BuildGroupId = jsonData.GroupId;
                    }

                    if (livingArea.ModelId != 0)
                    {
                        GameObject livingGo = GameObject.Instantiate(GameStaticData.ModelPrefab[livingArea.ModelId]);
                        livingGo.transform.SetParent(go.transform);
                        livingGo.SetActive(false);

                        Renderer[] renderers = livingGo.transform.GetComponentsInChildren<Renderer>();
                        Bounds bounds = renderers[0].bounds;
                        for (int j = 1; j < renderers.Length; j++)
                        {
                            bounds.Encapsulate(renderers[j].bounds);
                        }
                        livingArea.ModelPoint = bounds.center;
                    }

                    entityManager.SetComponentData(entity, livingArea);

                    entityManager.AddComponent(entity, ComponentType.Create<Position>());
                    entityManager.SetComponentData(entity, new Position
                    {
                        Value = new float3(data[i].PositionX, data[i].PositionY, data[i].PositionZ)
                    });

                    entityManager.AddComponent(entity, ComponentType.Create<InteractionElement>());
                    entityManager.SetComponentData(entity, new InteractionElement
                    {
                        Position = new Vector3(data[i].PositionX, data[i].PositionY, data[i].PositionZ),
                        Distance = 1,
                        Id = data[i].Id,
                        InteractionType = LocationType.LivingAreaIn,
                        InteractionExitType = LocationType.LivingAreaExit,
                        InteractionEnterType = LocationType.LivingAreaEnter,
                        Type = ElementType.LivingArea,
                    });

                    entityManager.AddComponent(entity, ComponentType.Create<PeriodTime>());
                    entityManager.SetComponentData(entity, new PeriodTime
                    {
                        Type = PeriodType.Month
                    });

                    GameStaticData.LivingAreaName.Add(data[i].Id, data[i].Name);
                    GameStaticData.LivingAreaDescription.Add(data[i].Id, data[i].Description);
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

                //建筑Model初始化
                BuildingSystem.InitModel(entityManager);
            }
            #endregion

            #region Boglogical
            {
                List<BiologicalData> data = SqlData.GetAllDatas<BiologicalData>();

                for (int i = 0; i < data.Count; i++)
                {
                    Entity entity = entityManager.CreateEntity(BiologicalArchetype);

                    entityManager.SetComponentData(entity, new Position
                    {
                        Value = new float3(data[i].X, data[i].Y, data[i].Z)
                    });

                    entityManager.SetComponentData(entity, new Rotation
                    {
                        Value = quaternion.identity
                    });

                    Biological biological = new Biological();
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
                    entityManager.SetComponentData(entity, biological);

                    BiologicalStatus biologicalStatus = new BiologicalStatus();
                    biologicalStatus.Position = new Vector3(data[i].X, data[i].Y, data[i].Z);
                    biologicalStatus.TargetId = 0;
                    biologicalStatus.TargetType = 0;
                    biologicalStatus.LocationType = (LocationType)data[i].LocationType;
                    entityManager.SetComponentData(entity, biologicalStatus);

                    Team team=new Team();
                    team.TeamBossId = data[i].TeamId;
                    team.RunModelCode = 0;
                    team.RunModelCode= ModelManager.Instance.AddModel(GameStaticData.ModelPrefab[data[i].ModelId], new Vector3(data[i].X, data[i].Y, data[i].Z));
                    entityManager.SetComponentData(entity,team);

                    GameStaticData.BiologicalNameDic.Add(data[i].Id, data[i].Name);
                    GameStaticData.BiologicalSurnameDic.Add(data[i].Id, data[i].Surname);
                    GameStaticData.BiologicalDescription.Add(data[i].Id, data[i].Description);
                }
            }
            #endregion


            #region Biological
            {
                List<BiologicalData> data = new List<BiologicalData>();
                for (int i = 0; i < data.Count; i++)
                {
                    var go = GameObject.Instantiate(GameStaticData.ModelPrefab[data[i].ModelId], new Vector3(data[i].X, data[i].Y, data[i].Z), quaternion.identity);
                    go.name = data[i].Id.ToString();
                    Entity biologicalEntity = go.GetComponent<GameObjectEntity>().Entity;

                    Biological biological = new Biological();
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
                    biological.TechniquesId = TechniquesSystem.SetData(data[i].JifaJson);

                    //初始Equipment
                    if (string.IsNullOrEmpty(data[i].EquipmentJson) == false)
                    {
                        EquipmentJsonData jsonData = JsonConvert.DeserializeObject<EquipmentJsonData>(data[i].EquipmentJson);
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
                        entityManager.AddComponent(biologicalEntity, ComponentType.Create<Team>());
                        entityManager.SetComponentData(biologicalEntity, new Team
                        {
                            TeamBossId = data[i].TeamId,
                        });
                    }

                    if (data[i].Id == settings.PlayerId)
                    {
                        entityManager.AddComponent(biologicalEntity, ComponentType.Create<PlayerInput>());
                        entityManager.SetComponentData(biologicalEntity, new PlayerInput
                        {
                            MousePoint = Vector3.zero
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
                    //GameStaticData.BiologicalNameDic.Add(data[i].Id, data[i].Name);
                    //GameStaticData.BiologicalSurnameDic.Add(data[i].Id, data[i].Surname);
                    //GameStaticData.BiologicalDescription.Add(data[i].Id, data[i].Description);
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

                    GameStaticData.TechniquesName.Add(techniquesDatas[i].Id, techniquesDatas[i].Name);
                    GameStaticData.TechniquesDescription.Add(techniquesDatas[i].Id, techniquesDatas[i].Description);
                    GameStaticData.TechniqueSprites.Add(techniquesDatas[i].Id, Resources.Load<Sprite>(techniquesDatas[i].AvatarPath));
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
                    entityManager.SetComponentData(family, new Family
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


            #region Camera
            {
                List<BiologicalData> data = SqlData.GetAllDatas<BiologicalData>();
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].Id == settings.PlayerId)
                    {
                        StrategyCameraManager.Instance.SetTarget(new Vector3(data[i].X, data[i].Y, data[i].Z), true);
                    }
                }
                data=null;
               
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



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

namespace GameSystem
{
    public sealed class StrategySceneInit
    {
        public static EntityArchetype DistrictArchetype;
        public static EntityArchetype LivingAreaArchetype;
        public static EntityArchetype BiologicalArchetype;
        public static EntityArchetype BuildingArchetype;
        public static EntityArchetype PlayerArchetype;
        public static EntityArchetype CameraArchetype;
        public static EntityArchetype TimeArchetype;

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
            Debuger.EnableLog = true;
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            CameraArchetype = entityManager.CreateArchetype(typeof(CameraProperty));

            DistrictArchetype = entityManager.CreateArchetype(typeof(District));

            BuildingArchetype = entityManager.CreateArchetype(typeof(Building));
            PlayerArchetype = entityManager.CreateArchetype(typeof(Biological), typeof(PlayerInput), typeof(Position), typeof(Rotation), typeof(AICharacterControl), typeof(Transform), typeof(NavMeshAgent), typeof(ThirdPersonCharacter));
            BiologicalArchetype = entityManager.CreateArchetype(typeof(Biological), typeof(AICharacterControl));
            BiologicalArchetype = entityManager.CreateArchetype(typeof(Biological), typeof(Position), typeof(NavMeshAgent));
            BiologicalArchetype = entityManager.CreateArchetype(typeof(Biological), typeof(Rigidbody), typeof(Transform), typeof(CapsuleCollider), typeof(NavMeshAgent));

            TimeArchetype = entityManager.CreateArchetype(typeof(TimeData));
        }


        /// <summary>
        /// 在场景加载后初始化数据
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializeAfterSceneLoad()
        {
            var settingsGo = GameObject.Find("Settings");

            if (settingsGo == null)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                return;
            }
            else
            {
                Settings = settingsGo?.GetComponent<DemoSetting>();
                if (!Settings) return;
            }

            InitializeWithScene();
        }

        // Begin a new game.
        public static void NewGame()
        {
        }

        public static void InitializeWithScene()
        {

            //PlayerLook = GetLookFromPrototype("PlayerRenderPrototype");
            // BiologicalLook = GetLookFromPrototype("BiologicalRenderPrototype");
            //LivingAreaLook = GetLookFromPrototype("LivingAreaRenderPrototype");
            //PlayerShotLook = GetLookFromPrototype("PlayerShotRenderPrototype");
            //EnemyShotLook = GetLookFromPrototype("EnemyShotRenderPrototype");
            //EnemyLook = GetLookFromPrototype("EnemyRenderPrototype");
            //EnemySpawnSystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());

            //StrategySystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());

            //LivingAreaBuildingSystem     --暂无开启

            //BiologicalSystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());
            // World.Active.GetOrCreateManager<UpdatePlayerHUD>().SetupGameObjects();
            //World.Active.GetOrCreateManager<StrategySystem>().Update();

            //获取配置
            DemoSetting settings = GameObject.Find("Settings").GetComponent<DemoSetting>();
            // Access the ECS entity manager
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            WorldTimeSystem.CurWorldTime = settings.curTime;

            #region Time
            {
                Debuger.Log("PlayProject Read Start  ");
                





                Debuger.Log("PlayProject Read End");
            }

            #endregion

            #region Dic
            {
                GameStaticData.BiologicalSex.Add(1, "男");
                GameStaticData.BiologicalSex.Add(2, "女");
                GameStaticData.BiologicalSex.Add(3, "未知");

                GameStaticData.LivingAreaLevel.Add(0, "0级");
                GameStaticData.LivingAreaLevel.Add(1, "1级");
                GameStaticData.LivingAreaLevel.Add(2, "2级");
                GameStaticData.LivingAreaLevel.Add(3, "3级");
                GameStaticData.LivingAreaLevel.Add(4, "4级");
                GameStaticData.LivingAreaLevel.Add(5, "5级");

                GameStaticData.LivingAreaType.Add(0, "Type1");
                GameStaticData.LivingAreaType.Add(1, "Type2");
                GameStaticData.LivingAreaType.Add(2, "Type3");
                GameStaticData.LivingAreaType.Add(3, "Type4");
                GameStaticData.LivingAreaType.Add(4, "Type5");
                GameStaticData.LivingAreaType.Add(5, "Type6");

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

                List<SocialDialogData> socialDialogDatas = SqlData.GetAllDatas<SocialDialogData>();

                for (int i = 0; i < socialDialogDatas.Count; i++)
                {
                    if (socialDialogDatas[i].Type == 1)
                    {
                        GameStaticData.SocialDialogInfo.Add(socialDialogDatas[i].Id, socialDialogDatas[i].Content);
                    }
                    else if (socialDialogDatas[i].Type == 2)
                    {
                        GameStaticData.SocialDialogNarration.Add(socialDialogDatas[i].Id, socialDialogDatas[i].Content);
                    }
                }
            }

            #endregion

            #region Model
            {

                List<ModelData> modelDatas = SqlData.GetAllDatas<ModelData>();
                for (int i = 0; i < modelDatas.Count; i++)
                {
                    GameStaticData.ModelPrefab.Add(modelDatas[i].Id, Resources.Load<GameObject>(modelDatas[i].Path));
                }
            }
            #endregion


            #region 初始化声望
            {
                List<PrestigeData> prestigeDatas = SqlData.GetAllDatas<PrestigeData>();

                List<int> max = new List<int>();
                List<int> min = new List<int>();
                List<int> level = new List<int>();
                for (int i = 0; i < prestigeDatas.Count; i++)
                {
                    GameStaticData.PrestigeBiolgicalDic.Add(prestigeDatas[i].LevelCode, prestigeDatas[i].BiologicalTitle);
                    GameStaticData.PrestigeDistrictDic.Add(prestigeDatas[i].LevelCode, prestigeDatas[i].DistrictTitle);
                    GameStaticData.PrestigeLivingAreaDic.Add(prestigeDatas[i].LevelCode, prestigeDatas[i].LivingAreaTitle);
                    max.Add(prestigeDatas[i].ValueMax);
                    min.Add(prestigeDatas[i].ValueMin);
                    level.Add(prestigeDatas[i].LevelCode);
                }
                PrestigeSystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>(), max, min, level);
            }
            #endregion

            #region 初始化派系

            {
                List<FactionData> factionDatas = SqlData.GetAllDatas<FactionData>();

                for (int i = 0; i < factionDatas.Count; i++)
                {
                    GameStaticData.FactionName.Add(factionDatas[i].Id, factionDatas[i].Name);
                }

            }


            #endregion

            #region 初始化家族

            {
                List<FamilyData> familyDatas = SqlData.GetAllDatas<FamilyData>();


                for (int i = 0; i < familyDatas.Count; i++)
                {
                    GameStaticData.FamilyName.Add(familyDatas[i].Id, familyDatas[i].Name);
                }
            }
            #endregion

            #region 初始化关系

            {
                List<RelationData> biologicalRelations = SqlData.GetAllDatas<RelationData>();

                for (int i = 0; i < biologicalRelations.Count; i++)
                {

                }
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
                    var go = GameObject.Instantiate(GameStaticData.ModelPrefab[livingAreaDatas[i].ModelMain] , new Vector3(livingAreaDatas[i].PositionX, livingAreaDatas[i].PositionY, livingAreaDatas[i].PositionZ), Quaternion.identity);

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
                        Position = new Vector3(livingAreaDatas[i].PositionX, livingAreaDatas[i].PositionY, livingAreaDatas[i].PositionZ)
                        

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
            }
            #endregion

            #region BiologicalInit
            {
                List<BiologicalData> data = SqlData.GetWhereDatas<BiologicalData>(" IsDebut=? ", new object[] { 1 });

                for (int i = 0; i < data.Count; i++)
                {
                    var go = GameObject.Instantiate(GameStaticData.ModelPrefab[data[i].ModelId], new Vector3(data[i].X, data[i].Y, data[i].Z), quaternion.identity);
                    go.name = data[i].Id.ToString();
                    Entity biologicalEntity = go.GetComponent<GameObjectEntity>().Entity;

                    entityManager.AddComponent(biologicalEntity, ComponentType.Create<Biological>());
                    entityManager.SetComponentData(biologicalEntity, new Biological
                    {
                        BiologicalId = data[i].Id,
                        AvatarId = data[i].AvatarId,
                        ModelId = data[i].ModelId,
                        PrestigeId = data[i].PrestigeId,
                        RelationId = data[i].RelationId,
                        FamilyId = data[i].FamilyId,
                        FactionId = data[i].FactionId,
                        TitleId = data[i].TitleId,

                        SexId = data[i].Sex,
                        Age = data[i].Age,
                        AgeMax = data[i].AgeMax,
                        Disposition = data[i].Disposition,

                        Tizhi = data[i].Tizhi,
                        Lidao = data[i].Lidao,
                        Jingshen = data[i].Jingshen,
                        Lingdong = data[i].Lingdong,
                        Wuxing = data[i].Wuxing



                    });

                    entityManager.AddComponent(biologicalEntity, ComponentType.Create<BiologicalStatus>());
                    entityManager.SetComponentData(biologicalEntity, new BiologicalStatus
                    {
                        Position = new Vector3(data[i].X, data[i].Y, data[i].Z),
                        TargetId = 0,
                        TargetType = 0,
                        LocationType = LocationType.Field,
                        PrestigeValue = 100

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

                    if (data[i].FamilyId != 0)
                    {
                        entityManager.AddComponent(biologicalEntity, ComponentType.Create<Family>());
                        entityManager.SetComponentData(biologicalEntity, new Family
                        {
                            FamilyId = data[i].FamilyId,
                            ThisId = data[i].Id
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

                    //Save Text
                    GameStaticData.BiologicalNameDic.Add(data[i].Id, data[i].Name);
                    GameStaticData.BiologicalSurnameDic.Add(data[i].Id, data[i].Surname);
                    GameStaticData.BiologicalDescription.Add(data[i].Id, data[i].Description);
                }
            }
            #endregion

            {
                Entity entity = entityManager.CreateEntity(TimeArchetype);
                entityManager.SetComponentData(entity, new TimeData
                {
                    TimeScalar = 1,
                    Schedule = 0,
                    ScheduleCell = 5,
                });
            }

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



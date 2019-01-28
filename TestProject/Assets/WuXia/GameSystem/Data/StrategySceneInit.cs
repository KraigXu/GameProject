using System;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;
using GameSystem.Ui;
using Manager;
using Newtonsoft.Json;

namespace GameSystem
{
    public sealed class StrategySceneInit
    {
        public static EntityArchetype DistrictArchetype;
        public static EntityArchetype TechniquesArchetype;
        public static EntityArchetype RelationArchetype;
        public static EntityArchetype BiologicalArchetype;
        public static EntityArchetype EventInfotype;
        public static EntityArchetype LivingAreaArchetype;

        public static MeshInstanceRenderer BiologicalNormalLook;
        public static MeshInstanceRenderer BiologicalManLook;
        public static MeshInstanceRenderer BiologicalFemaleLook;
        public static MeshInstanceRenderer LivingAreaLook;

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
            LivingAreaArchetype = entityManager.CreateArchetype(typeof(Position), typeof(LivingArea), typeof(InteractionElement), typeof(Element), typeof(FloatingInfo));
            TechniquesArchetype = entityManager.CreateArchetype(typeof(Techniques));
            RelationArchetype = entityManager.CreateArchetype(typeof(Relation));
            BiologicalArchetype = entityManager.CreateArchetype(typeof(Biological), typeof(BiologicalStatus));
            EventInfotype = entityManager.CreateArchetype(typeof(EventInfo));

            Debug.Log("Initialize SqlData");
            SQLService.GetInstance("TD.db");

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

        public static void InitializeWithScene()
        {
            UICenterMasterManager.Instance.ShowWindow(WindowID.LoadingWindow);

            BiologicalNormalLook = GetLookFromPrototype("BiologicalNormalLook");
            BiologicalManLook = GetLookFromPrototype("BiologicalManLook");
            BiologicalFemaleLook = GetLookFromPrototype("BiologicalFemaleLook");
            LivingAreaLook = GetLookFromPrototype("LivingAreaNormalLook");

            // Access the ECS entity manager
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            RelationSystem.SetupComponentData(entityManager);
            SocialDialogSystem.SetupComponentData(entityManager);
            PrestigeSystem.SetupComponentData(entityManager);

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
                //List<AvatarData> avatarDatas = SQLService.Instance.QueryAll<AvatarData>();
                //for (int i = 0; i < avatarDatas.Count; i++)
                //{
                //    if (avatarDatas[i].Type == 1)
                //    {
                //        GameStaticData.BiologicalAvatar.Add(avatarDatas[i].Id, Resources.Load<Sprite>(avatarDatas[i].Path));
                //    }
                //    else if (avatarDatas[i].Type == 2)
                //    {
                //        GameStaticData.BuildingAvatar.Add(avatarDatas[i].Id, Resources.Load<Sprite>(avatarDatas[i].Path));
                //    }
                //}

                List<BiologicalAvatarData> biologicalAvatarDatas = SQLService.Instance.QueryAll<BiologicalAvatarData>();
                for (int i = 0; i < biologicalAvatarDatas.Count; i++)
                {
                    GameStaticData.BiologicalAvatar.Add(biologicalAvatarDatas[i].Id, Resources.Load<Sprite>(biologicalAvatarDatas[i].Path));
                }

                List<ModelData> modelDatas = SQLService.Instance.QueryAll< ModelData>();
                for (int i = 0; i < modelDatas.Count; i++)
                {
                    GameStaticData.ModelPrefab.Add(modelDatas[i].Id, Resources.Load<GameObject>(modelDatas[i].Path));
                }
            }

            #endregion

            #region DistrictInit
            {
                //List<DistrictData> districtDatas = SQLService.Instance.QueryAll<DistrictData>();

                //for (int i = 0; i < districtDatas.Count; i++)
                //{
                //    GameObject go = GameObject.Instantiate(GameStaticData.ModelPrefab[districtDatas[i].Model], new Vector3(districtDatas[i].X, districtDatas[i].Y, districtDatas[i].Z), Quaternion.identity);

                //    DistrictRange districtRange = go.GetComponent<DistrictRange>();
                //    districtRange.DistrictId = districtDatas[i].Id;

                //    Entity district = go.GetComponent<GameObjectEntity>().Entity;
                //    entityManager.AddComponent(district, ComponentType.Create<District>());
                //    entityManager.SetComponentData(district, new District
                //    {
                //        Id = districtDatas[i].Id,
                //        Type = districtDatas[i].Type,
                //        ProsperityLevel = districtDatas[i].ProsperityLevel,
                //        TrafficLevel = districtDatas[i].TrafficLevel,
                //        GrowingModulus = districtDatas[i].GrowingModulus,
                //        SecurityModulus = districtDatas[i].SecurityModulus
                //    });

                //    GameStaticData.DistrictName.Add(districtDatas[i].Id, districtDatas[i].Name);
                //    GameStaticData.DistrictDescriptione.Add(districtDatas[i].Id, districtDatas[i].Description);

                //    entityManager.AddComponent(district, ComponentType.Create<PeriodTime>());
                //    entityManager.SetComponentData(district, new PeriodTime
                //    {
                //        Value = 0,
                //        Type = PeriodType.Shichen
                //    });
                //}
                //GameStaticData.DistrictStatusDsc.Add(0, "11");
                //GameStaticData.DistrictStatusDsc.Add(1, "11");
                //GameStaticData.DistrictStatusDsc.Add(2, "11");
                //GameStaticData.DistrictTypeDsc.Add(0, "1");
            }
            #endregion

            #region LivingAreaInit
            {
                List<LivingAreaData> datas = SQLService.Instance.QueryAll<LivingAreaData>();

                for (int i = 0; i < datas.Count; i++)
                {
                    Entity entity = entityManager.CreateEntity(LivingAreaArchetype);

                    entityManager.SetComponentData(entity, new Position
                    {
                        Value = new float3(datas[i].PositionX, datas[i].PositionY, datas[i].PositionZ)
                    });

                    entityManager.SetComponentData(entity, new LivingArea
                    {
                        Id = datas[i].Id,
                        PersonNumber = datas[i].PersonNumber,
                        Type = (LivingAreaType)datas[i].LivingAreaType,
                        Money = datas[i].Money,
                        MoneyMax = datas[i].MoneyMax,
                        Iron = datas[i].Iron,
                        IronMax = datas[i].IronMax,
                        Wood = datas[i].Wood,
                        WoodMax = datas[i].WoodMax,
                        Food = datas[i].Food,
                        FoodMax = datas[i].FoodMax,
                        DefenseStrength = datas[i].DefenseStrength,
                        StableValue = datas[i].StableValue
                    });

                    entityManager.SetComponentData(entity, new InteractionElement
                    {
                        Distance = 1,
                    });

                    entityManager.SetComponentData(entity, new Element
                    {
                        Type = ElementType.LivingArea,
                    });

                    //entityManager.SetComponentData(entity,new FloatingInfo());

                    entityManager.AddSharedComponentData(entity, LivingAreaLook);
                    GameStaticData.LivingAreaName.Add(datas[i].Id, datas[i].Name);
                    GameStaticData.LivingAreaDescription.Add(datas[i].Id, datas[i].Description);
                }
            }
            #endregion


            //#region LivingAreaInit
            //{

            //    List<LivingAreaData> data = SQLService.Instance.QueryAll<LivingAreaData>();
            //    for (int i = 0; i < data.Count; i++)
            //    {
            //        var go = GameObject.Instantiate(GameStaticData.ModelPrefab[data[i].ModelBaseId], new Vector3(data[i].PositionX, data[i].PositionY, data[i].PositionZ), Quaternion.identity);
            //        Entity entity = go.GetComponent<GameObjectEntity>().Entity;

            //        entityManager.AddComponent(entity, ComponentType.Create<LivingArea>());

            //        LivingArea livingArea = new LivingArea();
            //        livingArea.Id = data[i].Id;



            //        livingArea.Id = data[i].Id;
            //        livingArea.PowerId = data[i].PowerId;
            //        livingArea.ModelBaseId = data[i].ModelBaseId;
            //        livingArea.ModelId = data[i].ModelMain;
            //        livingArea.PersonNumber = data[i].PersonNumber;
            //        livingArea.CurLevel = data[i].LivingAreaLevel;
            //        livingArea.MaxLevel = data[i].LivingAreaMaxLevel;
            //       // livingArea.TypeId = data[i].LivingAreaType;
            //        livingArea.Money = data[i].Money;
            //        livingArea.MoneyMax = data[i].MoneyMax;
            //        livingArea.Iron = data[i].Iron;
            //        livingArea.IronMax = data[i].IronMax;
            //        livingArea.Wood = data[i].Wood;
            //        livingArea.WoodMax = data[i].WoodMax;
            //        livingArea.Food = data[i].Food;
            //        livingArea.FoodMax = data[i].FoodMax;
            //        livingArea.DefenseStrength = data[i].DefenseStrength;
            //        livingArea.StableValue = data[i].StableValue;
            //       // livingArea.Renown = data[i].StableValue;
            //       // livingArea.Position = new Vector3(data[i].PositionX, data[i].PositionY, data[i].PositionZ);

            //        if (string.IsNullOrEmpty(data[i].BuildingInfoJson) == false)
            //        {
            //            BuildingJsonData jsonData = JsonConvert.DeserializeObject<BuildingJsonData>(data[i].BuildingInfoJson);
            //            BuildingSystem.SetData(entityManager, jsonData, livingArea.Id);
            //            livingArea.BuildGroupId = jsonData.GroupId;
            //        }

            //        if (livingArea.ModelId != 0)
            //        {
            //            GameObject livingGo = GameObject.Instantiate(GameStaticData.ModelPrefab[livingArea.ModelId]);
            //            livingGo.transform.SetParent(go.transform);
            //            livingGo.SetActive(false);

            //            Renderer[] renderers = livingGo.transform.GetComponentsInChildren<Renderer>();
            //            Bounds bounds = renderers[0].bounds;
            //            for (int j = 1; j < renderers.Length; j++)
            //            {
            //                bounds.Encapsulate(renderers[j].bounds);
            //            }
            //            livingArea.ModelPoint = bounds.center;
            //        }

            //        entityManager.SetComponentData(entity, livingArea);

            //        entityManager.AddComponent(entity, ComponentType.Create<Position>());
            //        entityManager.SetComponentData(entity, new Position
            //        {
            //            Value = new float3(data[i].PositionX, data[i].PositionY, data[i].PositionZ)
            //        });

            //        entityManager.AddComponent(entity, ComponentType.Create<InteractionElement>());
            //        entityManager.SetComponentData(entity, new InteractionElement
            //        {
            //            Position = new Vector3(data[i].PositionX, data[i].PositionY, data[i].PositionZ),
            //            Distance = 1,
            //            Id = data[i].Id,
            //            InteractionType = LocationType.LivingAreaIn,
            //            InteractionExitType = LocationType.LivingAreaExit,
            //            InteractionEnterType = LocationType.LivingAreaEnter,
            //            Type = ElementType.LivingArea,
            //            EventCode = 1000,
            //        });

            //        entityManager.AddComponent(entity, ComponentType.Create<PeriodTime>());
            //        entityManager.SetComponentData(entity, new PeriodTime
            //        {
            //            Type = PeriodType.Month
            //        });

            //        GameStaticData.LivingAreaName.Add(data[i].Id, data[i].Name);
            //        GameStaticData.LivingAreaDescription.Add(data[i].Id, data[i].Description);
            //    }


            //    GameStaticData.LivingAreaLevel.Add(0, "0级");
            //    GameStaticData.LivingAreaLevel.Add(1, "1级");
            //    GameStaticData.LivingAreaLevel.Add(2, "2级");
            //    GameStaticData.LivingAreaLevel.Add(3, "3级");
            //    GameStaticData.LivingAreaLevel.Add(4, "4级");
            //    GameStaticData.LivingAreaLevel.Add(5, "5级");

            //    GameStaticData.LivingAreaType.Add(0, "城市");
            //    GameStaticData.LivingAreaType.Add(1, "帮派");
            //    GameStaticData.LivingAreaType.Add(2, "村落");
            //    GameStaticData.LivingAreaType.Add(3, "洞窟");
            //    GameStaticData.LivingAreaType.Add(4, "遗迹");
            //    GameStaticData.LivingAreaType.Add(5, "奇迹");

            //    //建筑Model初始化
            //    BuildingSystem.InitModel(entityManager);
            //}
            //#endregion
            #region Boglogical
            {
//                List<BiologicalData> data = SQLService.Instance.QueryAll<BiologicalData>();
//                for (int i = 0; i < data.Count; i++)
//                {
//                    Entity entity = entityManager.CreateEntity(BiologicalArchetype);

//                    Biological biological = new Biological();
//                    biological.BiologicalId = data[i].Id;
//                    biological.AvatarId = data[i].AvatarId;
//                    biological.ModelId = data[i].ModelId;
//                    biological.FamilyId = data[i].FamilyId;
//                    biological.FactionId = data[i].FactionId;
//                    biological.TitleId = data[i].TitleId;
//                    biological.SexId = data[i].Sex;
//                    biological.Age = data[i].Age;
//                    biological.AgeMax = data[i].AgeMax;
//                    biological.Disposition = data[i].Disposition;
//                    biological.PrestigeValue = data[i].PrestigeValue;
//                    biological.CharmValue = 0;
//                    biological.CharacterValue = 0;
//                    biological.NeutralValue = 0;
//                    biological.BodyValue = 0;
//                    biological.LuckValue = 0;
//                    biological.Tizhi = data[i].Tizhi;
//                    biological.Lidao = data[i].Lidao;
//                    biological.Jingshen = data[i].Jingshen;
//                    biological.Lingdong = data[i].Lingdong;
//                    biological.Wuxing = data[i].Wuxing;
//                    entityManager.SetComponentData(entity, biological);

//                    BiologicalStatus biologicalStatus = new BiologicalStatus();
//                    biologicalStatus.BiologicalIdentity = data[i].Identity;
//                    biologicalStatus.Position = new Vector3(data[i].X, data[i].Y, data[i].Z);
//                    biologicalStatus.Quaternion = Quaternion.identity;
//                    biologicalStatus.TargetId = 0;
//                    biologicalStatus.TargetType = 0;
//                    biologicalStatus.LocationType = (LocationType)data[i].LocationType;
//                    entityManager.SetComponentData(entity, biologicalStatus);

//                    Team team = new Team();
//                    team.TeamBossId = data[i].TeamId;
////                    team.RunModelCode = 0;
// //                   team.RunModelCode = SystemManager.Get<TeamSystem>().AddModel(GameStaticData.ModelPrefab[data[i].ModelId], new Vector3(data[i].X, data[i].Y, data[i].Z));
//                    entityManager.SetComponentData(entity, team);

//                    GameStaticData.BiologicalNameDic.Add(data[i].Id, data[i].Name);
//                    GameStaticData.BiologicalSurnameDic.Add(data[i].Id, data[i].Surname);
//                    GameStaticData.BiologicalDescription.Add(data[i].Id, data[i].Description);


//                }
//                GameStaticData.BiologicalSex.Add(1, "男");
//                GameStaticData.BiologicalSex.Add(2, "女");
//                GameStaticData.BiologicalSex.Add(3, "未知");
            }
            #endregion

            #region Character 信息初始

            {
                List<BiologicalData> datas = SQLService.Instance.QueryAll<BiologicalData>();

                for (int i = 0; i < datas.Count; i++)
                {
                    Entity entity = entityManager.CreateEntity(BiologicalArchetype);

                    
                    entityManager.AddComponent(entity,ComponentType.Create<Rotation>());
                    entityManager.SetComponentData(entity,new Rotation
                    {
                        Value =quaternion.identity
                    });

                    entityManager.AddComponent(entity,ComponentType.Create<Position>());
                    entityManager.SetComponentData(entity,new Position
                    {
                        Value = new Vector3(datas[i].X, datas[i].Y, datas[i].Z),
                    });

                    entityManager.SetComponentData(entity, new Biological()
                    {
                        BiologicalId = datas[i].Id,
                        Age = datas[i].Age,
                        AgeMax = datas[i].AgeMax,

                        Tizhi = datas[i].Tizhi,
                        Lidao = datas[i].Lidao,
                        Jingshen = datas[i].Jingshen,
                        Lingdong = datas[i].Lingdong,
                        Wuxing = datas[i].Wuxing,

                        //public int AvatarId;
                        //public int ModelId;
                        //public int FamilyId;
                        //public int FactionId;
                        //public int TitleId;

                        //public int SexId;
                        //public int Age;
                        //public int AgeMax;
                        //public int Disposition;
                        //public int PrestigeValue;
                        //public int CharmValue;
                        //public byte CharacterValue;     //性格值
                        //public byte NeutralValue;       //中立值
                        //public byte BodyValue;         //身体值
                        //public byte LuckValue;          //运气值

                        //public int Tizhi;
                        //public int Lidao;
                        //public int Jingshen;
                        //public int Lingdong;
                        //public int Wuxing;

                        //public int Jing;
                        //public int Qi;
                        //public int Shen;
                        //public int WaigongMin;
                        //public int WaigongMax;
                        //public int NeigongMin;
                        //public int NeigongMax;

                        //public int Mingzhong;
                        //public int Shanbi;
                        //public int Huixin;

                        //public int StrategyMoveSpeed;
                        //public int FireMoveSpeed;

                        //public int TechniquesId;

                        //public int EquipmentId;
                    });

                    BiologicalStatus biologicalStatus = new BiologicalStatus();
                    biologicalStatus.BiologicalIdentity = datas[i].Identity;
                    biologicalStatus.TargetId = 0;
                    biologicalStatus.TargetType = 0;
                    biologicalStatus.LocationType = (LocationType)datas[i].LocationType;
                    entityManager.SetComponentData(entity, biologicalStatus);

                    entityManager.AddComponent(entity, ComponentType.Create<Team>());
                    entityManager.SetComponentData(entity, new Team
                    {
                        TeamBossId = datas[i].TeamId
                    });

                    switch ((LocationType)datas[i].LocationType)
                    {
                        case LocationType.None:
                            break;
                        case LocationType.City:
                            {
                            }
                            break;
                        case LocationType.Field:
                            {
                                entityManager.AddComponent(entity,ComponentType.Create<ModelSpawnData>());
                                entityManager.SetComponentData(entity,new ModelSpawnData
                                {
                                    ModelData = new ModelComponent
                                    {
                                        Id = SystemManager.Get<ModelMoveSystem>().AddModel(GameStaticData.ModelPrefab[datas[i].ModelId], new Vector3(datas[i].X, datas[i].Y, datas[i].Z)),
                                        Target = Vector3.zero,
                                        Speed = 6,
                                },
                                });
                                
                                entityManager.AddComponent(entity, ComponentType.Create<InteractionElement>());
                                entityManager.SetComponentData(entity, new InteractionElement
                                {
                                    Distance = 1,
                                    ModelCode = 1,
                                });
                            }
                            break;
                    }

                    if (datas[i].Identity==0)
                    {
                        entityManager.AddComponent(entity,ComponentType.Create<NpcInput>());
                    }
                    else if (datas[i].Identity == 1)
                    {
                        entityManager.AddComponent(entity,ComponentType.Create<PlayerInput>());
                    }

                    entityManager.AddComponent(entity,ComponentType.Create<BehaviorData>());
                    entityManager.SetComponentData(entity,new BehaviorData
                    {

                    });


                    GameStaticData.BiologicalNameDic.Add(datas[i].Id, datas[i].Name);
                    GameStaticData.BiologicalSurnameDic.Add(datas[i].Id, datas[i].Surname);
                    GameStaticData.BiologicalDescription.Add(datas[i].Id, datas[i].Description);
                }
            }




            #endregion

            #region Relation
            {
                List<RelationData> relationDatas = SQLService.Instance.QueryAll<RelationData>();
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
                List<TechniquesData> techniquesDatas = SQLService.Instance.QueryAll<TechniquesData>();
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
                List<FactionData> factionDatas = SQLService.Instance.QueryAll<FactionData>();
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
                List<FamilyData> familyData = SQLService.Instance.QueryAll<FamilyData>();
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

                //常驻窗口
                UICenterMasterManager.Instance.ShowWindow(WindowID.FixedTitleWindow);

                UICenterMasterManager.Instance.ShowWindow(WindowID.StrategyWindow);

                //  WorldTimeManager.Instance.AddTimerNode(DateTime.Now.AddHours(2),Test1, DateTime.Now.AddHours(6),Test2);

            }
            #endregion

            #region Camera
            {
                List<BiologicalData> data = SQLService.Instance.QueryAll<BiologicalData>();
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].Id == Settings.PlayerId)
                    {
                        StrategyCameraManager.Instance.SetTarget(new Vector3(data[i].X, data[i].Y, data[i].Z), true);
                    }
                }
                data = null;

            }

            #endregion

            SystemManager.Get<PlayerControlSystem>().SetupInit();

            UICenterMasterManager.Instance.DestroyWindow(WindowID.LoadingWindow);

            LivingAreaSystem.SetupInfo();
        }

        private static MeshInstanceRenderer GetLookFromPrototype(string protoName)
        {
            var proto = GameObject.Find(protoName);
            var result = proto.GetComponent<MeshInstanceRendererComponent>().Value;
            UnityEngine.Object.Destroy(proto);
            return result;
        }

    }

}



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
using AntiquityWorld.StrategyManager;
using Unity.Collections;


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
        public static EntityArchetype BiologicalSocialArchetype;
        public static EntityArchetype AncientTombArchetype;
        public static EntityArchetype ArticleArchetype;

        public static MeshInstanceRenderer BiologicalNormalLook;
        public static MeshInstanceRenderer BiologicalManLook;
        public static MeshInstanceRenderer BiologicalFemaleLook;
        public static MeshInstanceRenderer LivingAreaLook;

        public static DemoSetting Settings;
        public static Entity PlayerEntity;

        public static Dictionary<Entity, GameObject> EcsGameObjectsDic = new Dictionary<Entity, GameObject>();

        public static FixedTitleWindow FixedTitleWindow;

        public static List<BuildingSystem> BuildingSystems=new List<BuildingSystem>();


        public static void InitializeWithScene()
        {
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
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            DistrictArchetype = entityManager.CreateArchetype(typeof(District));
            LivingAreaArchetype = entityManager.CreateArchetype(typeof(Position), typeof(Rotation), typeof(LivingArea));
            TechniquesArchetype = entityManager.CreateArchetype(typeof(Techniques));
            RelationArchetype = entityManager.CreateArchetype(typeof(Relation));
            BiologicalArchetype = entityManager.CreateArchetype(typeof(Element), typeof(Position), typeof(Rotation), typeof(Biological));
            EventInfotype = entityManager.CreateArchetype(typeof(EventInfo));
            BiologicalSocialArchetype = entityManager.CreateArchetype(typeof(BiologicalSocial));
            AncientTombArchetype = entityManager.CreateArchetype(typeof(Position), typeof(Rotation));
            ArticleArchetype = entityManager.CreateArchetype(typeof(ArticleItem));

            UICenterMasterManager.Instance.ShowWindow(WindowID.LoadingWindow);

            BiologicalNormalLook = GetLookFromPrototype("BiologicalNormalLook");
            BiologicalManLook = GetLookFromPrototype("BiologicalManLook");
            BiologicalFemaleLook = GetLookFromPrototype("BiologicalFemaleLook");
            LivingAreaLook = GetLookFromPrototype("LivingAreaNormalLook");


            RelationSystem.SetupComponentData(entityManager);
            SocialDialogSystem.SetupComponentData(entityManager);
            PrestigeSystem.SetupComponentData(entityManager);

            BuildingSystems.Add(SystemManager.Get<BuildingBlacksmithSystem>());
            


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

                List<ModelData> modelDatas = SQLService.Instance.QueryAll<ModelData>();
                for (int i = 0; i < modelDatas.Count; i++)
                {
                    GameStaticData.ModelPrefab.Add(modelDatas[i].Id, Resources.Load<GameObject>(modelDatas[i].Path));

                }
            }

            #endregion

            #region DistrictInit
            {
                List<DistrictData> datas = SQLService.Instance.QueryAll<DistrictData>();

                for (int i = 0; i < datas.Count; i++)
                {
                    GameObject go = GameObject.Instantiate(GameStaticData.ModelPrefab[datas[i].Model]);
                    go.transform.position = new Vector3(datas[i].X, datas[i].Y, datas[i].Z);

                    Entity entity = go.GetComponent<GameObjectEntity>().Entity;
                    entityManager.AddComponentData(entity, new District
                    {
                        Id = datas[i].Id,
                        GId = 324 + i,
                        Type = datas[i].Type,
                        ProsperityLevel = datas[i].ProsperityLevel,
                        TrafficLevel = datas[i].TrafficLevel,
                        GrowingModulus = datas[i].GrowingModulus,
                        SecurityModulus = datas[i].SecurityModulus
                    });

                    entityManager.AddComponentData(entity, new PeriodTime
                    {
                        Value = 0,
                        Type = PeriodType.Shichen,
                    });
                }

            }
            #endregion

            #region LivingAreaInit
            {
                List<LivingAreaData> datas = SQLService.Instance.QueryAll<LivingAreaData>();
                for (int i = 0; i < datas.Count; i++)
                {
                    Transform entityGo = WXPoolManager.Pools[Define.GeneratedPool].Spawn(GameStaticData.ModelPrefab[datas[i].ModelBaseId].transform);
                    entityGo.position = new float3(datas[i].PositionX, datas[i].PositionY, datas[i].PositionZ);
                    entityGo.gameObject.name = datas[i].Name;
                    ColliderTriggerEvent trigger = entityGo.gameObject.GetComponent<ColliderTriggerEvent>();

                    Entity entity = entityGo.GetComponent<GameObjectEntity>().Entity;

                    entityManager.AddComponentData(entity, new ModelInfo
                    {
                        ModelId = datas[i].ModelBaseId
                    });

                    entityManager.AddComponentData(entity, new LivingArea
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

                    entityManager.AddComponentData(entity, new Money
                    {
                        Value = datas[i].Money,
                        Upperlimit = datas[i].MoneyMax
                    });

                    if (datas[i].LivingAreaType == 1)
                    {
                        entityManager.AddComponentData(entity,new Crowd
                        {
                            Number = 300000
                        });
                        trigger.TriggerEnter = CitySystem.CityColliderEnter;
                        trigger.TriggerExit = CitySystem.CityColliderExit;

                    }
                    else if (datas[i].LivingAreaType == 2)
                    {
                        entityManager.AddComponentData(entity, new Collective()
                        {
                            CollectiveClassId = 1,
                            Cohesion = 1
                        });

                        trigger.TriggerEnter = OrganizationSystem.OrganizationColliderEnter;
                        trigger.TriggerExit = OrganizationSystem.OrganizationColliderExit;
                    }
                    else
                    {

                    }


                    Debug.Log(datas[i].BuildingInfoJson);
                    BuildingJsonData jsonData = JsonConvert.DeserializeObject<BuildingJsonData>(datas[i].BuildingInfoJson);

                    for (int j = 0; j < jsonData.Item.Count; j++)
                    {
                        var item = jsonData.Item[j];
                        if (item.Type == 0)
                        {

                        }else if (item.Type == 1)
                        {
                            if (SystemManager.Contains<BuildingBlacksmith>(entity) == false)
                            {
                                SystemManager.Get<BuildingBlacksmithSystem>().AddBuildingSystem(entity,item);
                            }

                        }
                    }


                    GameStaticData.LivingAreaName.Add(datas[i].Id, datas[i].Name);
                    GameStaticData.LivingAreaDescription.Add(datas[i].Id, datas[i].Description);

                }

                SystemManager.Get<LivingAreaSystem>().InitSystem();
            }
            #endregion

            #region Character 信息初始
            {

                List<BiologicalData> datas = SQLService.Instance.QueryAll<BiologicalData>();

                for (int i = 0; i < datas.Count; i++)
                {

                    Transform entityGo = WXPoolManager.Pools[Define.GeneratedPool].Spawn(GameStaticData.ModelPrefab[datas[i].ModelId].transform);
                    entityGo.position = new Vector3(datas[i].X, datas[i].Y, datas[i].Z);


                    Entity entity = entityGo.GetComponent<GameObjectEntity>().Entity;
                    entityManager.AddComponentData(entity, new Biological()
                    {
                        BiologicalId = datas[i].Id,
                        Age = datas[i].Age,
                        Sex = datas[i].Sex,
                        CharmValue = 0,
                        Mobility = 0,
                        OperationalAbility = 0,
                        LogicalThinking = 0,
                        Disposition = 100,
                        NeutralValue = 100,
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

                    entityManager.AddComponentData(entity, new BodyProperty
                    {
                        Thought = 100,
                        Neck = 100,
                        Heart = 100,
                        Eye = 100,
                        Ear = 100,
                        LeftLeg = 100,
                        RightLeg = 100,
                        LeftHand = 100,
                        RightHand = 100,
                        Fertility = 100,
                        Appearance = 100,
                        Dress = 100,
                        Skin = 100,

                        StrategyMoveSpeed = 6,
                        FireMoveSpeed = 10,

                        UpperLimit = 50000,//克
                    });




                    entityManager.AddComponent(entity, ComponentType.Create<Equipment>());
                    entityManager.SetComponentData(entity, new Equipment
                    {
                        HelmetId = -1,
                        ClothesId = -1,
                        BeltId = -1,
                        HandGuard = -1,
                        Pants = -1,
                        Shoes = -1,
                        WeaponFirstId = -1,
                        WeaponSecondaryId = -1
                    });

                    entityManager.AddComponent(entity, ComponentType.Create<Techniques>());
                    entityManager.SetComponentData(entity, new Techniques
                    {
                    });

                    entityManager.AddComponent(entity, ComponentType.Create<EquipmentCoat>());
                    entityManager.SetComponentData(entity, new EquipmentCoat
                    {
                        SpriteId = 1,
                        Type = EquipType.Coat,
                        Level = EquipLevel.General,
                        Part = EquipPart.All,
                        BluntDefense = 19,
                        SharpDefense = 20,
                        Operational = 100,
                        Weight = 3,
                        Price = 1233,
                    });



                    ////Entity entity = entityManager.CreateEntity(BiologicalArchetype);

                    //entityManager.SetComponentData(entity, new Element
                    //{
                    //    InnerId = datas[i].Id,
                    //    Type = ElementType.Biological
                    //});

                    //entityManager.SetComponentData(entity, new Rotation
                    //{
                    //    Value = quaternion.identity
                    //});

                    //entityManager.SetComponentData(entity, new Position
                    //{
                    //    Value = 
                    //});

                    //entityManager.AddComponent(entity, ComponentType.Create<Life>());
                    //entityManager.SetComponentData(entity, new Life
                    //{
                    //    Value = 100,
                    //});

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

                    entityManager.AddComponent(entity, ComponentType.Create<Team>());
                    entityManager.SetComponentData(entity, new Team
                    {
                        TeamBossId = datas[i].TeamId
                    });

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

                    if (datas[i].Identity == 0)
                    {
                        entityManager.AddComponent(entity, ComponentType.Create<NpcInput>());
                    }
                    else if (datas[i].Identity == 1)
                    {
                        entityManager.AddComponent(entity, ComponentType.Create<PlayerInput>());
                        entityGo.name = "PlayerMain";
                        PlayerEntity = entity;

                        SystemManager.Get<PlayerControlSystem>().InitPlayerEvent(entityGo.gameObject);
                    }

                    entityManager.AddComponent(entity, ComponentType.Create<BehaviorData>());
                    entityManager.SetComponentData(entity, new BehaviorData
                    {
                        Target = Vector3.zero,
                    });

                    GameStaticData.BiologicalNameDic.Add(datas[i].Id, datas[i].Name);
                    GameStaticData.BiologicalSurnameDic.Add(datas[i].Id, datas[i].Surname);
                    GameStaticData.BiologicalDescription.Add(datas[i].Id, datas[i].Description);

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
                    SystemManager.Get<BiologicalSystem>().InitComponent(entityGo.gameObject);
                }

            }

            #endregion

            #region Article
            {
                //    List<ArticleRecordingData> datas = SQLService.Instance.QueryAll<ArticleRecordingData>();
                List<ArticleRecordingData> datas = new List<ArticleRecordingData>();

                //datas=new List<ArticleRecordingData>();

                //datas.Add(new ArticleRecordingData
                //{
                //    Id = 1,
                //    Attribute = "0x0A4d2,0x0A4d2,0x0A4d2,0x0B4d2,0x0B4d2,0x0B4d2",
                //    GuiId = 50295,
                //    ItemDesc = "SSSSSSSSSSSSSSSSSSSSSS",
                //    Count = 10,
                //    MaxCount = 230,
                //    Type = 3,
                //    State = 3,
                //});
                //datas.Add(new ArticleRecordingData
                //{
                //    Id = 2,
                //    Attribute = "0x0A4d2,0x0A4d2,0x0A4d2,0x0B4d2,0x0B4d2,0x0B4d2",
                //    GuiId = 50295,
                //    ItemDesc = "SSSSSSSSSSSSSSSSSSSSSS",
                //    Count = 10,
                //    MaxCount = 230,
                //    Type = 3,
                //    State = 3,
                //});
                //datas.Add(new ArticleRecordingData
                //{
                //    Id = 3,
                //    Attribute = "0x0A4d2,0x0A4d2,0x0A4d2,0x0B4d2,0x0B4d2,0x0B4d2",
                //    GuiId = 50295,
                //    ItemDesc = "SSSSSSSSSSSSSSSSSSSSSS",
                //    Count = 10,
                //    MaxCount = 230,
                //    Type = 3,
                //    State = 3,
                //});
                //datas.Add(new ArticleRecordingData
                //{
                //    Id = 4,
                //    Attribute = "0x0A4d2,0x0A4d2,0x0A4d2,0x0B4d2,0x0B4d2,0x0B4d2",
                //    GuiId = 50295,
                //    ItemDesc = "SSSSSSSSSSSSSSSSSSSSSS",
                //    Count = 10,
                //    MaxCount = 230,
                //    Type = 3,
                //    State = 3,
                //});


                //  Convert.ToInt32();
                //Debug.Log(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_RECOVER_LIFE);
                //Debug.Log(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_RECOVER_LIFE.GetHashCode());
                //  Debug.Log(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_RECOVER_LIFE.);
                //+Convert.ToString(9801, 16)

                int id = Convert.ToInt32("0x0A", 16);
                //  Debug.Log((ENUM_ITEM_ATTRIBUTE)id);
                //Debug.Log("0x0B" + Convert.ToString(10, 16));
                //Debug.Log("0x0B" + Convert.ToString(999, 16));
                //Debug.Log("0x0B" + Convert.ToString(230, 16));
                //Debug.Log("0x0B" + Convert.ToString(1234, 16));
                //Debug.Log(Convert.ToInt32(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_RECOVER_LIFE).ToString())+">>"+Convert.ToInt32("89",16).ToString())));

                //datas.Add(new ArticleData(1,"11",ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_RECOVER_LIFE+","+,));
                //datas.Add(new ArticleData(2,"22"));
                //datas.Add(new ArticleData(3,"33"));
                //datas.Add(new ArticleData(4,"44"));
                //datas.Add(new ArticleData(5,"55"));

                for (int i = 0; i < 10; i++)
                {
                    Entity entity = entityManager.CreateEntity(ArticleArchetype);
                    entityManager.SetComponentData(entity, new ArticleItem
                    {
                        GuiId = i,
                        Count = 10,
                        MaxCount = 99,
                        ObjectType = ENUM_OBJECT_TYPE.OBJECT_MAIL,
                        ObjectState = ENUM_OBJECT_STATE.OBJECT_INVALID_STATE,
                        BiologicalId = 1,
                        Type = ENUM_ITEM_CLASS.ITEM_CLASS_SKILL_BOOK,
                        Attribute1 = ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_ATTACKSPEED,
                        AttributeValue1 = 10,
                        Attribute2 = ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_ATTACKSPEED,
                        AttributeValue2 = 10,
                        Attribute3 = ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_ATTACKSPEED,
                        AttributeValue3 = 10,
                        Attribute4 = ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_ATTACKSPEED,
                        AttributeValue4 = 10,
                        Attribute5 = ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_ATTACKSPEED,
                        AttributeValue5 = 10,
                        Attribute6 = ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_ATTACKSPEED,
                        AttributeValue6 = 10,
                        Attribute7 = ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_ATTACKSPEED,
                        AttributeValue7 = 10,
                        Attribute8 = ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_ATTACKSPEED,
                        AttributeValue8 = 10
                    });
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

            #region InitOver

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

                UICenterMasterManager.Instance.ShowWindow(WindowID.MessageWindow);
                UICenterMasterManager.Instance.ShowWindow(WindowID.MenuWindow);
                FixedTitleWindow = (FixedTitleWindow)UICenterMasterManager.Instance.ShowWindow(WindowID.FixedTitleWindow);
                UICenterMasterManager.Instance.ShowWindow(WindowID.StrategyWindow);

                UICenterMasterManager.Instance.DestroyWindow(WindowID.LoadingWindow);

            }

            #endregion
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



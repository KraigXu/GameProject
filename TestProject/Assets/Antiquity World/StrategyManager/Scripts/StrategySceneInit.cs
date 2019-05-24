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
        public static EntityArchetype FactionArchetype;

        public static DemoSetting Settings;
        public static Entity PlayerEntity;

        public static Dictionary<Entity, GameObject> EcsGameObjectsDic = new Dictionary<Entity, GameObject>();

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
            FactionArchetype = entityManager.CreateArchetype(typeof(Faction));

            UICenterMasterManager.Instance.ShowWindow(WindowID.LoadingWindow);

            RelationSystem.SetupComponentData(entityManager);
            SocialDialogSystem.SetupComponentData(entityManager);
            PrestigeSystem.SetupComponentData(entityManager);


            #region Article
            {
                List<KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>> valuePairs = new List<KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>>();
                //valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_ATTACKSPEED, "10"));
                //valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_CRIT, "0"));
                //valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_DODGE, "0"));
                //valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_JEWEL_2, "30"));
                //valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_JEWEL_3, "0"));

                valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_MIN_ATTACK, "30"));
                valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_MAX_ATTACK, "35"));
                valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_CRIT, "20"));
                valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_HIT, "90"));
                valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_SKILL_ATTACK, "1"));

                string json = JsonConvert.SerializeObject(valuePairs);
               // Debug.Log(JsonConvert.SerializeObject(valuePairs));

                valuePairs = JsonConvert.DeserializeObject<List<KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>>>(json);


                Dictionary<int,string> valjson=new Dictionary<int, string>();
                valjson.Add(1,"100");
                valjson.Add(2,"200");
                valjson.Add(3,"300");
                Debug.Log(JsonConvert.SerializeObject(valjson));

                string valus = "AAAAA{0},!!!!";

                Debug.Log(string.Format(valus,10.333f));

                //  Debug.Log(JsonConvert.SerializeObject(valuePairs));

            }
            #endregion


            //#region DistrictInit
            //{
            //    List<DistrictData> datas = SQLService.Instance.QueryAll<DistrictData>();

            //    for (int i = 0; i < datas.Count; i++)
            //    {
            //        GameObject go = GameObject.Instantiate(GameStaticData.ModelPrefab[datas[i].Model]);
            //        go.transform.position = new Vector3(datas[i].X, datas[i].Y, datas[i].Z);

            //        Entity entity = go.GetComponent<GameObjectEntity>().Entity;
            //        entityManager.AddComponentData(entity, new District
            //        {
            //            Id = datas[i].Id,
            //            GId = 324 + i,
            //            Type = datas[i].Type,
            //            ProsperityLevel = datas[i].ProsperityLevel,
            //            TrafficLevel = datas[i].TrafficLevel,
            //            GrowingModulus = datas[i].GrowingModulus,
            //            SecurityModulus = datas[i].SecurityModulus
            //        });

            //        entityManager.AddComponentData(entity, new PeriodTime
            //        {
            //            Value = 0,
            //            Type = PeriodType.Shichen,
            //        });
            //    }
            //}
            //#endregion
            #region LivingAreaInit
            {
                SystemManager.Get<LivingAreaSystem>().LivingAreaInit(entityManager);
            }
            #endregion

            #region Character 信息初始
            {
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

                List<BiologicalData> datas = SQLService.Instance.QueryAll<BiologicalData>();

                for (int i = 0; i < datas.Count; i++)
                {
                    Transform entityGo = WXPoolManager.Pools[Define.GeneratedPool].Spawn(GameStaticData.BiologicalPrefab[datas[i].ModelId].transform);
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

                    //entityManager.AddComponent(entity, ComponentType.Create<EquipmentCoat>());
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

                    entityManager.AddComponentData(entity, new Knapsack
                    {
                        UpperLimit = 1000000,
                        KnapscakCode = datas[i].Id
                    });

                    entityManager.AddComponent(entity, ComponentType.Create<Team>());
                    entityManager.SetComponentData(entity, new Team
                    {
                        TeamBossId = datas[i].TeamId
                    });

                    if (datas[i].Identity == 0)
                    {
                        entityManager.AddComponent(entity, ComponentType.Create<NpcInput>());
                    }
                    else if (datas[i].Identity == 1)
                    {
                        entityManager.AddComponent(entity, ComponentType.Create<PlayerInput>());
                        //  entityGo.name = "PlayerMain";
                        PlayerEntity = entity;

                        SystemManager.Get<PlayerControlSystem>().InitPlayerEvent(entityGo.gameObject);

                        StrategyCameraManager.Instance.SetTarget(entityGo.transform);

                    }

                    if (datas[i].FactionId != 0)
                    {
                        entityManager.AddComponentData(entity,new FactionProperty
                        {

                        });
                    }


                    entityManager.AddComponent(entity, ComponentType.Create<BehaviorData>());
                    entityManager.SetComponentData(entity, new BehaviorData
                    {
                        Target = Vector3.zero,
                    });


                    if (string.IsNullOrEmpty(datas[i].JifaJson) == false)
                    {
                        TechniquesSystem.SpawnTechnique(entity, datas[i].JifaJson);
                    }
                        
                    ArticleSystem.SpawnArticle(SQLService.Instance.SimpleQuery<ArticleData>(" Bid=?", datas[i].Id), entity);
                    
                    SystemManager.Get<BiologicalSystem>().InitComponent(entityGo.gameObject);

                    GameStaticData.BiologicalNameDic.Add(datas[i].Id, datas[i].Name);
                    GameStaticData.BiologicalSurnameDic.Add(datas[i].Id, datas[i].Surname);
                    GameStaticData.BiologicalDescription.Add(datas[i].Id, datas[i].Description);


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

                List<FactionData> factionDatas = SQLService.Instance.QueryAll<FactionData>();

                for (int i = 0; i < factionDatas.Count; i++)
                {
                    Entity faction = entityManager.CreateEntity(FactionArchetype);
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
                //List<BiologicalData> data = SQLService.Instance.QueryAll<BiologicalData>();
                //for (int i = 0; i < data.Count; i++)
                //{
                //    if (data[i].Id == Settings.PlayerId)
                //    {
                //        StrategyCameraManager.Instance.SetTarget(new Vector3(data[i].X, data[i].Y, data[i].Z), true);
                //    }
                //}
                //data = null;

                UICenterMasterManager.Instance.ShowWindow(WindowID.WorldTimeWindow);
                UICenterMasterManager.Instance.ShowWindow(WindowID.MenuWindow);
                UICenterMasterManager.Instance.ShowWindow(WindowID.PlayerInfoWindow);
                UICenterMasterManager.Instance.ShowWindow(WindowID.MessageWindow);
                UICenterMasterManager.Instance.ShowWindow(WindowID.MapWindow);

                UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaTitleWindow);


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



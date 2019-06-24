using System;
using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using GameSystem.Ui;
using Newtonsoft.Json;
using Unity.Mathematics;
using UnityStandardAssets.Characters.ThirdPerson;
using Object = UnityEngine.Object;


namespace GameSystem
{
    public enum LocationType
    {
        None = 0,
        Field = 10,
        City = 20,
    }
    public class BiologicalSystem : ComponentSystem
    {
        struct Data
        {
            public readonly int Length;
           // public EntityArray Entitys;
          //  public GameObjectArray GameObjects;
            public ComponentDataArray<Biological> Biological;
            
            public ComponentArray<HexUnit> HexUnit;
            public ComponentArray<Transform> Transforms;

            //   public ComponentDataArray<BodyProperty> Body;
            //   public ComponentDataArray<BehaviorData> Behavior;
        }

        [Inject]
        private Data _data;

        private EntityManager _entityManager;
        private TipsWindow _tipsWindow;

        public static EntityArchetype BiologicalArchetype;
        public class ComponentGroup
        {
            public AICharacterControl AiCharacter;
            public Animator Animator;
        }

        private Dictionary<Entity, ComponentGroup> ComponentDic = new Dictionary<Entity, ComponentGroup>();


        public BiologicalSystem()
        {
            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
            BiologicalArchetype = _entityManager.CreateArchetype(typeof(Element), typeof(Position), typeof(Rotation), typeof(Biological), typeof(BodyProperty), typeof(Equipment), typeof(EquipmentCoat));
        }

        /// <summary>
        /// 缓存组件
        /// </summary>
        public void InitComponent(GameObject go)
        {

        }


        public static void SpawnRandomBiological(Transform node)
        {
            EntityManager entityManager = World.Active.GetOrCreateManager<EntityManager>();
            Entity entity = node.gameObject.AddComponent<GameObjectEntity>().Entity;

            entityManager.AddComponent(entity, ComponentType.Create<BehaviorData>());
            entityManager.SetComponentData(entity, new BehaviorData
            {
                Target = Vector3.zero,
            });


            BiologicalData data = SQLService.Instance.QueryUnique<BiologicalData>(" Id=?", 1);
            //Entity entity = entityManager.CreateEntity(BiologicalArchetype);

            entityManager.SetComponentData(entity, new Position
            {
                Value = new float3(0, -6, 6)
            });

            entityManager.SetComponentData(entity, new Rotation
            {
                Value = Quaternion.identity
            });

            entityManager.SetComponentData(entity, new Biological()
            {
                BiologicalId = data.Id,
                Age = data.Age,
                Sex = data.Sex,
                CharmValue = 0,
                Mobility = 0,
                OperationalAbility = 0,
                LogicalThinking = 0,

                Disposition = (byte)data.Disposition,
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

                AvatarId = data.AvatarId,
                ModelId = data.ModelId,
                FamilyId = data.FamilyId,
                FactionId = data.FactionId,
                TitleId = data.TitleId,
                TechniquesId = 0,
                EquipmentId = 0,

                Jing = 100,
                Qi = 100,
                Shen = 100,
                Tizhi = data.Tizhi,
                Lidao = data.Lidao,
                Jingshen = data.Jingshen,
                Lingdong = data.Lingdong,
                Wuxing = data.Wuxing
            });

            entityManager.SetComponentData(entity, new BodyProperty
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

            entityManager.AddComponentData(entity, new Knapsack
            {
                UpperLimit = 1000000,
                KnapscakCode = data.Id
            });

            entityManager.AddComponentData(entity, new Team
            {
                TeamBossId = data.TeamId
            });



            if (data.Identity == 0)
            {
                entityManager.AddComponent(entity, ComponentType.Create<NpcInput>());
            }
            else if (data.Identity == 1)
            {
                entityManager.AddComponent(entity, ComponentType.Create<PlayerInput>());

                //  SystemManager.Get<PlayerControlSystem>().InitPlayerEvent(entityGo.gameObject);
            }

            if (data.FactionId != 0)
            {
                entityManager.AddComponentData(entity, new FactionProperty
                {

                });
            }

            entityManager.AddComponent(entity, ComponentType.Create<BehaviorData>());
            entityManager.SetComponentData(entity, new BehaviorData
            {
                Target = Vector3.zero,
            });

            if (string.IsNullOrEmpty(data.JifaJson) == false)
            {
                TechniquesSystem.SpawnTechnique(entity, data.JifaJson);
            }

            ArticleSystem.SpawnArticle(SQLService.Instance.SimpleQuery<ArticleData>(" Bid=?", data.Id), entity);

            //  SystemManager.Get<BiologicalSystem>().InitComponent(entityGo.gameObject);

            //   ComponentGroup group = new ComponentGroup();
            //    group.AiCharacter = entityGo.GetComponent<AICharacterControl>();
            //  group.Animator = entityGo.GetComponent<Animator>();
            //if (ComponentDic.ContainsKey(entity) == false)
            //{
            //    ComponentDic.Add(entity, group);
            //}

            GameStaticData.BiologicalNameDic.Add(data.Id, data.Name);
            GameStaticData.BiologicalSurnameDic.Add(data.Id, data.Surname);
            GameStaticData.BiologicalDescription.Add(data.Id, data.Description);


        }


        public static void AddBiological(EntityManager entityManager, int id)
        {


            BiologicalData data = SQLService.Instance.QueryUnique<BiologicalData>(" Id=?", id);
            Entity entity = entityManager.CreateEntity(BiologicalArchetype);

            entityManager.SetComponentData(entity, new Position
            {
                Value = new float3(0, -6, 6)
            });

            entityManager.SetComponentData(entity, new Rotation
            {
                Value = Quaternion.identity
            });

            entityManager.SetComponentData(entity, new Biological()
            {
                BiologicalId = data.Id,
                Age = data.Age,
                Sex = data.Sex,
                CharmValue = 0,
                Mobility = 0,
                OperationalAbility = 0,
                LogicalThinking = 0,

                Disposition = (byte)data.Disposition,
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

                AvatarId = data.AvatarId,
                ModelId = data.ModelId,
                FamilyId = data.FamilyId,
                FactionId = data.FactionId,
                TitleId = data.TitleId,
                TechniquesId = 0,
                EquipmentId = 0,

                Jing = 100,
                Qi = 100,
                Shen = 100,
                Tizhi = data.Tizhi,
                Lidao = data.Lidao,
                Jingshen = data.Jingshen,
                Lingdong = data.Lingdong,
                Wuxing = data.Wuxing
            });

            entityManager.SetComponentData(entity, new BodyProperty
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

            entityManager.AddComponentData(entity, new Knapsack
            {
                UpperLimit = 1000000,
                KnapscakCode = data.Id
            });

            entityManager.AddComponentData(entity, new Team
            {
                TeamBossId = data.TeamId
            });



            if (data.Identity == 0)
            {
                entityManager.AddComponent(entity, ComponentType.Create<NpcInput>());
            }
            else if (data.Identity == 1)
            {
                entityManager.AddComponent(entity, ComponentType.Create<PlayerInput>());

                //  SystemManager.Get<PlayerControlSystem>().InitPlayerEvent(entityGo.gameObject);
            }

            if (data.FactionId != 0)
            {
                entityManager.AddComponentData(entity, new FactionProperty
                {

                });
            }

            entityManager.AddComponent(entity, ComponentType.Create<BehaviorData>());
            entityManager.SetComponentData(entity, new BehaviorData
            {
                Target = Vector3.zero,
            });

            if (string.IsNullOrEmpty(data.JifaJson) == false)
            {
                TechniquesSystem.SpawnTechnique(entity, data.JifaJson);
            }

            ArticleSystem.SpawnArticle(SQLService.Instance.SimpleQuery<ArticleData>(" Bid=?", data.Id), entity);

            //  SystemManager.Get<BiologicalSystem>().InitComponent(entityGo.gameObject);

            //   ComponentGroup group = new ComponentGroup();
            //    group.AiCharacter = entityGo.GetComponent<AICharacterControl>();
            //  group.Animator = entityGo.GetComponent<Animator>();
            //if (ComponentDic.ContainsKey(entity) == false)
            //{
            //    ComponentDic.Add(entity, group);
            //}

            GameStaticData.BiologicalNameDic.Add(data.Id, data.Name);
            GameStaticData.BiologicalSurnameDic.Add(data.Id, data.Surname);
            GameStaticData.BiologicalDescription.Add(data.Id, data.Description);


        }



        public void SetupComponentData(EntityManager entityManager)
        {



            List<BiologicalData> datas = SQLService.Instance.QueryAll<BiologicalData>();

            for (int i = 0; i < datas.Count; i++)
            {
                //entityManager.CreateEntity(BiologicalArchetype);
                Transform entityGo = WXPoolManager.Pools[Define.GeneratedPool].Spawn(GameStaticData.BiologicalPrefab[datas[i].ModelId].transform);
                  entityGo.position = new Vector3(datas[i].X, datas[i].Y, datas[i].Z);

               

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
        }





        protected override void OnUpdate()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                //var biological = _data.Biological[i];
                //var body = _data.Body[i];
                //var entity = _data.Entitys[i];
                //var behavior = _data.Behavior[i];
                //biological.Sex = body.Fertility;
                //biological.CharmValue = (20 * (body.Appearance / 100)) + (10 * (body.Dress / 100)) + (30 * (body.Skin / 100));
                //biological.Mobility = (3 * (body.RightLeg / 100)) + (3 * (body.LeftLeg / 100));
                //biological.OperationalAbility = (3 * (body.RightHand / 100)) + (3 * (body.LeftHand / 100));
                //biological.LogicalThinking = (100 * (body.Thought / 100));

                //biological.Jing = Convert.ToInt16(biological.Tizhi + (biological.Wuxing * 0.3f) + (biological.Lidao * 0.5f));
                //biological.Qi = Convert.ToInt16(biological.Jingshen + (biological.Tizhi * 0.5f) + (biological.Wuxing * 0.5f));
                //biological.Shen = Convert.ToInt16(biological.Wuxing + biological.Lidao * 0.3);

                //_data.Biological[i] = biological;
                //_data.Body[i] = body;

                //if (behavior.Target != Vector3.zero)
                //{
                //    ComponentDic[entity].AiCharacter.SetTarget(behavior.Target);
                //}

            }
            Debug.Log("111>>>>");
            Debug.Log(_data.Length+">>>>");
        }


        /// <summary>
        /// 根据当前状态获取Biologicals
        /// </summary>
        /// <param name="type">本地状态</param>
        /// <param name="id">本地ID</param>
        /// <returns>Entity 集合，使用ECS获取所需数据</returns>
        public List<Entity> GetBiologicalOnLocation(LocationType type, int id)
        {
            List<Entity> entities = new List<Entity>();
            for (int i = 0; i < _data.Length; i++)
            {
                //if (_data.Status[i].LocationType == type && _data.Status[i].LocationId == id)
                //{
                //    entities.Add(_data.Entitys[i]);
                //}
            }
            return entities;
        }
        /// <summary>
        /// 获取所有生物名字
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllBiologicalName()
        {
            List<int> result = new List<int>();

            for (int i = 0; i < _data.Length; i++)
            {
                //result.Add(_data.Biological[i].BiologicalId);
            }
            return result;
        }

        /// <summary>
        /// 获取指定ID的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Biological GetBiologicalInfo(int id)
        {
            for (int i = 0; i < _data.Length; i++)
            {
                //if (id == _data.Biological[i].BiologicalId)
                //{
                //    return new Biological();
                //   / return _data.Biological[i];
                //}
            }
            return new Biological();
        }


        //public void EnitiyAppendComponent<T>(int id)
        //{
        //    for (int i = 0; i < _data.Length; i++)
        //    {
        //        if (id == _data.Biological[i].BiologicalId)
        //        {
        //            Entity entity = _data.Entitys[i];
        //            _entityManager.AddComponent(entity, ComponentType.Create<T>());
        //        }
        //    }
        //}

        public Entity GetBiologicalEntity(int id)
        {
            for (int i = 0; i < _data.Length; i++)
            {
                //if (id == _data.Biological[i].BiologicalId)
                //{
                //    return Entity.Null;
                //}
            }
            return new Entity();
        }
        //public void SetBiologicalStatus(int id, BiologicalStatus status)
        //{
        //    for (int i = 0; i < _data.Length; i++)
        //    {
        //        if (_data.Biological[i].BiologicalId == id)
        //        {
        //            _data.Status[i] = status;
        //            return;
        //        }
        //    }
        //}



        /// <summary>
        /// Biological状态改变
        /// </summary>
        /// <param name="info"></param>
        public void BiologicalStatusChange(EventInfo info)
        {



        }

        /// <summary>
        /// 阵营计算
        /// </summary>
        /// <param name="x">横轴属性，也是秩序值 越小标识越秩序</param>
        /// <param name="y">竖轴属性，也是善良值，越小越善良</param>
        /// <returns></returns>
        public static string BiologicalDisposition(byte x, byte y)
        {
            if (y >= 0 && y <= 85)
            {
                if (x >= 0 && x <= 85)
                {
                    return "守序善良";
                }

                if (x > 85 && x <= 170)
                {
                    return "中立善良";
                }

                if (x > 170 && x <= 255)
                {
                    return "混乱善良";
                }

            }

            if (y > 85 && y <= 170)
            {
                if (x >= 0 && x <= 85)
                {
                    return "守序中立";
                }

                if (x > 85 && x <= 170)
                {
                    return "绝对中立";
                }

                if (x > 170 && x <= 255)
                {
                    return "混乱中立";
                }
            }

            if (y > 170 && y <= 255)
            {
                if (x >= 0 && x <= 85)
                {
                    return "守序邪恶";
                }

                if (x > 85 && x <= 170)
                {
                    return "中立邪恶";
                }

                if (x > 170 && x <= 255)
                {
                    return "混乱邪恶";
                }

            }

            return "混乱邪恶";
        }

    }

}


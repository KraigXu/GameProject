using System;
using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using GameSystem.Ui;
using Newtonsoft.Json;
using Unity.Rendering;
using UnityStandardAssets.Characters.ThirdPerson;

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
            public EntityArray Entitys;
            public GameObjectArray GameObjects;
            public ComponentDataArray<Biological> Biological;
            public ComponentDataArray<BodyProperty> Body;
            public ComponentDataArray<BehaviorData> Behavior;
        }
        
        [Inject]
        private Data _data;

        private EntityManager _entityManager;
        private TipsWindow _tipsWindow;

        public class ComponentGroup
        {
            public AICharacterControl AiCharacter;
            public Animator Animator;
        }

        private Dictionary<Entity, ComponentGroup> ComponentDic=new Dictionary<Entity, ComponentGroup>();


        public BiologicalSystem()
        {
            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
        }

        /// <summary>
        /// 缓存组件
        /// </summary>
        public void InitComponent(GameObject go)
        {

        }
        public void SetupComponentData(EntityManager entityManager)
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

                    SystemManager.Get<PlayerControlSystem>().InitPlayerEvent(entityGo.gameObject);
                }

                if (datas[i].FactionId != 0)
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

                if (string.IsNullOrEmpty(datas[i].JifaJson) == false)
                {
                    TechniquesSystem.SpawnTechnique(entity, datas[i].JifaJson);
                }

                ArticleSystem.SpawnArticle(SQLService.Instance.SimpleQuery<ArticleData>(" Bid=?", datas[i].Id), entity);

                SystemManager.Get<BiologicalSystem>().InitComponent(entityGo.gameObject);

                ComponentGroup group = new ComponentGroup();
                group.AiCharacter = entityGo.GetComponent<AICharacterControl>();
                group.Animator = entityGo.GetComponent<Animator>();
                if (ComponentDic.ContainsKey(entity) == false)
                {
                    ComponentDic.Add(entity, group);
                }

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

        protected override void OnUpdate()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                var biological = _data.Biological[i];
                var body = _data.Body[i];
                var entity = _data.Entitys[i];
                var behavior = _data.Behavior[i];
                biological.Sex = body.Fertility;
                biological.CharmValue = (20 * (body.Appearance / 100)) + (10 * (body.Dress / 100)) + (30 * (body.Skin / 100));
                biological.Mobility = (3 * (body.RightLeg / 100)) + (3 * (body.LeftLeg / 100));
                biological.OperationalAbility = (3 * (body.RightHand / 100)) + (3 * (body.LeftHand / 100));
                biological.LogicalThinking = (100 * (body.Thought / 100));

                biological.Jing= Convert.ToInt16(biological.Tizhi + (biological.Wuxing * 0.3f) + (biological.Lidao * 0.5f));
                biological.Qi = Convert.ToInt16(biological.Jingshen + (biological.Tizhi * 0.5f) + (biological.Wuxing * 0.5f));
                biological.Shen = Convert.ToInt16(biological.Wuxing + biological.Lidao * 0.3);

                _data.Biological[i] = biological;
                _data.Body[i] = body;

                if (behavior.Target != Vector3.zero)
                {
                    ComponentDic[entity].AiCharacter.SetTarget(behavior.Target);
                }
                
            }
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
                result.Add(_data.Biological[i].BiologicalId);
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
                if (id == _data.Biological[i].BiologicalId)
                {
                    return _data.Biological[i];
                }
            }
            return new Biological();
        }


        public void EnitiyAppendComponent<T>(int id)
        {
            for (int i = 0; i < _data.Length; i++)
            {
                if (id == _data.Biological[i].BiologicalId)
                {
                    Entity entity = _data.Entitys[i];
                    _entityManager.AddComponent(entity, ComponentType.Create<T>());
                }
            }
        }

        public Entity GetBiologicalEntity(int id)
        {
            for (int i = 0; i < _data.Length; i++)
            {
                if (id == _data.Biological[i].BiologicalId)
                {
                    return _data.Entitys[i];
                }
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

    }

}


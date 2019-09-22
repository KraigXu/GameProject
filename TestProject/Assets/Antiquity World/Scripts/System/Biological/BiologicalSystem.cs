using System;
using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using GameSystem.Ui;
using Unity.Mathematics;


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
            public ComponentDataArray<Biological> Biological;
            public ComponentDataArray<BodyProperty> Body;
            public ComponentArray<HexUnit> HexUnit;
        }

        [Inject]
        private Data _data;
        private EntityManager _entityManager;
        private TipsWindow _tipsWindow;

      
        public class ComponentGroup
        {
            public Animator Animator;
        }

        public BiologicalSystem()
        {
            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
        }

        protected override void OnUpdate()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                var biological = _data.Biological[i];
                var body = _data.Body[i];

                var unit = _data.HexUnit[i];

                //biological.Sex = body.Fertility;
                //biological.CharmValue = (20 * (body.Appearance / 100)) + (10 * (body.Dress / 100)) + (30 * (body.Skin / 100));
                //biological.Mobility = (3 * (body.RightLeg / 100)) + (3 * (body.LeftLeg / 100));
                //biological.OperationalAbility = (3 * (body.RightHand / 100)) + (3 * (body.LeftHand / 100));
                //biological.LogicalThinking = (100 * (body.Thought / 100));

                //biological.Jing = Convert.ToInt16(biological.Tizhi + (biological.Wuxing * 0.3f) + (biological.Lidao * 0.5f));
                //biological.Qi = Convert.ToInt16(biological.Jingshen + (biological.Tizhi * 0.5f) + (biological.Wuxing * 0.5f));
                //biological.Shen = Convert.ToInt16(biological.Wuxing + biological.Lidao * 0.3);

                //unit.Speed = biological.StrategyMoveBasSpeed + biological.StrategyMoveSpeed;
                //unit.VisionRange = biological.VisionRange + biological.VisionBaseRange;

                _data.Biological[i] = biological;
                _data.Body[i] = body;

            }
        }

        public static void SpawnRandomBiological(Transform node)
        {
            Entity entity = node.gameObject.GetComponent<GameObjectEntity>().Entity;
            BiologicalData data = SQLService.Instance.QueryUnique<BiologicalData>(" Id=?", 1);
            SystemManager.Get<BiologicalSystem>().AddBiological(data, entity);
        }

        public void AddBiological(PlayerData data, Entity entity)
        {
            _entityManager.AddComponentData(entity, new Biological()
            {
                BiologicalId = data.Id,
                Age = data.Age,
                Sex = data.Sex,
                CharmValue = 0,
                Mobility = 0,
                OperationalAbility = 0,
                LogicalThinking = 0,

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
                Wuxing = data.Wuxing,

                StrategyMoveSpeed = 6,
                FireMoveSpeed = 10,

                VisionRange = 3,

            });

            _entityManager.AddComponentData(entity, new BodyProperty
            {
                Tizhi = data.Tizhi,
                Lidao = data.Lidao,
                Jingshen = data.Jingshen,
                Lingdong = data.Lingdong,
                Wuxing = data.Wuxing
            });

            _entityManager.AddComponent(entity, ComponentType.Create<BehaviorData>());
            _entityManager.SetComponentData(entity, new BehaviorData
            {
                Target = Vector3.zero,

                BehaviourType = 1,
                CreantePositionCode = 1,
                NextPoint = Vector3.back,
                SelfPoint = Vector3.back,
                TargetEntity = Entity.Null,
                TargetId = 1,

                TimeToLive = 1,
            });

            _entityManager.AddComponentData(entity, new ExternalProperty());

            if (GameStaticData.BiologicalDictionary.ContainsKey(entity) == false)
            {
                string sex = data.Sex == 0 ? "男" : "女";

                BiologicalFixed biologicalFixed = new BiologicalFixed
                {
                    Description = data.Description,
                    Surname = data.Surname,
                    Name = data.Name,
                    Sex = sex,

                };
                GameStaticData.BiologicalDictionary.Add(entity, biologicalFixed);
            }

        }


        public void AddBiological(BiologicalData data, Entity entity)
        {
            _entityManager.AddComponentData(entity, new Biological()
            {
                BiologicalId = data.Id,
                Age = data.Age,
                Sex = data.Sex,
                CharmValue = 0,
                Mobility = 0,
                OperationalAbility = 0,
                LogicalThinking = 0,

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
                Wuxing = data.Wuxing,

                StrategyMoveSpeed = 6,
                FireMoveSpeed = 10,

                VisionRange = 3,

            });

            _entityManager.AddComponentData(entity, new BodyProperty
            {
                Tizhi = data.Tizhi,
                Lidao = data.Lidao,
                Jingshen = data.Jingshen,
                Lingdong = data.Lingdong,
                Wuxing = data.Wuxing
            });

            _entityManager.AddComponent(entity, ComponentType.Create<BehaviorData>());
            _entityManager.SetComponentData(entity, new BehaviorData
            {
                Target = Vector3.zero,
                
                BehaviourType = 1,
                CreantePositionCode = 1,
                NextPoint = Vector3.back,
                SelfPoint = Vector3.back,
                TargetEntity = Entity.Null,
                TargetId = 1,
              
                TimeToLive = 1,
            });

            _entityManager.AddComponentData(entity,new ExternalProperty());

            
        }

        /// <summary>
        /// 获取指定平面坐标上的所有单位
        /// </summary>
        /// <param name="units"></param>
        /// <param name="x"></param>
        /// <param name="z"></param>
        public void GetPoint(ref List<HexUnit> units, int x, int z)
        {

            for (int i = 0; i < _data.Length; i++)
            {
                HexCoordinates coordinates = _data.HexUnit[i].Location.coordinates;
                if (coordinates.X == x && coordinates.Z == z)
                    units.Add(_data.HexUnit[i]);
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



        public Entity GetBiologicalEntity(int id)
        {
            for (int i = 0; i < _data.Length; i++)
            {
                if (id == _data.Biological[i].BiologicalId)
                    return _data.Entitys[i];
            }
            return new Entity();
        }

        public HexUnit GetUnit(int id)
        {
            for (int i = 0; i < _data.Length; i++)
            {
                if (id == _data.Biological[i].BiologicalId)
                    return _data.HexUnit[i];
            }
            return new HexUnit();
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


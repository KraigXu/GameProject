using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using GameSystem.Ui;
using Invector;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    /// <summary>
    /// 城市
    /// </summary>
    public class CitySystem : ComponentSystem, LivingAreaFunction
    {
        struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
            public GameObjectArray GameObjects;
            public ComponentDataArray<LivingArea> LivingArea;
            public ComponentDataArray<Crowd> Crowd;
            public ComponentDataArray<CellMap> Map;
        }
        [Inject]
        private Data _data;

        private EntityManager _entityManager;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
        }


        protected override void OnUpdate()
        {
        }



        /// <summary>
        /// 进入城市
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="livingAreaEntity"></param>
        public static void EnterCity(Entity entity, Entity livingAreaEntity)
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            entityManager.AddComponentData(entity, new LivingAreaAssociated
            {
                LivingAreaEntity = livingAreaEntity,
                BuildingEntity = Entity.Null,
            });
        }




        public static void CityColliderEnter(GameObject go, Collider other)
        {
            Entity thisEntity = go.GetComponent<GameObjectEntity>().Entity;
            Entity targetEntity = other.gameObject.GetComponent<GameObjectEntity>().Entity;

            if (SystemManager.Contains<PlayerInput>(targetEntity) == true)
            {
                BehaviorData behaviorData = SystemManager.GetProperty<BehaviorData>(targetEntity);

                if (thisEntity == behaviorData.TargetEntity)
                {

                    other.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Hide"));

                    EnterCity(targetEntity, thisEntity);

                    ShowCityInside(thisEntity);

                    ShowWindowData showWindowData = new ShowWindowData();

                    LivingAreaWindowCD livingAreaWindowCd = new LivingAreaWindowCD();
                    livingAreaWindowCd.LivingAreaEntity = thisEntity;
                    showWindowData.contextData = livingAreaWindowCd;
                    UICenterMasterManager.Instance.ShowWindow(WindowID.CityWindow, showWindowData);
                    //UICenterMasterManager.Instance.ShowWindow(WindowID.BuildingWindow);
                }
            }
            else
            {
                BehaviorData behaviorData = SystemManager.GetProperty<BehaviorData>(targetEntity);

                if (thisEntity == behaviorData.TargetEntity)
                {
                    EnterCity(targetEntity, thisEntity);
                }

            }
        }

        public static void CityColliderExit(GameObject go, Collider other)
        {

        }


        /// <summary>
        /// 打开城市内景
        /// </summary>
        /// <param name="cityEntity"></param>
        public static void ShowCityInside(Entity cityEntity)
        {
            ShowWindowData windowData = new ShowWindowData();
            LivingAreaWindowCD windowCd = new LivingAreaWindowCD();

            UICenterMasterManager.Instance.ShowWindow(WindowID.LoadingWindow, windowData);


            UICenterMasterManager.Instance.DestroyWindow(WindowID.LoadingWindow);
        }


        /// <summary>
        /// 解析数据 往实体上增加建筑信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="values"></param>
        public void AnalysisDataSet(Entity entity, string[] values)
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();


            for (int i = 0; i < values.Length; i++)
            {

            }

            entityManager.AddComponentData(entity, new BuildingBazaar
            {

            });

            entityManager.AddComponentData(entity, new BuildingBlacksmith
            {

            });

            entityManager.AddComponentData(entity, new BuidingTavern
            {


            });
        }


        public void AddCity(HexCoordinates coordinates)
        {
            LivingAreaData data = SQLService.Instance.QueryUnique<LivingAreaData>(" PositionX=? and PositionZ=? ", coordinates.X, coordinates.Z);
            if (data == null)
            {
                return;
            }

            AddCity(data,coordinates);
        }


        public void AddCity(LivingAreaData data, HexCoordinates coordinates)
        {
            for (int i = 0; i < _data.Length; i++)
            {
                if (_data.Map[i].Coordinates.X == coordinates.X && _data.Map[i].Coordinates.Z== coordinates.Z)
                {
                    return;
                }
            }


            Entity entity = _entityManager.CreateEntity();
            _entityManager.AddComponentData(entity, new CellMap()
            {
                Coordinates = coordinates
            });

            _entityManager.AddComponentData(entity, new LivingArea
            {
                Id = data.Id,
                PersonNumber = data.PersonNumber,
                Type = (LivingAreaType)data.LivingAreaType,
                Money = data.Money,
                MoneyMax = data.MoneyMax,
                Iron = data.Iron,
                IronMax = data.IronMax,
                Wood = data.Wood,
                WoodMax = data.WoodMax,
                Food = data.Food,
                FoodMax = data.FoodMax,
                DefenseStrength = data.DefenseStrength,
                StableValue = data.StableValue
            });

            _entityManager.AddComponentData(entity, new District
            {
            });

            _entityManager.AddComponentData(entity, new Money
            {
                Value = data.Money,
                Upperlimit = data.MoneyMax
            });

            _entityManager.AddComponentData(entity, new Crowd
            {
                Number = 300000
            });

            LivingAreaSystem.LivingAreaAddBuilding(entity, data.BuildingInfoJson);

            if (GameStaticData.CityName.ContainsKey(data.Id) == false)
            {
                GameStaticData.CityName.Add(data.Id, data.Name);
                GameStaticData.CityDescription.Add(data.Id, data.Description);
            }

        }
    }

}
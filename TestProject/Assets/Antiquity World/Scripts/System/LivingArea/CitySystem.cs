using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using GameSystem.Ui;
using Invector;
using Newtonsoft.Json;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{

    public class CityRunData
    {
        public string Name;
        public string Description;
        public Sprite Sprite;

    }

    /// <summary>
    /// 城市， 固定点生成信息
    /// </summary>
    public class CitySystem : ComponentSystem, LivingAreaFunction
    {

        public CityTitleWindow CityTitleWindow;


        struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<HexCell> HexCells;
            public ComponentDataArray<City> City;
            
        }
        [Inject]
        private Data _data;

        private EntityManager _entityManager;
        private int _initId = 0;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
        }




        protected override void OnUpdate()
        {
            if (CityTitleWindow == null)
            {
                if (UICenterMasterManager.Instance.GetGameWindow(WindowID.CityTitleWindow) == null)
                {
                    CityTitleWindow = UICenterMasterManager.Instance.ShowWindow(WindowID.CityTitleWindow).GetComponent<CityTitleWindow>();
                }
                else
                {
                    CityTitleWindow = (CityTitleWindow)UICenterMasterManager.Instance.GetGameWindow(WindowID.CityTitleWindow);
                }
            }

            for (int i = 0; i < _data.Length; i++)
            {
                CityTitleWindow.Change(_data.City[i],_data.HexCells[i]);
            }

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

        public void AddCity(LivingAreaData data, HexCell cell)
        {
            _entityManager.AddComponentData(cell.Entity, new City
            {
                ModelId = data.ModelBaseId,
                UniqueCode = data.Id,
                CityLevel = data.LivingAreaLevel,
                Type = data.LivingAreaType,
            });

            _entityManager.AddComponentData(cell.Entity, new LivingArea
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

            if (GameStaticData.CityRunDataDic.ContainsKey(data.Id) == false)
            {
                CityRunData runData = new CityRunData();
                runData.Name = data.Name;
                runData.Description = data.Description;
                runData.Sprite = Resources.Load<Sprite>("Atlas/1 (6)");
                GameStaticData.CityRunDataDic.Add(data.Id, runData);
            }



            //Entity entity = _entityManager.CreateEntity();

            //_entityManager.AddComponentData(entity, new CellMap()
            //{
            //    Coordinates = coordinates
            //});

            //_entityManager.AddComponentData(entity,new City
            //{
            //    ModelId = 1,
            //    UniqueCode = _initId++,
            //});

            //LivingAreaSystem.LivingAreaAddBuilding(entity, data.BuildingInfoJson);

            //if (GameStaticData.CityRunDataDic.ContainsKey(data.Id) == false)
            //{
            //    CityRunData runData=new CityRunData();
            //    runData.Name = data.Name;
            //    runData.Description = data.Description;
            //    runData.Sprite = Resources.Load<Sprite>("Atlas/1 (6)");
            //    GameStaticData.CityRunDataDic.Add(data.Id,runData);
            //}

        }
    }

}
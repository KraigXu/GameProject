using System.Collections;
using System.Collections.Generic;

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
    public class CitySystem : ComponentSystem
    {
        public CityTitleWindow CityTitleWindow;

        private CityWindow _cityWin;

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
                CityTitleWindow.Change(_data.City[i], _data.HexCells[i]);
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

                    //  ShowCityInside(thisEntity);
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


        /// <summary>
        /// 打开城市内景
        /// </summary>
        /// <param name="cityEntity"></param>
        public static void ShowCityInside(Entity cityEntity, Entity targetEntity)
        {
            ShowWindowData windowData = new ShowWindowData();
            LivingAreaWindowCD windowCd = new LivingAreaWindowCD();

            //UICenterMasterManager.Instance.ShowWindow(WindowID.LoadingWindow, windowData);
            //UICenterMasterManager.Instance.DestroyWindow(WindowID.LoadingWindow);
        }

        public static void ShowCityWindow(Entity cityEntity, Entity targetEntity)
        {
            EntityManager entityManager = World.Active.GetOrCreateManager<EntityManager>();

            entityManager.GetComponentData<City>(cityEntity);

            ShowWindowData cityWindowData = new ShowWindowData();

            LivingAreaWindowCD livingAreaWindowCd = new LivingAreaWindowCD();
            livingAreaWindowCd.LivingAreaEntity = cityEntity;
            cityWindowData.contextData = livingAreaWindowCd;

            CityWindow cityWindow = (CityWindow)UICenterMasterManager.Instance.ShowWindow(WindowID.CityWindow);
            StrategyScene.Instance.ChangeModel(StrategySceneModel.LivingArea);

        }
        /// <summary>
        /// 生成城市
        /// </summary>
        /// <param name="entityManager"></param>
        /// <param name="data"></param>
        /// <param name="cell"></param>
        public static void AddCity(EntityManager entityManager, LivingAreaData data, HexCell cell)
        {
            entityManager.AddComponentData(cell.Entity, new City
            {
                ModelId = data.ModelBaseId,
                UniqueCode = data.Id,
                CityLevel = data.LivingAreaLevel,
                Type = data.LivingAreaType
            });

            entityManager.AddComponentData(cell.Entity, new LivingArea
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



            if (GameStaticData.CityRunDataDic.ContainsKey(cell.Entity) == false)
            {
                CityRunData runData = new CityRunData();
                runData.Name = data.Name;
                runData.Description = data.Description;
                runData.Sprite = Resources.Load<Sprite>("Atlas/1 (6)");
                GameStaticData.CityRunDataDic.Add(cell.Entity, runData);
            }


            // _entityManager.AddComponentData(cell.Entity,new Vector3());

            //根据City类型追加不同类型的建筑物
            switch (data.LivingAreaType)
            {
                case 0:   //小镇
                    {
                        entityManager.AddComponentData(cell.Entity, new BuidingTavern());
                    }
                    break;
                case 1:  //小城
                    {
                        entityManager.AddComponentData(cell.Entity, new BuildingBazaar());
                        entityManager.AddComponentData(cell.Entity, new BuildingBlacksmith());
                    }
                    break;
                case 2: //皇城
                    {
                        entityManager.AddComponentData(cell.Entity, new BuidingTavern());
                        entityManager.AddComponentData(cell.Entity, new BuildingBazaar());
                        entityManager.AddComponentData(cell.Entity, new BuildingBlacksmith());
                    }
                    break;
                case 3:
                    {
                        // _entityManager.AddComponentData(cell.Entity,new Dwellings());
                    }
                    break;
            }

            //BuildingFunctions.Add("Bazaar", SystemManager.Get<BuildingBazaarSystem>());
            //BuildingFunctions.Add("Blacksmith", SystemManager.Get<BuildingBlacksmithSystem>());
            //BuildingFunctions.Add("Dressmak", SystemManager.Get<BuildingDressmakSystem>());
            //BuildingFunctions.Add("Dwellings", SystemManager.Get<BuildingDwellingsSystem>());
            //BuildingFunctions.Add("Hospital", SystemManager.Get<BuildingHospitalSystem>());
            //BuildingFunctions.Add("Official", SystemManager.Get<BuildingOfficialSystem>());
            //BuildingFunctions.Add("Tavern", SystemManager.Get<BuildingTavernSystem>());


        }

        public void CityMass(Entity cityEntity, Entity massEntity)
        {
            if (cityEntity != Entity.Null)
            {
                LivingArea livingArea = _entityManager.GetComponentData<LivingArea>(cityEntity);

                if (SystemManager.Contains<CityMass>(massEntity) == false)
                {
                    _entityManager.AddComponentData(massEntity,new CityMass
                    {

                    });

                }

            }


        }

    }

}
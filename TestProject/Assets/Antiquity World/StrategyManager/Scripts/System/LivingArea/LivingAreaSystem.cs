using System;
using System.Collections.Generic;
using System.ComponentModel;
using DataAccessObject;
using Unity.Entities;
using UnityEngine;
using GameSystem.Ui;
using Newtonsoft.Json;
using Unity.Mathematics;
using Unity.Transforms;

namespace GameSystem
{
    /// <summary>
    /// 城市管理
    /// </summary>
    public class LivingAreaSystem : ComponentSystem
    {
        struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
            public GameObjectArray GameObjects;
            public ComponentDataArray<LivingArea> LivingArea;
        }
        [Inject]
        private Data _data;

        /// <summary>
        /// Key为LivingAreaEntity, Value=Building集的Entity
        /// 存储城市与建筑关联信息
        /// </summary>
        private Dictionary<Entity, List<Entity>> _livingAreaBuildMap = new Dictionary<Entity, List<Entity>>();
        public static List<BuildingFunction> BuildingSystems = new List<BuildingFunction>();
        public static Dictionary<string, BuildingFunction> BuildingFunctions = new Dictionary<string, BuildingFunction>();
        public static Dictionary<string, LivingAreaFunction> LivingAreaFunctions = new Dictionary<string, LivingAreaFunction>();

        public Dictionary<Entity, List<Entity>> LivingAreaBuildMap
        {
            get { return _livingAreaBuildMap; }
        }

        public EntityArray CurEntityArray
        {
            get { return _data.Entity; }
        }

        public GameObjectArray CuGameObjectArray
        {
            get { return _data.GameObjects; }
        }



        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            LivingAreaFunctions.Add("City", SystemManager.Get<CitySystem>());
            LivingAreaFunctions.Add("Organization", SystemManager.Get<OrganizationSystem>());

            BuildingSystems.Add(SystemManager.Get<BuildingBazaarSystem>());
            BuildingSystems.Add(SystemManager.Get<BuildingBlacksmithSystem>());
            BuildingSystems.Add(SystemManager.Get<BuildingDressmakSystem>());
            BuildingSystems.Add(SystemManager.Get<BuildingDwellingsSystem>());
            BuildingSystems.Add(SystemManager.Get<BuildingHospitalSystem>());
            BuildingSystems.Add(SystemManager.Get<BuildingOfficialSystem>());
            BuildingSystems.Add(SystemManager.Get<BuildingTavernSystem>());

            BuildingFunctions.Add("Bazaar", SystemManager.Get<BuildingBazaarSystem>());
            BuildingFunctions.Add("Blacksmith", SystemManager.Get<BuildingBlacksmithSystem>());
            BuildingFunctions.Add("Dressmak", SystemManager.Get<BuildingDressmakSystem>());
            BuildingFunctions.Add("Dwellings", SystemManager.Get<BuildingDwellingsSystem>());
            BuildingFunctions.Add("Hospital", SystemManager.Get<BuildingHospitalSystem>());
            BuildingFunctions.Add("Official", SystemManager.Get<BuildingOfficialSystem>());
            BuildingFunctions.Add("Tavern", SystemManager.Get<BuildingTavernSystem>());

        }

        /// <summary>
        /// 初始化城市
        /// </summary>
        /// <param name="entityManager"></param>
        public void LivingAreaInit(EntityManager entityManager)
        {

            List<LivingAreaModelData> livingAreaModelDatas = SQLService.Instance.QueryAll<LivingAreaModelData>();


            for (int i = 0; i < livingAreaModelDatas.Count; i++)
            {
               GameStaticData.LivingAreaPrefabDic.Add(livingAreaModelDatas[i].Id,Resources.Load<GameObject>(livingAreaModelDatas[i].Path));
            }



            List<LivingAreaData> datas = SQLService.Instance.QueryAll<LivingAreaData>();
            for (int i = 0; i < datas.Count; i++)
            {
                Transform entityGo = WXPoolManager.Pools[Define.GeneratedPool].Spawn(GameStaticData.LivingAreaPrefabDic[datas[i].ModelBaseId].transform);
                entityGo.position = new float3(datas[i].PositionX, datas[i].PositionY, datas[i].PositionZ);
                //entityGo.gameObject.name = datas[i].Name;
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

                entityManager.AddComponentData(entity, new District
                {
                    DistrictCode = i
                });

                entityManager.AddComponentData(entity, new Money
                {
                    Value = datas[i].Money,
                    Upperlimit = datas[i].MoneyMax
                });

                if (datas[i].LivingAreaType == 1)
                {
                    entityManager.AddComponentData(entity, new Crowd
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

                LivingAreaAddBuilding(entity, datas[i].BuildingInfoJson);

                GameStaticData.LivingAreaName.Add(datas[i].Id, datas[i].Name);
                GameStaticData.LivingAreaDescription.Add(datas[i].Id, datas[i].Description);
                

            }

        }

        public class BuildingJsonData
        {
            public string Code;
            public string[] Values;
        }
        /// <summary>
        /// 解析data,将符合条件的Building添加到指定的实体上
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="data">Building数据</param>
        /// <returns></returns>
        public static void LivingAreaAddBuilding(Entity entity, string data)
        {

            //List<BuildingJsonData> jsondatas = new List<BuildingJsonData>();
            //jsondatas.Add(new BuildingJsonData
            //{
            //    Code = "Bazaar",
            //    Values = new []{"1","3","0","33","44","22","33"}
            //});

            //jsondatas.Add(new BuildingJsonData
            //{
            //    Code = "Blacksmith",
            //    Values = new[] { "1", "3", "23", "34", "22", "33" }
            //});
            //jsondatas.Add(new BuildingJsonData
            //{
            //    Code = "Dressmak",
            //    Values = new[] { "1", "3", "0", "23", "44", "22", "33" }
            //});


            List<BuildingJsonData> jsondatas = JsonConvert.DeserializeObject<List<BuildingJsonData>>(data);

            for (int i = 0; i < jsondatas.Count; i++)
            {
                if (BuildingFunctions.ContainsKey(jsondatas[i].Code))
                {
                    BuildingFunctions[jsondatas[i].Code].AnalysisDataSet(entity, jsondatas[i].Values);
                }
                else
                {
                    Debuger.Log("???Buillding数据错误，错误:" + jsondatas[i].Code);
                }
            }
        }

        /// <summary>
        /// 新增城市建筑物信息
        /// </summary>
        /// <param name="livingentity"></param>
        /// <param name="buildingentity"></param>
        public void LivingAreaAddBuilding(Entity livingentity, Entity buildingentity)
        {
            if (_livingAreaBuildMap.ContainsKey(livingentity))
            {
                _livingAreaBuildMap[livingentity].Add(buildingentity);
            }
            else
            {
                _livingAreaBuildMap.Add(livingentity, new List<Entity>
                {
                    buildingentity
                });
            }
        }


        /// <summary>
        /// 获取这个城市内的建筑物实体
        /// </summary>
        /// <param name="livingEntity"></param>
        /// <returns></returns>
        public List<Entity> GetBuilding(Entity livingEntity)
        {
            if (_livingAreaBuildMap.ContainsKey(livingEntity))
            {
                return _livingAreaBuildMap[livingEntity];
            }
            return null;
        }
        protected override void OnUpdate()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                var livingArea = _data.LivingArea[i];
                //var node = _data.GameObjects[i];

                _data.LivingArea[i] = livingArea;
                //StrategySceneInit.FixedTitleWindow.Change(livingArea, node.transform);
            }
        }

        #region  Info

        /// <summary>
        /// 获取所有名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetLivingAreaNames()
        {
            List<string> names = new List<string>();

            List<LivingAreaData> datas = SQLService.Instance.QueryAll<LivingAreaData>();

            for (int i = 0; i < datas.Count; i++)
            {
                names.Add(datas[i].Name);
            }
            return names;
        }

        #endregion


        /// <summary>
        /// 是否可以进入城市
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int IsEnterLivingArea(Entity entity, Entity livingAreaEntity)
        {
            return 0;
        }

        /// <summary>
        /// 进入城市
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="livingAreaEntity"></param>
        public static void EnterLivingArea(Entity entity, Entity livingAreaEntity)
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            entityManager.AddComponentData(entity, new LivingAreaAssociated
            {
                LivingAreaEntity = livingAreaEntity,
                BuildingEntity = Entity.Null,
            });
        }


        /// <summary>
        /// 打开建筑内景视图
        /// </summary>
        /// <param name="buildingEntity"></param>
        /// <param name="biological"></param>
        public static void ShowBuildingInside(Entity buildingEntity, Entity biologicalentity, Entity livingarEntity)
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            if (SystemManager.Contains<BuildingBlacksmith>(buildingEntity) == true)
            {
                ShowWindowData show = new ShowWindowData();
                show.contextData = new BuildingUiInfo()
                {
                };

                BuildingWindow window = (BuildingWindow)UICenterMasterManager.Instance.ShowWindow(WindowID.BuildingWindow, show);
            }
            else if (SystemManager.Contains<Building>(buildingEntity) == true)
            {

            }
            else
            {
                Debug.Log("?????");
            }
        }
    }

}



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
    /// 城市
    /// </summary>
    public class CitySystem : ComponentSystem, LivingAreaFunction
    {
        struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
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
            for (int i = 0; i < _data.Length; i++)
            {
                
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

        

        public void AddCity(HexCoordinates coordinates)
        {
            LivingAreaData data = SQLService.Instance.QueryUnique<LivingAreaData>(" PositionX=? and PositionZ=? ", coordinates.X, coordinates.Z);
            if (data == null)
            {
                return;
            }

            AddCity(data, coordinates);
        }

        public void AddCity(HexCell cell)
        {

            LivingAreaData data = SQLService.Instance.QueryUnique<LivingAreaData>(" PositionX=? and PositionZ=? ", cell.coordinates.X, cell.coordinates.Z);
            if(data==null)
                return;

            _entityManager.AddComponentData(cell.Entity, new City
            {
                ModelId = data.ModelBaseId,
                UniqueCode = data.Id,
                CityLevel=data.LivingAreaLevel,
                Type=data.LivingAreaType,
            });


            //List<BuildingJsonData> jsondatas = JsonConvert.DeserializeObject<List<BuildingJsonData>>(data);

            //for (int i = 0; i < jsondatas.Count; i++)
            //{
            //    if (BuildingFunctions.ContainsKey(jsondatas[i].Code))
            //    {
            //        BuildingFunctions[jsondatas[i].Code].AnalysisDataSet(entity, jsondatas[i].Values);
            //    }
            //    else
            //    {
            //        Debuger.Log("???Buillding数据错误，错误:" + jsondatas[i].Code);
            //    }
            //}
           // LivingAreaSystem.LivingAreaAddBuilding(cell.Entity, data.BuildingInfoJson);

            if (GameStaticData.CityRunDataDic.ContainsKey(data.Id) == false)
            {
                CityRunData runData = new CityRunData();
                runData.Name = data.Name;
                runData.Description = data.Description;
                runData.Sprite = Resources.Load<Sprite>("Atlas/1 (6)");
                GameStaticData.CityRunDataDic.Add(data.Id, runData);
            }
        }


        public void AddCity(LivingAreaData data, HexCoordinates coordinates)
        {
            Entity entity = _entityManager.CreateEntity();

            _entityManager.AddComponentData(entity, new CellMap()
            {
                Coordinates = coordinates
            });

            _entityManager.AddComponentData(entity,new City
            {
                ModelId = 1,
                UniqueCode = _initId++,
            });

            LivingAreaSystem.LivingAreaAddBuilding(entity, data.BuildingInfoJson);

            if (GameStaticData.CityRunDataDic.ContainsKey(data.Id) == false)
            {
                CityRunData runData=new CityRunData();
                runData.Name = data.Name;
                runData.Description = data.Description;
                runData.Sprite = Resources.Load<Sprite>("Atlas/1 (6)");
                GameStaticData.CityRunDataDic.Add(data.Id,runData);
            }

        }
    }

}
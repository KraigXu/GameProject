using System;
using System.Collections.Generic;
using System.ComponentModel;
using DataAccessObject;
using Unity.Entities;
using UnityEngine;
using GameSystem.Ui;
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

        public Dictionary<Entity, List<Entity>> LivingAreaBuildMap
        {
            get { return _livingAreaBuildMap; }
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

        public void InitSystem()
        {


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
                var node = _data.GameObjects[i];
               
                _data.LivingArea[i] = livingArea;
                StrategySceneInit.FixedTitleWindow.Change(livingArea, node.transform);
            }
        }

        #region  Info

        /// <summary>
        /// 获取所有名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetLivingAreaNames()
        {
            List<string> names=new List<string>();
 
           List<LivingAreaData> datas=  SQLService.Instance.QueryAll<LivingAreaData>();

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
            }else if (SystemManager.Contains<Building>(buildingEntity) == true)
            {

            }
            else
            {
                Debug.Log("?????");
            }
        }


    }

}



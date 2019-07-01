using System.Collections;
using System.Collections.Generic;
using GameSystem.Ui;
using Newtonsoft.Json;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{

    /// <summary>
    /// 市集系统
    /// </summary>
    public class BuildingBazaarSystem : ComponentSystem, BuildingFunction
    {
        struct Data
        {
            public readonly int Length;
            public EntityArray Entitys;
            public ComponentDataArray<BuildingBazaar> Bazaars;
        }
        private Data _data;


        protected override void OnUpdate()
        {
        }

        public void AnalysisDataSet(Entity entity,string[] values)
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            entityManager.AddComponentData(entity, new BuildingBazaar
            {
                LevelId = int.Parse(values[0]),
                OperateEnd = int.Parse(values[1]),
                OperateStart = int.Parse(values[2]),
                PositionCode =int.Parse(values[3]),
                ShopSeed = int.Parse(values[4])
            });
        }

        /// <summary>
        /// 获取这个功能的内部方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public UiBuildingItem GetBuildingItem(Entity entity)
        {
            if (SystemManager.Contains<BuildingBlacksmith>(entity) == true)
            {
                BuildingBlacksmith buildingBlacksmith = SystemManager.GetProperty<BuildingBlacksmith>(entity);
                UiBuildingItem uiBuildingItem = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyStyle.UiPersonButton).GetComponent<UiBuildingItem>();
                uiBuildingItem.Value= "市集";
                uiBuildingItem.OnBuildingEnter = OpenUi;
                return uiBuildingItem;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="value"></param>
        public void AddBuildingSystem(Entity entity, BuildingItem item)
        {
            BuildingBlacksmith blacksmith = new BuildingBlacksmith();
            blacksmith.LevelId = item.Level;
            blacksmith.ShopSeed = 10;
            blacksmith.OperateEnd = 10;
            blacksmith.OperateStart = 10;
            EntityManager.AddComponentData(entity, blacksmith);
        }

        /// <summary>
        /// 打开内部
        /// </summary>
        /// <param name="entity"></param>
        public void OpenUi(Entity entity)
        {
            if (SystemManager.Contains<BuildingBazaar>(entity) == true)
            {
                ShowWindowData showWindowData = new ShowWindowData();
                EntityContentData entityContentData=new EntityContentData();
                entityContentData.Entity = entity;
                showWindowData.contextData = entityContentData;
                UICenterMasterManager.Instance.ShowWindow(WindowID.BuildingBazaarWindow, showWindowData);

            }
            else
            {
                Debuger.LogError("在进入BuildingBlacksmite时发生错误！");
            }

        }

        public bool IsBuilding(Entity entity)
        {
            return true;
        }


    }
}

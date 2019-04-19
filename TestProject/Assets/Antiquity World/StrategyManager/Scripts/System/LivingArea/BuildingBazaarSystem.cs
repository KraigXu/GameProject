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
            public BuildingBazaar Bazaars;
        }
        private Data _data;

        protected override void OnStartRunning()
        {
            base.OnStartRunning();

            

        }


        protected override void OnUpdate()
        {

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
                UiBuildingItem uiBuildingItem = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyStyle.Instance.UiPersonButton).GetComponent<UiBuildingItem>();
                uiBuildingItem.Value= "TTTT";
                uiBuildingItem.OnBuildingEnter = OpenUi;
                return uiBuildingItem;
            }
            else
            {
                return null;
            }
        }

        

        //protected override void OnStartRunning()
        //{
        //    base.OnStartRunning();
        //    BuildingItem = Resources.Load<RectTransform>("UiPrefab/UiItem/LivingAreaBuilding");

        //    FeaturesesEntitys = new[]
        //    {
        //        new BuildingBlacksmithFeatures
        //        {
        //            Id = 1,
        //            Name = "ZZ",
        //            Type = "1",
        //            CallBack = BuildingBlackZZ
        //        },
        //        new BuildingBlacksmithFeatures
        //        {
        //            Id = 1,
        //            Name = "YL",
        //            Type = "2",
        //            CallBack = BuildingBlackYL
        //        }
        //    };

        //}


        //private RectTransform BuildingItem;



        //public BuildingBlacksmithFeatures[] FeaturesesEntitys;



        //private void BuildingBlackZZ(Entity entity, int id)
        //{

        //}
        //private void BuildingBlackYL(Entity entity, int id)
        //{

        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="value"></param>
        public  void AddBuildingSystem(Entity entity, BuildingItem item)
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
        public  void OpenUi(Entity entity)
        {
            if (SystemManager.Contains<BuildingBlacksmith>(entity) == true)
            {
                //初始化值
                ShowWindowData showWindowData = new ShowWindowData();

                BuildingUiInfo uiInfo = new BuildingUiInfo();
            //    uiInfo.FeaturesUiInfos = FeaturesesEntitys;

                UICenterMasterManager.Instance.ShowWindow(WindowID.BuildingBlacksmithWindow, showWindowData);

            }
            else
            {
                Debuger.LogError("在进入BuildingBlacksmite时发生错误！");
            }

        }

        public bool IsBuilding(Entity entity)
        {
            throw new System.NotImplementedException();
        }


    }
}

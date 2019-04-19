using System.Collections;
using System.Collections.Generic;
using GameSystem.Ui;
using Newtonsoft.Json;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{

    public class BuildingDwellingsSystem : ComponentSystem, BuildingFunction
    {
        struct Data
        {
            public readonly int Length;
            public EntityArray Entitys;
            public BuildingBlacksmith BuildingBlacksmith;

        }

        private Data _data;

        private RectTransform BuildingItem;

        public BuildingBlacksmithFeatures[] FeaturesesEntitys;




        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            BuildingItem = Resources.Load<RectTransform>("UiPrefab/UiItem/LivingAreaBuilding");

            FeaturesesEntitys = new[]
            {
                new BuildingBlacksmithFeatures
                {
                    Id = 1,
                    Name = "ZZ",
                    Type = "1",
                    CallBack = BuildingBlackZZ
                },
                new BuildingBlacksmithFeatures
                {
                    Id = 1,
                    Name = "YL",
                    Type = "2",
                    CallBack = BuildingBlackYL
                }
            };


        }

        private void BuildingBlackZZ(Entity entity, int id)
        {

        }
        private void BuildingBlackYL(Entity entity, int id)
        {

        }


        protected override void OnUpdate()
        {
        }




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
        /// 获取这个功能的内部方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public  UiBuildingItem GetBuildingItem(Entity entity)
        {
            if (SystemManager.Contains<BuildingBlacksmith>(entity) == true)
            {
                BuildingBlacksmith buildingBlacksmith = SystemManager.GetProperty<BuildingBlacksmith>(entity);
                UiBuildingItem uiBuildingItem = WXPoolManager.Pools[Define.GeneratedPool].Spawn(BuildingItem).GetComponent<UiBuildingItem>();
                uiBuildingItem.Value= "TTTT";
                uiBuildingItem.OnBuildingEnter = OpenUi;
                return uiBuildingItem;
            }
            else
            {
                return null;
            }
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
                uiInfo.FeaturesUiInfos = FeaturesesEntitys;

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

using System.Collections;
using System.Collections.Generic;
using GameSystem.Ui;
using Newtonsoft.Json;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class BuildingBlacksmithSystem : BuildingSystem
    {


        struct Data
        {
            public readonly int Length;
            public EntityArray Entitys;
            public BuildingBlacksmith BuildingBlacksmith;

        }

        private Data _data;

        private RectTransform BuildingItem;

        public class BuildingBlacksmithFeatures
        {
            public int Id;
            public string Name;
        }

        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            BuildingItem = Resources.Load<RectTransform>("UiPrefab/UiItem/LivingAreaBuilding");
        }

        protected override void OnUpdate()
        {
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="value"></param>
        public override void AddBuildingSystem(Entity entity, BuildingItem item)
        {
            BuildingBlacksmith blacksmith = new BuildingBlacksmith();
            blacksmith.LevelId = item.Level;
            blacksmith.ShopSeed = 10;
            blacksmith.OperateEnd = 10;
            blacksmith.OperateStart = 10;
            EntityManager.AddComponentData(entity, blacksmith);
        }

        public override UiBuildingItem GetBuildingItem(Entity entity)
        {
            if (SystemManager.Contains<BuildingBlacksmith>(entity) == true)
            {
                BuildingBlacksmith buildingBlacksmith = SystemManager.GetProperty<BuildingBlacksmith>(entity);
                UiBuildingItem uiBuildingItem = WXPoolManager.Pools[Define.GeneratedPool].Spawn(BuildingItem).GetComponent<UiBuildingItem>();
                uiBuildingItem.Name.text = "TTTT";
                uiBuildingItem.OnBuildingEnter = OpenUi;

                return uiBuildingItem;
            }
            else
            {
                return null;
            }
        }

        public override void OpenUi(Entity entity)
        {
            ShowWindowData showWindowData = new ShowWindowData();
            BuildingWindow buildingWindow = (BuildingWindow)UICenterMasterManager.Instance.ShowWindow(WindowID.BuildingWindow, showWindowData);
            
            buildingWindow.Show();
        }
    }
}

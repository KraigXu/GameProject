using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using GameSystem.Ui;
using Newtonsoft.Json;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{

    public class BuildingBlacksmithSystem : ComponentSystem, BuildingFunction
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



        struct BuildingGroup
        {
            public readonly int Length;
            public ComponentDataArray<Building> Building;
            public EntityArray Entity;
        }

        [Inject]
        private BuildingGroup _buildingGroup;

        public static List<BuildingData> BuildingDatas = new List<BuildingData>();

        public static void InitModel(EntityManager entityManager)
        {
            BuildingDatas = SQLService.Instance.QueryAll<BuildingData>();
            for (int j = 0; j < BuildingDatas.Count; j++)
            {
                GameStaticData.BuildingName.Add(BuildingDatas[j].Id, BuildingDatas[j].Name);
                GameStaticData.BuildingDescription.Add(BuildingDatas[j].Id, BuildingDatas[j].Description);
            }
            GameStaticData.BuildingType.Add(0, "BuildingType1");
            GameStaticData.BuildingType.Add(1, "BuildingType2");
            GameStaticData.BuildingType.Add(2, "BuildingType3");
            GameStaticData.BuildingType.Add(3, "BuildingType4");
            GameStaticData.BuildingType.Add(4, "BuildingType5");
            GameStaticData.BuildingType.Add(5, "BuildingType6");
            GameStaticData.BuildingType.Add(6, "BuildingType7");

            GameStaticData.BuildingStatus.Add(0, "空地");
            GameStaticData.BuildingStatus.Add(1, "正常");
            GameStaticData.BuildingStatus.Add(2, "建筑中");
        }


        /// <summary>
        /// 初始化entity
        /// </summary>
        /// <param name="entityManager"></param>
        /// <param name="jsonData"></param>
        public static void SetData(EntityManager entityManager, BuildingJsonData jsonData, int livingAreaId)
        {
            EntityArchetype buildingArchetype = entityManager.CreateArchetype(typeof(Building), typeof(PeriodTime));
            foreach (var item in jsonData.Item)
            {
                Entity entity = entityManager.CreateEntity(buildingArchetype);
                entityManager.SetComponentData(entity, new Building
                {
                    LivingAreaId = livingAreaId,
                    BuildingModelId = item.BuildingModelId,
                    DurableValue = item.DurableValue,
                    Id = jsonData.GroupId,
                    Level = item.BuildingLevel,

                    Status = item.Status,
                    Type = item.Type,
                });

                entityManager.SetComponentData(entity, new PeriodTime
                {
                    Type = PeriodType.Day
                });
            }
        }

        public List<BuildingiDataItem> GetUiData(int livingAreaId)
        {
            List<BuildingiDataItem> datas = new List<BuildingiDataItem>();

            for (int i = 0; i < _buildingGroup.Length; i++)
            {
                if (_buildingGroup.Building[i].ParentId == livingAreaId)
                {
                    var building = _buildingGroup.Building[i];
                    BuildingiDataItem item = new BuildingiDataItem();
                    item.Id = building.Id;
                    item.Level = building.Level;
                    item.OnlyEntity = _buildingGroup.Entity[i];
                    item.Status = _buildingGroup.Building[i].Status;
                    item.ImageId = _buildingGroup.Building[i].Type;
                    item.Point = _buildingGroup.Building[i].Position;

                    datas.Add(item);
                }
            }
            return datas;
        }

        public List<Entity> GetBuildingGroup(int livingAreaId)
        {
            List<Entity> entities = new List<Entity>();

            for (int i = 0; i < _buildingGroup.Length; i++)
            {
                if (_buildingGroup.Building[i].LivingAreaId == livingAreaId)
                {
                    entities.Add(_buildingGroup.Entity[i]);
                }
            }
            return entities;


        }


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
                uiBuildingItem.Value = "TTTT";
                uiBuildingItem.OnBuildingEnter = OpenUi;
                return uiBuildingItem;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 打开建筑内景视图
        /// </summary>
        /// <param name="buildingEntity"></param>
        /// <param name="biological"></param>
        public static void ShowBuildingInside(Entity buildingEntity, Entity biologicalentity, Entity livingarEntity)
        {
            BuildingWindow window = (BuildingWindow)UICenterMasterManager.Instance.ShowWindow(WindowID.BuildingWindow);

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

        public virtual BuildingBlacksmith InitData(string value)
        {
            return new BuildingBlacksmith();
        }

        public void AnalysisDataSet(Entity entity, string[] values)
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            entityManager.AddComponentData(entity,new BuildingBlacksmith
            {
                LevelId = 3,
                OperateStart = 3,
                OperateEnd = 3,
              //  Person = 3,
                PositionCode = 3,
                ShopSeed = 3,
            });
        }
    }
}

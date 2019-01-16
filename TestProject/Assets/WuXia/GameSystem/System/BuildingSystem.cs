using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using GameSystem.Ui;

namespace GameSystem
{
    public delegate void BuildingEvent(Entity entity, int id);

    public class BuildingJsonData
    {
        public int GroupId;
        public List<BuildingItem> Item = new List<BuildingItem>();
    }

    public class BuildingItem
    {
        public int BuildingModelId;
        public int BuildingLevel;
        public int Status;
        public int Type;
        public int DurableValue;

        public BuildingItem() { }
        public BuildingItem(int buildingModelId, int buildingLevel, int status, int type, int durableValue)
        {
            this.BuildingModelId = buildingModelId;
            this.BuildingLevel = buildingLevel;
            this.Status = status;
            this.Type = type;
            this.DurableValue = durableValue;
        }
    }

    //建筑物状态
    public enum BuildingStatus
    {
        None,                   //空地
        Normal,                 //正常
        UnderConstruction,     //建筑中
    }

    public class BuildingFeatures
    {
        public string Name;
        public string Description;
    }

    public class BuildingSystem : ComponentSystem
    {
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
        public static void SetData(EntityManager entityManager, BuildingJsonData jsonData,int livingAreaId)
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

        protected override void OnUpdate()
        {

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
            List<Entity> entities=new List<Entity>();

            for (int i = 0; i < _buildingGroup.Length; i++)
            {
                if (_buildingGroup.Building[i].LivingAreaId == livingAreaId)
                {
                    entities.Add(_buildingGroup.Entity[i]);
                }
            }
            return entities;


        }
    }

}


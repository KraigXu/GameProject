using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using GameSystem.Ui;

namespace GameSystem
{
    /// <summary>
    /// LivingArea：居住地类型 影响本身的逻辑
    /// </summary>
    public enum LivingAreaType
    {
        Camp,  //营地
        Faction,  //帮派
        City,     //城市
        Cave,    //洞窟
    }



    /// <summary>
    /// 建筑物
    /// </summary>
    public class BuildingObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int BuildingLevel { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public int DurableValue { get; set; }
        public int OwnId { get; set; }
        public int ImageId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public BuildingObject() { }
        // public string MarkIds { get; set; }
        // public string ModelPath { get; set; }
        // public string BuildingFeaturesIds { get; set; }

        //public BuildingObject(string key, string name, string description, int buildingLevel, int status, int type,
        //    int durableValue, int ownId, string buildingFeaturesIds, string markIds, string modelPath)
        //{
        //    this.Key = key;
        //    this.Name = name;
        //    this.Description = description;
        //    this.BuildingLevel = buildingLevel;
        //    this.Status = status;
        //    this.Type = type;
        //    this.DurableValue = durableValue;
        //    this.OwnId = ownId;
        //    this.BuildingFeaturesIds = buildingFeaturesIds;
        //    this.MarkIds = markIds;
        //    this.ModelPath = modelPath;
        //}
    }

    //建筑物状态
    public enum BuildingStatus
    {
        None,                   //空地
        Normal,                 //正常
        UnderConstruction,     //建筑中
    }
    /// <summary>
    /// 建筑物类型
    /// </summary>
    public enum BuildingType
    {
        Workout,                //锻炼
        Rest                   //休息

    }

    public class BuildingFeatures
    {
        public string Name;
        public string Description;
    }

    public class LivingAreaSystem : ComponentSystem
    {
        struct LivingAreaGroup
        {
            public readonly int Length;
            public ComponentDataArray<LivingArea> LivingAreaNode;
            public ComponentArray<Transform> LivingAreaPositon;
            
            public EntityArray Entity;
        }
        [Inject]
        private LivingAreaGroup _livingAreas;
        [Inject]
        private BuildingSystem _buildingSystem;
        protected override void OnUpdate()
        {
            for (int i = 0; i < _livingAreas.Length; i++)
            {
                var livingArea = _livingAreas.LivingAreaNode[i];

                if (livingArea.TitleUiId == 0)
                {
                    livingArea.TitleUiId= UICenterMasterManager.Instance
                        .GetGameWindowScript<FixedTitleWindow>(WindowID.FixedTitleWindow).AddTitle(
                            ElementType.LivingArea, livingArea.Id, _livingAreas.LivingAreaPositon[i].position);
                }


                _livingAreas.LivingAreaNode[i] = livingArea;
            }
        }
        /// <summary>
        /// 获取UI数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LivingAreaWindowCD GetLivingAreaData(int id)
        {
            LivingAreaWindowCD uidata = new LivingAreaWindowCD();
            for (int i = 0; i < _livingAreas.Length; i++)
            {
                if (_livingAreas.LivingAreaNode[i].Id != id)
                {
                    continue;
                }
                var livingArea = _livingAreas.LivingAreaNode[i];
                uidata.LivingAreaId = _livingAreas.LivingAreaNode[i].Id;
                uidata.PowerId = _livingAreas.LivingAreaNode[i].Id;
                uidata.ModelId = _livingAreas.LivingAreaNode[i].ModelId;
                uidata.PersonId = _livingAreas.LivingAreaNode[i].Id;
                uidata.PersonNumber = livingArea.PersonNumber;
                uidata.Money = livingArea.Money;
                uidata.MoneyMax = livingArea.MoneyMax;
                uidata.Iron = livingArea.Iron;
                uidata.IronMax = livingArea.IronMax;
                uidata.Wood = livingArea.Wood;
                uidata.WoodMax = livingArea.WoodMax;
                uidata.Food = livingArea.Food;
                uidata.FoodMax = livingArea.FoodMax;
                uidata.LivingAreaLevel = livingArea.CurLevel;
                uidata.LivingAreaMaxLevel = livingArea.MaxLevel;
                uidata.LivingAreaType = livingArea.TypeId;
                uidata.DefenseStrength = livingArea.DefenseStrength;
            }

            uidata.BuildingiDataItems=_buildingSystem.GetUiData(id);
            return uidata;

        }

        public LivingArea GetLivingAreaInfo(int id)
        {
            for (int i = 0; i < _livingAreas.Length; i++)
            {
                if (_livingAreas.LivingAreaNode[i].Id != id)
                {
                    return _livingAreas.LivingAreaNode[i];
                }
            }
           return  new LivingArea();
        }

        


        /// <summary>
        /// 检测这个ID是否存在数据
        /// </summary>
        /// <param name="livingAreaId"></param>
        /// <returns></returns>
        public bool IsTrue(int livingAreaId)
        {
            ComponentDataArray<LivingArea> livingAreas = _livingAreas.LivingAreaNode;

            for (int i = 0; i < livingAreas.Length; i++)
            {
                if (livingAreas[i].Id == livingAreaId)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取指定Transform的数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public LivingArea GetLivingArea(Transform node)
        {
            ComponentArray<Transform> livingAreas = _livingAreas.LivingAreaPositon;

            for (int i = 0; i < livingAreas.Length; i++)
            {
                if (livingAreas[i] == node)
                {
                    return _livingAreas.LivingAreaNode[i];
                }
            }
            return new LivingArea();
        }

        public List<int> GetLivingAreaIds()
        {
            List<int> ids=new List<int>();

            for (int i = 0; i < _livingAreas.Length; i++)
            {
                ids.Add(_livingAreas.LivingAreaNode[i].Id);
            }

            return ids;
        }

        




    }

}



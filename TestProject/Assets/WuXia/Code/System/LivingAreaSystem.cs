using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TinyFrameWork;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using DataAccessObject;
using Unity.Mathematics;

namespace WX
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

    /// <summary>
    /// 设施类型
    /// </summary>
    public enum BuildingFeatureType
    {

    }


    public class LivingAreaSystem : ComponentSystem
    {

        public bool CurShowUi = false;

        //UI
        private LivingAreaTitleWindow _livingAreaTitle;

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

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
        }

        protected override void OnUpdate()
        {

            for (int i = 0; i < _livingAreas.Length; i++)
            {
                var livingArea = _livingAreas.LivingAreaNode[i];
                if (livingArea.IsInternal == 1)
                {
                    UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow);

                }
            }

            if (CurShowUi == false)
            {
                WindowContextLivingAreaData uidata = new WindowContextLivingAreaData();
                for (int i = 0; i < _livingAreas.Length; i++)
                {
                    uidata.EntityArray.Add(_livingAreas.Entity[i]);
                    uidata.Points.Add(_livingAreas.LivingAreaPositon[i].position);
                }

                if (_livingAreaTitle)
                {
                    ShowWindowData data = new ShowWindowData();
                    data.contextData = uidata;
                    _livingAreaTitle.ShowWindow(data.contextData);
                }
                else
                {
                    ShowWindowData data = new ShowWindowData();
                    data.contextData = uidata;
                    _livingAreaTitle = (LivingAreaTitleWindow)UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaTitleWindow, data);
                }
                CurShowUi = true;
            }
        }

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
                uidata.OnlyEntity = _livingAreas.Entity[i];
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


    }

}



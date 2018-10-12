using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using LitJson;
using TinyFrameWork;
using UnityEngine;

namespace LivingArea
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
        public string Name { get; set; }                    //名称
        public string Description { get; set; }             //说明
        public int TypeId { get; set; }                         //类型ID
        public int BuildingLevel { get; set; }                  //等级
        public int DurableValue { get; set; }               //耐久      
        public int DurableMax { get; set; }               //最大耐久
        public int BuildingStatus { get; set; }           //建筑状态
        public int HaveId { get; set; }                       //所属Id
        public string BuildingFeaturesIds { get; set; }   //功能Id 字符串

        public BuildingObject() { }

        public BuildingObject(string name, string description, int typeId, int buildingLevel, int durableValue,int durableMax,  int buildingStatus, int haveId,string buildingFeaturesId)
        {
            this.Name = name;
            this.Description = description;
            this.TypeId = typeId;
            this.BuildingLevel = buildingLevel;
            this.DurableValue = durableValue;
            this.DurableMax = durableMax;
            this.BuildingStatus = buildingStatus;
            this.HaveId = haveId;
            this.BuildingFeaturesIds = buildingFeaturesId;
        }
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
        None,                   //空地
    }

    public enum BuildingStatus
    {
        Normal,
        UnderConstruction,
        Damage
    }


    /// <summary>
    /// LivingArea管理器，负责生成和操作livingAa的逻辑
    /// </summary>
    public class LivingAreaManager : MonoBehaviour
    {

        public List<LivingAreaNode> LivingAreas = new List<LivingAreaNode>();
        void Start()
        {
            RefreshLivingAreaData();
            ShowWindowData data = new ShowWindowData();
            data.contextData = new WindowContextLivingAreaData(LivingAreas);
            UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaTitleWindow, data);
        }

        void Update()
        {

        }

        /// <summary>
        /// 刷新所有信息
        /// </summary>
        private void RefreshLivingAreaData()
        {
            //初始化
            for (int i = 0; i < LivingAreas.Count; i++)
            {
                LivingAreaModel model = SqlData.GetModelId<LivingAreaModel>(LivingAreas[i].Id);

                LivingAreas[i].LivingAreaName = model.Name;
                LivingAreas[i].Description = model.Description;
                LivingAreas[i].PowerId = model.PowerId;
               // LivingAreas[i].HaveId = model.HaveId;
               // LivingAreas[i].TypeId = model.TypeId;
               // LivingAreas[i].BuildingLevel = model.BuildingLevel;
                LivingAreas[i].PersonNumber = model.PersonNumber;
                LivingAreas[i].LivingAreaMoney = model.LivingAreaMoney;
                LivingAreas[i].BuildingObjects = JsonMapper.ToObject<BuildingObject[]>(model.BuildingInfoJson);
                
            }

        }

        /// <summary>
        /// 进入LivingArea
        /// </summary>
        /// <param name="livingAreaId"></param>
        public void EnterLivingArea(int livingAreaId)
        {
            LivingAreaNode livingArea = null;
            for (int i = 0; i < LivingAreas.Count; i++)
            {
                if (LivingAreas[i].Id == livingAreaId)
                {
                    livingArea = LivingAreas[i];
                    break;
                }
            }

            if (livingArea != null)
            {
                ShowWindowData showWindowData = new ShowWindowData();
                showWindowData.contextData = new WindowContextLivingAreaNodeData(livingArea);
                UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, showWindowData);

                //初始化LivingArea
                





                StaticValue.Instance.EnterLivingAreaId = livingAreaId;
            }
            else
            {
                Debuger.LogError("没有这个LivingAreaId所对应的内容");
            }

        }

        public void EnterLivingArea(LivingAreaNode node)
        {
            
        }

        void OnDestroy()
        {
            UICenterMasterManager.Instance.DestroyWindow(WindowID.LivingAreaTitleWindow);
        }

        /// <summary>
        /// 生活区过一天
        /// </summary>
        public void LivingAreaToDay()
        {
            
        }

        /// <summary>
        /// 生活区过一月
        /// </summary>
        public void LivingAreaToMonthy()
        {

        }
        /// <summary>
        /// 生活区过一年
        /// </summary>
        public void LivingAreaToYear()
        {

        }

    }


}


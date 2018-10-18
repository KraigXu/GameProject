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


        private Dictionary<int, LivingAreaAction> s = new Dictionary<int, LivingAreaAction>();
        void Awake()
        {
           
        }

        void Start()
        {
            BuildingObject[] s = new BuildingObject[3];
            s[0]=new BuildingObject("酒馆","喝酒",1,3,100,100,1,1,"1,2,3");
            s[1] = new BuildingObject("市场", "购物", 1, 3, 100, 100, 1, 1, "1,2,3");
            s[2] = new BuildingObject("马场", "买马", 1, 3, 100, 100, 1, 1, "1,2,3");
            Debug.Log(JsonMapper.ToJson(s));
            //
            RefreshLivingAreaData();
            RefreshLivingAreaBehvaior();

            ShowWindowData data = new ShowWindowData();
            data.contextData = new WindowContextLivingAreaData(LivingAreas);
            UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaTitleWindow, data);
        }


        void Update()
        {
            
        }

        public void InitLivingAreaData()
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
                LivingAreas[i].Value = SqlData.GetDataId<LivingAreaData>(LivingAreas[i].Id);
                LivingAreas[i].BuildingObjects = JsonMapper.ToObject<BuildingObject[]>(LivingAreas[i].Value.BuildingInfoJson);
               // LivingAreaModel model = SqlData.GetModelId<LivingAreaModel>(LivingAreas[i].Id);
               // LivingAreas[i].LivingAreaName = model.Name;
               // LivingAreas[i].Description = model.Description;
               // LivingAreas[i].PowerId = model.PowerId;
               //// LivingAreas[i].HaveId = model.HaveId;
               //// LivingAreas[i].TypeId = model.TypeId;
               //// LivingAreas[i].BuildingLevel = model.BuildingLevel;
               // LivingAreas[i].PersonNumber = model.PersonNumber;
               // LivingAreas[i].LivingAreaMoney = model.LivingAreaMoney;
               // LivingAreas[i].BuildingObjects = JsonMapper.ToObject<BuildingObject[]>(model.BuildingInfoJson);
            }
        }

        /// <summary>
        /// 刷新所有城市信息
        /// </summary>
        private void RefreshLivingAreaBehvaior()
        {
            for (int i = 0; i < LivingAreas.Count; i++)
            {
                LivingAreaState[] groups = new LivingAreaState[3];
                groups[0] = new LivingAreaState(1, "1", "", 10, null);
                groups[1] = new LivingAreaState(2, "1", "", 10, null);
                groups[2] = new LivingAreaState(3, "1", "", 10, null);
                LivingAreas[i].Groups = groups;
            }
        }
        void OnDestroy()
        {
            UICenterMasterManager.Instance.DestroyWindow(WindowID.LivingAreaTitleWindow);
        }


        public void LivingAreaToDay()
        {
            
        }

        public void LivingAreaToMonthy()
        {

        }

        public void LivingAreaToYear()
        {

        }

    }


}


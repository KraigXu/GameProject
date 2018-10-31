using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Newtonsoft.Json;
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
        public string Key { get; set; }                         //建筑物key
        public string Name { get; set; }                       //名称                                           
        public string Description { get; set; }                 //说明
        public int BuildingLevel { get; set; }                  //建筑物等级
        public BuildingStatus Status { get; set; }                 //建筑物状态                      
        public BuildingType Type { get; set; }                   //建筑物类型    
        public int DurableValue { get; set; }                   //建筑物耐久  百分比                                                                                                        
        public int OwnId { get; set; }                          //拥有者Id                                        
        public string BuildingFeaturesIds { get; set; }         //建筑物特征
        public string MarkIds { get; set; }                     //建筑物词缀
        public string ModelPath { get; set; }                   //模型位置
        public BuildingObject() { }

        public BuildingObject(string key,string name, string description, int buildingLevel, BuildingStatus status, BuildingType type,
            int durableValue,int ownId,string buildingFeaturesIds,string markIds, string modelPath)
        {
            this.Key = key;
            this.Name = name;
            this.Description = description;
            this.BuildingLevel = buildingLevel;
            this.Status = status;
            this.Type = type;
            this.DurableValue = durableValue;
            this.OwnId = ownId;
            this.BuildingFeaturesIds = buildingFeaturesIds;
            this.MarkIds = markIds;
            this.ModelPath = modelPath;
        }
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


    /// <summary>
    /// Strategy管理器，负责生成和操作livingAa的逻辑
    /// </summary>
    public class StrategyManager : MonoBehaviour
    {
        public List<DistrictNode> Districts;
        public List<LivingAreaNode> LivingAreas;
        public Dictionary<int, LivingAreaNode> LivingAreasOwn=new Dictionary<int, LivingAreaNode>();

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitStrategyData()
        {
            List<DistrictData> districtDatas = SqlData.GetAllDatas<DistrictData>();
            for (int i = 0; i < Districts.Count; i++)
            {
                for (int j = 0; j < districtDatas.Count; j++)
                {
                    if (Districts[i].Id == districtDatas[j].Id)
                    {
                        Districts[i].Name = districtDatas[j].Name;
                        Districts[i].Description = districtDatas[j].Description;
                        Districts[i].GrowingModulus = districtDatas[j].GrowingModulus;
                        Districts[i].SecurityModulus = districtDatas[j].SecurityModulus;
                        Districts[i].Traffic = districtDatas[j].TrafficModulus;

                        string[] livingAreaids = districtDatas[j].LivinfAreasIds.Split(';');
                        for (int k = 0; k < livingAreaids.Length; k++)
                        {
                            Districts[i].LivingAreaChilds.Add(GetLivingArea(int.Parse(livingAreaids[k])));
                        }
                        break;
                    }
                }
            }

            List<LivingAreaData> livingAreaDatas = SqlData.GetAllDatas<LivingAreaData>();
            for (int i = 0; i < LivingAreas.Count; i++)
            {
                for (int j = 0; j < livingAreaDatas.Count; j++)
                {
                    if (LivingAreas[i].Id == livingAreaDatas[j].Id)
                    {
                        LivingAreas[i].Name = livingAreaDatas[j].Name;
                        LivingAreas[i].Description = livingAreaDatas[j].Description;
                        LivingAreas[i].PersonNumber = livingAreaDatas[j].PersonNumber;
                        LivingAreas[i].CurLevel = livingAreaDatas[j].LivingAreaLevel;
                        LivingAreas[i].MaxLevel = livingAreaDatas[j].LivingAreaMaxLevel;
                        LivingAreas[i].Type =(LivingAreaType)livingAreaDatas[j].LivingAreaType;
                        LivingAreas[i].Money = livingAreaDatas[j].Money;
                        LivingAreas[i].MoneyMax = livingAreaDatas[j].MoneyMax;
                        LivingAreas[i].Iron = livingAreaDatas[j].Iron;
                        LivingAreas[i].IronMax = livingAreaDatas[j].IronMax;
                        LivingAreas[i].Wood = livingAreaDatas[j].Wood;
                        LivingAreas[i].WoodMax = livingAreaDatas[j].WoodMax;
                        LivingAreas[i].Food = livingAreaDatas[j].Food;
                        LivingAreas[i].FoodMax = livingAreaDatas[j].FoodMax;
                        LivingAreas[i].DefenseStrength = livingAreaDatas[j].DefenseStrength;
                        LivingAreas[i].StableValue = livingAreaDatas[j].StableValue;
                        LivingAreas[i].BuildingObjects= JsonConvert.DeserializeObject<BuildingObject[]>(livingAreaDatas[j].BuildingInfoJson);
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定ID的LivingArea
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LivingAreaNode GetLivingArea(int id)
        {
            for (int i = 0; i < LivingAreas.Count; i++)
            {
                if (LivingAreas[i].Id == id)
                {
                    return LivingAreas[i];
                }
            }

            return null;
        }

        /// <summary>
        /// 更新所属
        /// </summary>
        public void LivingAreasChangeOwn()
        {

        }


        public void ChangeLivingAreaState()
        {
            //初始化LivingAreas
            for (int i = 0; i < LivingAreas.Count; i++)
            {

                //LivingAreas[i].Value = SqlData.GetDataId<LivingAreaData>(LivingAreas[i].Id);
                //LivingAreas[i].BuildingObjects = JsonConvert.DeserializeObject<BuildingObject[]>(LivingAreas[i].Value.BuildingInfoJson);

                //？
                LivingAreaState[] groups = new LivingAreaState[3];
                groups[0] = new LivingAreaState(1, "1", "", 10, null);
                groups[1] = new LivingAreaState(2, "1", "", 10, null);
                groups[2] = new LivingAreaState(3, "1", "", 10, null);
                LivingAreas[i].Groups = groups;
            }
        }


        /// <summary>
        /// 实例,构造这个LivingArea所有信息
        /// </summary>
        /// <param name="node"></param>
        public void InstanceLivingArea(LivingAreaNode node)
        {
        }

        public void ChangeLivingAreas()
        {

        }

        public void EnterLivingAreas(LivingAreaNode livingAreaNode, Biological biological)
        {
            //pow>rece>guanxi
            if (biological == null)
            {
                Debuger.Log("Value 为空");
                return;
            }
            switch (biological.RaceType)
            {
                case RaceType.Elf:
                    
                    break;
                case RaceType.Ghost:
                    break;
                case RaceType.Human:

                    break;
            }
            //if (livingAreaNode.Value.PowerId == biological.PowerId)
            //{
            //}
            biological.CurWhereStatus = WhereStatus.City;
        }

        public void EnterLivingAreas(LivingAreaNode livingAreaNode, List<Biological> biologicals)
        {

        }
    }
}


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
        public string Name { get; set; }                                                                  
        public string Description { get; set; }
        public int BuildingLevel { get; set; }
        public int BuildingStatus { get; set; }                                                                                                                          
        public int DurableValue { get; set; }                                                            
        public int DurableMax { get; set; }                                                                                                      
        public int HaveId { get; set; }                                                                     
        public string BuildingFeaturesIds { get; set; }
        public string ModelId { get; set; }

        public BuildingObject() { }

        public BuildingObject(string name, string description, int buildingLevel, int durableValue,int durableMax,  int buildingStatus, int haveId,string buildingFeaturesId,string modelId)
        {
            this.Name = name;
            this.Description = description;
            this.BuildingLevel = buildingLevel;
            this.DurableValue = durableValue;
            this.DurableMax = durableMax;
            this.BuildingStatus = buildingStatus;
            this.HaveId = haveId;
            this.BuildingFeaturesIds = buildingFeaturesId;
            this.ModelId = modelId;
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
                        Districts[i].Traffic = districtDatas[j].Traffic;

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
                        LivingAreas[i].Money = livingAreaDatas[j].LivingAreaMoney;
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

                LivingAreas[i].Value = SqlData.GetDataId<LivingAreaData>(LivingAreas[i].Id);
                LivingAreas[i].BuildingObjects = JsonConvert.DeserializeObject<BuildingObject[]>(LivingAreas[i].Value.BuildingInfoJson);

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


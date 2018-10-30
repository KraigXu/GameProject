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
        public List<LivingAreaNode> LivingAreas;
        public List<DistrictNode> Districts;
        public bool IsInitOver = false;

        [SerializeField]
        private Transform _livingAreasSelect;

        public void InitStrategyData()
        {
            //初始化DistrictNode
            for (int i = 0; i < Districts.Count; i++)
            {
                DistrictData data = SqlData.GetDataId<DistrictData>(Districts[i].Id);
                Districts[i].Name = data.Name;
                Districts[i].Description = data.Description;

            }
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
            IsInitOver = true;
        }
        /// <summary>
        /// 选中城市模型
        /// </summary>
        public void SelectLivingAreasModel(LivingAreaNode node)
        {
            //获取城市坐标
            _livingAreasSelect.position = node.LivingAreaRender.bounds.center;

        }

        /// <summary>
        /// 实例,构造这个LivingArea所有信息
        /// </summary>
        /// <param name="node"></param>
        public void InstanceLivingArea(LivingAreaNode node)
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
            

            if (livingAreaNode.Value.PowerId == biological.PowerId)
            {

            }
            biological.CurWhereStatus = WhereStatus.City;
        }

        public void EnterLivingAreas(LivingAreaNode livingAreaNode, List<Biological> biologicals)
        {

        }


    }
}


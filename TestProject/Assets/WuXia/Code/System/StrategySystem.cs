using System.Collections.Generic;
using DataAccessObject;
using Newtonsoft.Json;
//using Newtonsoft.Json;
using TinyFrameWork;
using Unity.Entities;
using UnityEngine;

namespace Strategy
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

        public BuildingObject(string key, string name, string description, int buildingLevel, BuildingStatus status, BuildingType type,
            int durableValue, int ownId, string buildingFeaturesIds, string markIds, string modelPath)
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
    public class StrategySystem : ComponentSystem
    {
        public bool IsShowUi=true;
        public bool CurShowUi=false;

         StrategyData m_Node;
        struct StrategyData
        {
            public ComponentArray<District> DistrictArray;
            public ComponentArray<LivingArea> LivingAreaArray;
        }

        struct LivingAreaGroup
        {
            public LivingArea LivingAreaNode;
        }


        public static void SetupComponentData(EntityManager entityManager)
        {
            List<DistrictData> districtDatas = SqlData.GetAllDatas<DistrictData>();

            District[] districtCom = GameObject.Find("StrategyManager").GetComponentsInChildren<District>();


            for (int i = 0; i < districtDatas.Count; i++)
            {
                for (int j = 0; j < districtCom.Length; j++)
                {
                    if (districtDatas[i].Id == districtCom[j].Id)
                    {
                        districtCom[j].Name = districtDatas[i].Name;

                        districtDatas.Remove(districtDatas[i]);
                        continue;
                    }
                }
            }

            LivingArea[] livingAreaCom = GameObject.Find("StrategyManager").GetComponentsInChildren<LivingArea>();
            List<LivingAreaData> livingAreaDatas = SqlData.GetAllDatas<LivingAreaData>();
            for (int i = 0; i < livingAreaCom.Length; i++)
            {
                for (int j = 0; j < livingAreaDatas.Count; j++)
                {
                    if (livingAreaCom[i].Id == livingAreaDatas[j].Id)
                    {
                        livingAreaCom[i].Name = livingAreaDatas[j].Name;
                        livingAreaCom[i].Description = livingAreaDatas[j].Description;
                        livingAreaCom[i].PersonNumber = livingAreaDatas[j].PersonNumber;
                        livingAreaCom[i].CurLevel = livingAreaDatas[j].LivingAreaLevel;
                        livingAreaCom[i].MaxLevel = livingAreaDatas[j].LivingAreaMaxLevel;
                        livingAreaCom[i].Type = (LivingAreaType)livingAreaDatas[j].LivingAreaType;
                        livingAreaCom[i].Money = livingAreaDatas[j].Money;
                        livingAreaCom[i].MoneyMax = livingAreaDatas[j].MoneyMax;
                        livingAreaCom[i].Iron = livingAreaDatas[j].Iron;
                        livingAreaCom[i].IronMax = livingAreaDatas[j].IronMax;
                        livingAreaCom[i].Wood = livingAreaDatas[j].Wood;
                        livingAreaCom[i].WoodMax = livingAreaDatas[j].WoodMax;
                        livingAreaCom[i].Food = livingAreaDatas[j].Food;
                        livingAreaCom[i].FoodMax = livingAreaDatas[j].FoodMax;
                        livingAreaCom[i].DefenseStrength = livingAreaDatas[j].DefenseStrength;
                        livingAreaCom[i].StableValue = livingAreaDatas[j].StableValue;
                        livingAreaCom[i].BuildingObjects = JsonConvert.DeserializeObject<BuildingObject[]>(livingAreaDatas[j].BuildingInfoJson);
                    }
                }
            }

            //var arch = entityManager.CreateArchetype(typeof(District));
            //var stateEntity = entityManager.CreateEntity(arch);
            // entityManager.SetComponentData(stateEntity, new District{Id =99,FactionId = 3,GrowingModulus = 4});

        }


        //protected override void OnUpdate()
        //{
        //   Debug.Log(">>>SSSSSSSSSSSSSSSSS");
        //}
        //override protected void OnUpdate()
        //{
        //     这里可以看第一个优化点：
        //     我们知道所有Rotator所经过的deltaTime是一样的，
        //     因此可以将deltaTime先保存至一个局部变量中供后续使用，
        //     这样避免了每次调用Time.deltaTime的开销。
        //    float deltaTime = Time.deltaTime;

        //     ComponentSystem.GetEntities<Group>可以高效的遍历所有符合匹配条件的GameObject
        //     匹配条件：即包含Transform又包含Rotator组件（在上面struct Group中定义）
        //    foreach (var e in GetEntities<ItemGroup>())
        //    {
        //        e.Transform.rotation *= Quaternion.AngleAxis(e.Rotator.Speed * deltaTime, Vector3.up);
        //    }
        //}
        protected override void OnUpdate()
        {
            Debug.Log("ss");
            float deltaTime = Time.deltaTime;
            // Debug.Log(m_MainGroup.GetEntityArray()[0].);

            //for (int i = 0; i < m_State.DistrictArray.Length; i++)
            //{
            //    Debug.Log(m_State.DistrictArray[i].Id);
            //}
            // Debug.Log(m_State.DistrictArray.Length);
            //for (int i = 0; i < m_State.DistrictPosition.Length; i++)
            //{
            //    //Debug.Log(m_State.DistrictPosition[i].Value.x+">>>"+ m_State.DistrictPosition[i].Value.y+">>>"+ m_State.DistrictPosition[i].Value.z);
            //}

            // Debug.Log(m_State.Length+">>>");
            //foreach (var e in GetEntities<DistrictGroup>())
            //{
            //    Debuger.Log(e.district.Id);
            //}
            Debug.Log("IsShowUi??" + IsShowUi + "???CurShowUi???" + CurShowUi);
            if (IsShowUi == true && CurShowUi == false)
            {
                Debug.Log("><");
                string[] names = new string[GetEntities<LivingAreaGroup>().Length];
                Vector3[] points= new Vector3[GetEntities<LivingAreaGroup>().Length];
                int i = 0;
                foreach (var c in GetEntities<LivingAreaGroup>())
                {
                    names[i] = c.LivingAreaNode.Name;
                    points[i] = c.LivingAreaNode.transform.position;
                }



                ShowWindowData data = new ShowWindowData();
                data.contextData = new WindowContextLivingAreaData(names,points);
                UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaTitleWindow, data);
                CurShowUi = true;
            }


        }


        ///// <summary>
        ///// 获取指定ID的LivingArea
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public LivingArea GetLivingArea(int id)
        //{
        //    for (int i = 0; i < LivingAreas.Count; i++)
        //    {
        //        if (LivingAreas[i].Id == id)
        //        {
        //            return LivingAreas[i];
        //        }
        //    }

        //    return null;
        //}

        ///// <summary>
        ///// 更新所属
        ///// </summary>
        //public void LivingAreasChangeOwn()
        //{

        //}


        //public void ChangeLivingAreaState()
        //{
        //    //初始化LivingAreas
        //    for (int i = 0; i < LivingAreas.Count; i++)
        //    {

        //        //LivingAreas[i].Value = SqlData.GetDataId<LivingAreaData>(LivingAreas[i].Id);
        //        //LivingAreas[i].BuildingObjects = JsonConvert.DeserializeObject<BuildingObject[]>(LivingAreas[i].Value.BuildingInfoJson);

        //        //？
        //        LivingAreaState[] groups = new LivingAreaState[3];
        //        groups[0] = new LivingAreaState(1, "1", "", 10, null);
        //        groups[1] = new LivingAreaState(2, "1", "", 10, null);
        //        groups[2] = new LivingAreaState(3, "1", "", 10, null);
        //        LivingAreas[i].Groups = groups;
        //    }
        //}


        ///// <summary>
        ///// 实例,构造这个LivingArea所有信息
        ///// </summary>
        ///// <param name="node"></param>
        //public void InstanceLivingArea(LivingArea node)
        //{
        //}

        //public void ChangeLivingAreas()
        //{

        //}

        //public void EnterLivingAreas(LivingArea livingAreaNode, Biological biological)
        //{
        //    //pow>rece>guanxi
        //    if (biological == null)
        //    {
        //        Debuger.Log("Value 为空");
        //        return;
        //    }
        //    switch (biological.RaceType)
        //    {
        //        case RaceType.Elf:

        //            break;
        //        case RaceType.Human:

        //            break;
        //    }
        //    //if (livingAreaNode.Value.PowerId == biological.PowerId)
        //    //{
        //    //}
        //    biological.CurWhereStatus = WhereStatus.City;
        //}

        //public void EnterLivingAreas(LivingArea livingAreaNode, List<Biological> biologicals)
        //{

        //}
    }

}
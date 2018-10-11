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
        public string Name { get; set; }       //名称
        public string Description { get; set; }          //说明
        public int TypeId { get; set; }               //类型ID
        public int BuildingLevel { get; set; }      //等级

        public BuildingObject() { }

        public BuildingObject(string name, string description, int typeId, int buildingLevel)
        {
            this.Name = name;
            this.Description = description;
            this.TypeId = typeId;
            this.BuildingLevel = buildingLevel;
        }
    }

    /// <summary>
    /// LivingArea管理器，负责生成和操作livingArea的逻辑
    /// </summary>
    public class LivingAreaManager : MonoBehaviour
    {

        public List<LivingAreaNode> LivingAreas = new List<LivingAreaNode>();
        void Start()
        {
            //BuildingObject[] s=new BuildingObject[5];
            //s[0]=new BuildingObject("皇宫","aa",1,9);
            //s[1] = new BuildingObject("衙门", "aa", 1, 2);
            //s[2] = new BuildingObject("市场", "aa", 1, 3);
            //s[3] = new BuildingObject("民房", "aa", 2, 4);
            //s[4] = new BuildingObject("酒馆", "aa", 2, 5);

            //Debug.Log(JsonMapper.ToJson(s));

            //return;//---------------------------------
            //初始化
            for (int i = 0; i < LivingAreas.Count; i++)
            {
                Debug.Log(LivingAreas[i].Id);
                ConfigLivingAreas( LivingAreas[i],SqlData.GetModelId<LivingAreaModel>(LivingAreas[i].Id));
            }

            ShowWindowData data = new ShowWindowData();
            data.contextData = new WindowContextLivingAreaData(LivingAreas);
            UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaTitleWindow, data);
        }

        void OnDisable()
        {
            UICenterMasterManager.Instance.DestroyWindow(WindowID.LivingAreaTitleWindow);
        }

        void OnEnable()
        {
           
        }


        void Update()
        {

        }

        private void ConfigLivingAreas(LivingAreaNode node,  LivingAreaModel model)
        {
            node.LivingAreaName = model.Name;
            node.Description = model.Description;
            node.PowerId = model.PowerId;
            node.HaveId = model.HaveId;
            node.TypeId = model.TypeId;
            node.BuildingLevel = model.BuildingLevel;
            node.PersonNumber = model.PersonNumber;
            node.LivingAreaMoney = model.LivingAreaMoney;
            node.BuildingObjects = JsonMapper.ToObject<BuildingObject[]>(model.BuildingInfoJson);
            

        }
    }


}


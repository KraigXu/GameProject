using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using TinyFrameWork;
using UnityEngine;

namespace LivingArea
{

    /// <summary>
    /// 居住地节点 ，核心逻辑解析解析建筑物
    /// </summary>
    public class LivingAreaNode : MonoBehaviour
    {

        public int Id;         //这个Id需要手动输入， 映射到数据库中的Id        

        public LivingAreaModel Model;

        public string Name { get { return Model.Name; } }
        public string Description { get { return Model.Description; } }
        public int RegionId { get { return Model.RegionId; } }
        public int PersonNumber { get { return Model.PersonNumber; } }
        public int LivingAreaLevel { get { return Model.LivingAreaLevel; } }
        public int LivingAreaType { get { return Model.LivingAreaType; } }
        public int PowerId { get { return Model.PowerId; } }
        public int ThaneId { get { return Model.ThaneId; } }
        public int DefenseStrength { get { return Model.DefenseStrength; } }
        public int LivingAreaMoney { get { return Model.LivingAreaMoney; } }
        public int FoodValue { get { return Model.FoodValue; } }
        public int FoodMax { get { return Model.FoodMax; } }
        public int MaterialsValue { get { return Model.MaterialsValue; } }
        public int MaterialsMax { get { return Model.MaterialsMax; } }
        public int StableValue { get { return Model.StableValue; } }
        public string BuildingInfoJson { get { return Model.BuildingInfoJson; } }

        public int Renown;      //声望

        public BuildingObject[] BuildingObjects;

        public GameObject LivingAreaM;
        void Start()
        {
            //计算数据

        }

        void Update()
        {

        }



    }
}

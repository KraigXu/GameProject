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
        public string LivingAreaName { get; set; }
        public string Description { get; set; }
        public int PowerId { get; set; }
        public int HaveId { get; set; }
        public int TypeId { get; set; }
        public int BuildingLevel { get; set; }
        public int PersonNumber { get; set; }
        public int LivingAreaMoney { get; set; }
        public int LivingAreaMoneyMax { get; set; }

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

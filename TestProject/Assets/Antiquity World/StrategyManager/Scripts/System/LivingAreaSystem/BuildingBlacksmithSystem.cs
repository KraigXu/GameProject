using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class BuildingBlacksmithSystem : BuildingSystem
    {


        struct Data
        {
            public readonly int Length;
            public EntityArray Entitys;
            public BuildingBlacksmith BuildingBlacksmith;
            
        }

        private Data _data;

        public class BuildingBlacksmithFeatures
        {
            public int Id;
            public string Name;
        }
        

        protected override void OnUpdate()
        {

        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="value"></param>
        public override void AddBuildingSystem(Entity entity, BuildingItem item)
        {
            BuildingBlacksmith blacksmith=new BuildingBlacksmith();
            blacksmith.LevelId = item.Level;
            blacksmith.ShopSeed = 10;
            blacksmith.OperateEnd = 10;
            blacksmith.OperateStart = 10;
            EntityManager.AddComponentData(entity,blacksmith);
        }

        
    }
}

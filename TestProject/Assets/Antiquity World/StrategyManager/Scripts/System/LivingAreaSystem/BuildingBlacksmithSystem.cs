using System.Collections;
using System.Collections.Generic;
using GameSystem.Ui;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class BuildingBlacksmithSystem : ComponentSystem
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 打开建筑内景视图
        /// </summary>
        /// <param name="buildingEntity"></param>
        /// <param name="biological"></param>
        public static void ShowBuildingInside(Entity buildingEntity, Entity biologicalentity, Entity livingarEntity)
        {
            BuildingWindow window = (BuildingWindow)UICenterMasterManager.Instance.ShowWindow(WindowID.BuildingWindow);

        }

        protected override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}
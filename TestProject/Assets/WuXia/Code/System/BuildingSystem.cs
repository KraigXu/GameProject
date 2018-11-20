﻿using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace WX
{

    public delegate void BuildingEvent(Entity entity, int id);

    public class BuildingSystem : ComponentSystem
    {

        struct BuildingGroup
        {
            public readonly int Length;
            public ComponentDataArray<Building> Building;
            public EntityArray Entity;
        }

        [Inject]
        private BuildingGroup _buildingGroup;

        protected override void OnUpdate()
        {
        }


        public List<BuildingiDataItem> GetUiData(int livingAreaId)
        {
            List<BuildingiDataItem> datas=new List<BuildingiDataItem>();

            for (int i = 0; i < _buildingGroup.Length; i++)
            {
                if (_buildingGroup.Building[i].ParentId == livingAreaId)
                {
                    var building = _buildingGroup.Building[i];
                    BuildingiDataItem item=new BuildingiDataItem();
                    item.Level = building.Level;
                    item.OnlyEntity = _buildingGroup.Entity[i];
                    item.Status = _buildingGroup.Building[i].Status;
                    item.ImageId = _buildingGroup.Building[i].Type;
                    item.Point = _buildingGroup.Building[i].Position;
                    item.OnOpen = BuildingOnOpen;
<<<<<<< HEAD
=======
                    item.OnClose = BuildingOnClose;
>>>>>>> parent of df74982... Update
                    datas.Add(item);
                }


            }
            return datas;
        }


<<<<<<< HEAD

        private void BuildingOnOpen(Entity entity, int id)
        {
            for (int i = 0; i < _buildingGroup.Length; i++)
            {
                if (_buildingGroup.Building[i].Id == id)
                {
                    //_buildingGroup.Building[i].DurableValue=main
                    //maindata.Id = _buildingGroup.Building[i].Id;
                    //maindata.Type = _buildingGroup.Building[i].Type;
                    //maindata.Level = _buildingGroup.Building[i].Level;
                }
            }
            //BuildingiMainData maindata = new BuildingiMainData();
            //maindata.Id = id;
            //ShowWindowData data = new ShowWindowData();
            //data.contextData = new BuildingiMainData();
        }

        private void BuildingOnClose(Entity entity, int id)
=======
        private void BuildingOnOpen(Entity entity,int id)
>>>>>>> parent of df74982... Update
        {
            
        }

        private void BuildingOnClose(Entity entity, int id)
        {

        }



        

    }

}


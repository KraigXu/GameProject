using System.Collections;
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
                    item.OnClose = BuildingOnClose;
                    datas.Add(item);
                }
            }
            return datas;
        }


        private void BuildingOnOpen(Entity entity,int id)
        {
            
        }

        private void BuildingOnClose(Entity entity, int id)
        {

        }



        

    }

}


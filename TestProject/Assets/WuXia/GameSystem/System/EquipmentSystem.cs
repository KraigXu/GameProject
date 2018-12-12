using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;


namespace GameSystem
{
    public class EquipmentSystem : ComponentSystem
    {
        struct EquipmentGroup
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<Biological> Biological;
            public ComponentDataArray<Equipment> Equipment;

        }
        [Inject]
        private  EquipmentGroup _equipmentInfo;


        protected override void OnUpdate()
        {
            for (int i = 0; i < _equipmentInfo.Length; i++)
            {
                
            }
        }


        
    }

}
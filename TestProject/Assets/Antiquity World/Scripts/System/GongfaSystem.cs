using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
namespace GameSystem
{
    public class GongfaSystem : ComponentSystem
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

        private static Dictionary<int, EquipmentJsonData> _equipmentDic = new Dictionary<int, EquipmentJsonData>();
        public static void SetData(EquipmentJsonData jsonData)
        {
            if (_equipmentDic.ContainsKey(jsonData.Id) == true)
            {
                Debug.Log("有重复数据！ Technique");
            }
            else
            {
                _equipmentDic.Add(jsonData.Id, jsonData);
            }
        }
        protected override void OnUpdate()
        {
            for (int i = 0; i < _equipmentInfo.Length; i++)
            {
                
            }
        }
        
    }

}
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Entities;
using UnityEngine;


namespace GameSystem
{
    public struct EquipmentJsonData
    {
        public int Id;
        public int HelmetId;
        public int ClothesId ;
        public int BeltId;
        public int HandGuard;
        public int Pants ;
        public int Shoes;
        public int WeaponFirstId;
        public int WeaponSecondaryId;
    }

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
        private EquipmentGroup _equipmentInfo;

        private EntityManager _entityManager;

        private static Dictionary<int, EquipmentJsonData> _equipmentDic = new Dictionary<int, EquipmentJsonData>();

        public EquipmentSystem()
        {
            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
        }
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


        //public static EquipmentJsonData GetEquipment(int equipmentId)
        //{
        //    if (_equipmentDic.ContainsKey(equipmentId) == true)
        //    {
        //        return _equipmentDic[equipmentId];
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}

        public void AddEquipment(Entity entity, string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                _entityManager.AddComponentData(entity, new Equipment
                {
                    HelmetId = -1,
                    ClothesId = -1,
                    BeltId = -1,
                    HandGuard = -1,
                    Pants = -1,
                    Shoes = -1,
                    WeaponFirstId = -1,
                    WeaponSecondaryId = -1
                });
                _entityManager.AddComponentData(entity, new EquipmentCoat
                {
                    SpriteId = 1,
                    Type = EquipType.Coat,
                    Level = EquipLevel.General,
                    Part = EquipPart.All,
                    BluntDefense = 19,
                    SharpDefense = 20,
                    Operational = 100,
                    Weight = 3,
                    Price = 1233,
                });
            }
            else
            {

                EquipmentJsonData equipmentJson = JsonConvert.DeserializeObject<EquipmentJsonData>(data);
                _entityManager.AddComponentData(entity, new Equipment
                {
                    HelmetId = equipmentJson.HelmetId,
                    ClothesId = equipmentJson.ClothesId,
                    BeltId = equipmentJson.BeltId,
                    HandGuard =equipmentJson.HandGuard,
                    Pants = equipmentJson.Pants,
                    Shoes = equipmentJson.Shoes,
                    WeaponFirstId = equipmentJson.WeaponFirstId,
                    WeaponSecondaryId = equipmentJson.WeaponSecondaryId
                });

                _entityManager.AddComponentData(entity, new EquipmentCoat
                {
                    SpriteId = 1,
                    Type = EquipType.Coat,
                    Level = EquipLevel.General,
                    Part = EquipPart.All,
                    BluntDefense = 19,
                    SharpDefense = 20,
                    Operational = 100,
                    Weight = 3,
                    Price = 1233,
                });
            }


        }




    }

}
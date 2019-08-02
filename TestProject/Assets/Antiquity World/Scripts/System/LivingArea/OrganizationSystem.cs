using DataAccessObject;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    /// <summary>
    /// 组织
    /// </summary>
    public class OrganizationSystem : ComponentSystem
    {

        struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<FactionProperty> FactionPropertys;
        }
        [Inject]
        private Data _data;
        private EntityManager _entityManager;
        protected override void OnUpdate()
        {

        }

        public static void OrganizationColliderEnter(GameObject go, Collider other)
        {

        }

        public static void OrganizationColliderExit(GameObject go, Collider other)
        {

        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            _entityManager= World.Active.GetOrCreateManager<EntityManager>();
        }


        public void AddOrganization(LivingAreaData data, HexCell cell)
        {

            _entityManager.AddComponentData(cell.Entity, new FactionProperty
            {
                //FactionEntity = factionEntity,
                FactionEntityId = 1,
                Level = Random.Range(0, 6),
            });

            _entityManager.AddComponentData(cell.Entity, new LivingArea
            {
                Id = data.Id,
                PersonNumber = data.PersonNumber,
                Type = (LivingAreaType)data.LivingAreaType,
                Money = data.Money,
                MoneyMax = data.MoneyMax,
                Iron = data.Iron,
                IronMax = data.IronMax,
                Wood = data.Wood,
                WoodMax = data.WoodMax,
                Food = data.Food,
                FoodMax = data.FoodMax,
                DefenseStrength = data.DefenseStrength,
                StableValue = data.StableValue
            });

            _entityManager.AddComponentData(cell.Entity, new Collective()
            {
                Id = data.Id,
                CollectiveClassId = 1,
                Cohesion = 1
            });

            //LivingAreaSystem.LivingAreaAddBuilding(entity, data.BuildingInfoJson);

        }




    }
}

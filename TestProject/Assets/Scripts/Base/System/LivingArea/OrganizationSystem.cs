
using System.Collections;
using System.Collections.Generic;
using GameSystem.Ui;
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
           // public EntityArray Entity;
            public FactionProperty FactionPropertys;
            public LivingArea LivingAreas;
            public HexCell HexCells;
        }
      //  
        private Data _data;
        private EntityManager _entityManager;

        private OrganizationTitleWindow _organizationTitleWindow;

        protected override void OnUpdate()
        {

            if (_organizationTitleWindow == null)
            {
                if (UICenterMasterManager.Instance.GetGameWindow(WindowID.OrganizationTitleWindow) == null)
                {
                    _organizationTitleWindow = UICenterMasterManager.Instance.ShowWindow(WindowID.OrganizationTitleWindow).GetComponent<OrganizationTitleWindow>();
                }
                else
                {
                    _organizationTitleWindow = (OrganizationTitleWindow)UICenterMasterManager.Instance.GetGameWindow(WindowID.OrganizationTitleWindow);
                }
            }

            for (int i = 0; i < _data.Length; i++)
            {
                //_organizationTitleWindow.Change(_data.LivingAreas[i],_data.FactionPropertys[i],_data.HexCells[i]);
            }


        }

        public static void OrganizationColliderEnter(GameObject go, Collider other)
        {

        }

        public static void OrganizationColliderExit(GameObject go, Collider other)
        {

        }



        public static void AddOrganization(EntityManager entityManager, LivingAreaData data, HexCell cell)
        {

            entityManager.AddComponentData(cell.Entity, new FactionProperty
            {
                //FactionEntity = factionEntity,
                FactionEntityId = 1,
                Level = Random.Range(0, 6),
            });

            entityManager.AddComponentData(cell.Entity, new LivingArea
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

            entityManager.AddComponentData(cell.Entity, new Collective()
            {
                Id = data.Id,
                CollectiveClassId = 1,
                Cohesion = 1
            });

            //LivingAreaSystem.LivingAreaAddBuilding(entity, data.BuildingInfoJson);

        }




    }
}

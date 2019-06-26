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
    public class OrganizationSystem : ComponentSystem, LivingAreaFunction
    {

        struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
            public GameObjectArray GameObjects;
            //public ComponentDataArray<LivingArea> LivingArea;
            public ComponentDataArray<Collective> Collective;
            public ComponentDataArray<CellMap> Map;
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

        public void AnalysisDataSet(Entity entity, string[] values)
        {
            
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            _entityManager= World.Active.GetOrCreateManager<EntityManager>();
        }

        public void AddOrganization(HexCoordinates coordinates)
        {
            LivingAreaData data = SQLService.Instance.QueryUnique<LivingAreaData>(" PositionX=? and PositionZ=? ", coordinates.X, coordinates.Z);

            if (data == null)
            {
                Debug.Log(coordinates.X+">>>"+coordinates.Y+">>>>"+coordinates.Z);
                return;
            }

            AddOrganization(data, coordinates);
        }


        public void AddOrganization(LivingAreaData data, HexCoordinates coordinates)
        {
            for (int i = 0; i < _data.Length; i++)
            {
                if (_data.Map[i].Coordinates.X == coordinates.X && _data.Map[i].Coordinates.Z == coordinates.Z)
                {
                    return;
                }
            }


            Entity entity = _entityManager.CreateEntity();
            _entityManager.AddComponentData(entity, new CellMap()
            {
                Coordinates = coordinates
            });

            _entityManager.AddComponentData(entity, new LivingArea
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

            _entityManager.AddComponentData(entity, new District
            {
            });

            _entityManager.AddComponentData(entity, new Money
            {
                Value = data.Money,
                Upperlimit = data.MoneyMax
            });

            _entityManager.AddComponentData(entity, new Collective()
            {
                Id = data.Id,
                CollectiveClassId = 1,
                Cohesion = 1


            });
            LivingAreaSystem.LivingAreaAddBuilding(entity, data.BuildingInfoJson);
            GameStaticData.CityName.Add(data.Id, data.Name);
            GameStaticData.CityDescription.Add(data.Id, data.Description);

        }




    }
}

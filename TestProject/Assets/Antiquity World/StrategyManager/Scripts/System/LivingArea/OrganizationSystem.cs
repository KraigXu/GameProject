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
        }
        [Inject]
        private Data _data;

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


        public static void AddOrganization(Transform node)
        {
            GameObjectEntity entitygo = node.GetComponent<GameObjectEntity>();
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            OrganizationData data = SQLService.Instance.QueryUnique<OrganizationData>(" Id=? ", GameStaticData.OrganizationName.Count + 1);

            Entity entity = entitygo.Entity;

            entityManager.AddComponentData(entity, new Collective()
            {
                Id=data.Id,
                CollectiveClassId = 1,
                Cohesion = 1


            });

            //entityManager.AddComponentData(entity, new LivingArea
            //{
            //    Id = data.Id,
            //});



            //entityManager.AddComponentData(entity, new District
            //{
            //});

            //entityManager.AddComponentData(entity, new Money
            //{
            //    Value = 1000,
            //    Upperlimit = 19999,
            //});


            GameStaticData.CityName.Add(data.Id, data.Name);
            GameStaticData.CityDescription.Add(data.Id, data.Description);

        }

        //public int Level { get; set; }
        //public int Permanence { get; set; }
        //public int ResearchValue { get; set; }
        //public int FoodValue { get; set; }
        //public int HandworkValue { get; set; }

    }
}

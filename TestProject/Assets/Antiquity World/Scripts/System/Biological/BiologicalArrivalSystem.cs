using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class BiologicalArrivalSystem : ComponentSystem
    {

        public BiologicalArrivalSystem()
        {
            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
        }
        EntityManager _entityManager;

        struct Biologicals
        {
            public readonly int Length;
            public ComponentDataArray<Biological> Biological;
            //public ComponentDataArray<BiologicalStatus> Status;
            public EntityArray Entitys;
        }

        [Inject]
        Biologicals _biological;

        [Inject] private Biologicals _biologicals;

        struct Data
        {
            public readonly int Length;
            public ComponentDataArray<Biological> Biological;
            //public ComponentDataArray<BiologicalStatus> Status;
            public EntityArray Entitys;
        }
        [Inject]
        private Data _data;

        protected override void OnUpdate()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                int i2 = 1223;



            }
            //for (int i = 0; i < UPPER; i++)
            //{
                
            //}


            //if (_biological.Length == 0)
            //    return;

            //var arrivingShipTransforms = new NativeList<Entity>(Allocator.Temp);
            //var arrivingShipData = new NativeList<Biological>(Allocator.Temp);
            //var arrivingstatus = new NativeList<BiologicalStatus>(Allocator.Temp);

            //for (int i = 0; i < _biological.Length; i++)
            //{
            //    var data = _biological.Biological[i];
            //    var entity = _biological.Entitys[i];
            //    arrivingShipTransforms.Add(entity);
            //    arrivingShipData.Add(data);
            //    arrivingstatus.Add(_biological.Status[i]);
            //}

            //HandleArrivedShips(arrivingShipData, arrivingShipTransforms, arrivingstatus);
            //arrivingShipTransforms.Dispose();
            //arrivingShipData.Dispose();
            
        }

        //void HandleArrivedShips(NativeList<Biological> arrivingShipData, NativeList<Entity> arrivingShipEntities,NativeArray<BiologicalStatus> status)
        //{
        //    for (var shipIndex = 0; shipIndex < arrivingShipData.Length; shipIndex++)
        //    {
        //        var shipData = arrivingShipData[shipIndex];
        //        var planetData = _entityManager.GetComponentData<LivingArea>(status[shipIndex].TargetEntity);
        //        var planetDat = _entityManager.GetComponentData<LivingArea>(status[shipIndex].TargetEntity);
        //        //var planetData=_entityManager.GetComponentData<>()
        //        //if (shipData.TeamOwnership != planetData.TeamOwnership)
        //        //{
        //        //    planetData.Occupants = planetData.Occupants - 1;
        //        //    if (planetData.Occupants <= 0)
        //        //    {
        //        //        planetData.TeamOwnership = shipData.TeamOwnership;
        //        //        PlanetSpawner.SetColor(shipData.TargetEntity, planetData.TeamOwnership);
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    planetData.Occupants = planetData.Occupants + 1;
        //        //}
        //        //_entityManager.SetComponentData(shipData.TargetEntity, planetData);
        //    }
        //    _entityManager.DestroyEntity(arrivingShipEntities);
        //}
    }
}


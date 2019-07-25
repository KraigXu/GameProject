using System.Collections;
using System.Collections.Generic;
using GameSystem;
using Unity.Entities;
using UnityEngine;


namespace AntiquityWorld
{
    public class MapCellSystem : ComponentSystem
    {
        struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<CellMap> Map;
        }
        [Inject]
        private Data _data;

        private EntityManager _entityManager;


        protected override void OnUpdate()
        {
        }


        public Entity GetEntity(int x, int z)
        {
            for (int i = 0; i < _data.Length; i++)
            {
                var point = _data.Map[i];
                if (point.Coordinates.X == x && point.Coordinates.Z == z)
                    return _data.Entity[i];
            }

            return Entity.Null;
        }


    }


}


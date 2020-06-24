using System.Collections;
using System.Collections.Generic;

using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class BehaviorSystem : ComponentSystem
    {
        struct Data
        {
            public readonly int Length;
            
            public BehaviorData Behaviors;
        }

       private Data _data;


        public static void SetupComponentData(EntityManager entityManager)
        {

        }

        protected override void OnUpdate()
        {

        }

        public List<Entity> GetPositionCode(int posCode)
        {
            List<Entity> values = new List<Entity>();
            for (int i = 0; i < _data.Length; i++)
            {
                //if (_data.Behaviors[i].CreantePositionCode == posCode)
                //{
                //    values.Add(_data.Entitys[i]);

                //}
            }
            return values;

        }



    }

}
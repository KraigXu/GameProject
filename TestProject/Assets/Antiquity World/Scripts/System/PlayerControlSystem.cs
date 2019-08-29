using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class PlayerControlSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {

        }


        public void SetupComponentData(EntityManager entityManager, Entity entity)
        {

            entityManager.AddComponentData(entity, new PlayerInput());


            entityManager.AddComponentData(entity,new PlayerMember());

        }

    }

}
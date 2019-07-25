using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class FightingSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {

        }


        public void AddFighting(Entity entity)
        {
            SystemManager.AddProperty(entity, new Fighting
            {

                
            });
        }
    }
}
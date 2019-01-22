using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using GameSystem.Ui;


namespace GameSystem
{
    public class InteractionSystem : ComponentSystem
    {

        struct Data
        {
            public readonly int Length;
            public ComponentDataArray<InteractionElement> Interaction;
            public EntityArray Entity;
        }
        [Inject]
        private Data _data;

        protected override void OnUpdate()
        {
          
        }
    }

}


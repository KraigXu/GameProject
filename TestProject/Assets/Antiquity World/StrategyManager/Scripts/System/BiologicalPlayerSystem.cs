using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class BiologicalPlayerSystem : ComponentSystem
    {


        struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<PlayerInput> Input;
            public ComponentDataArray<Biological> Biological;
            public GameObjectArray Gos;
        }


        protected override void OnUpdate()
        {
           // if()

        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

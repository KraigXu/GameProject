using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameSystem
{
    public class PlayerInputSystem : ComponentSystem
    {


        struct Data
        {
            public readonly int Length;
            public ComponentDataArray<PlayerInput> PInput;
        }

        [Inject]
        private Data _data;
        protected override void OnUpdate()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;


            for (int i = 0; i < _data.Length; i++)
            {


            }


        }
    }


}

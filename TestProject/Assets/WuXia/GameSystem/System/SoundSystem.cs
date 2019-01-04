using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class SoundSystem : ComponentSystem
    {
        struct SoundGroup
        {
            public readonly int Length;
            public ComponentDataArray<Sound> Sound;
        }

        protected override void OnUpdate()
        {



        }

        public void Test()
        {

        }


    }

}


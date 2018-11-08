using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace WX
{
    public class MouseSystem : ComponentSystem
    {
        
        struct MouseGroup
        {
            public Mouse mouse;
            public Position position;

        }

        protected override void OnUpdate()
        {

        }

    }

}


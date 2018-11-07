using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace WX
{
    public struct Mouse : IComponentData { }

    public struct PlayerInput : IComponentData
    {
        
        public float3 Move;
        public float3 Shoot;
        public float FireCooldown;

        public bool Fire => FireCooldown <= 0.0 && math.length(Shoot) > 0.5f;
    }

    public struct Player : IComponentData { }
    


}


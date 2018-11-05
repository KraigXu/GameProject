using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace  WX
{
    public class PlayerInput : MonoBehaviour
    {

        [HideInInspector] public float2 Move;
        [HideInInspector] public float2 Shoot;
        [HideInInspector] public float FireCooldown;
    }

}


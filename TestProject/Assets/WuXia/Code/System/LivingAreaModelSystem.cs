using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace WX
{
    public class LivingAreaModelSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return base.OnUpdate(inputDeps);
        }
    }

}


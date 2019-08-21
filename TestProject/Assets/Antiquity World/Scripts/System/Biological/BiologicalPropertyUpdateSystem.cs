using System;
using System.Collections;
using System.Collections.Generic;
using GameSystem;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class BiologicalPropertyUpdateSystem : JobComponentSystem
{
    struct BiologicalExternalProperty : IJobProcessComponentData<Biological, BodyProperty, ExternalProperty>
    {
        public void Execute(ref Biological biological,[ReadOnly]ref BodyProperty body,[ReadOnly]ref ExternalProperty externalProperty)
        {
            biological.Tizhi = body.Tizhi+ externalProperty.Tizhi;
            biological.Lidao = body.Lidao+ externalProperty.Lidao;
            biological.Jingshen =body.Jingshen+ externalProperty.Jingshen;
            biological.Lingdong =body.Lingdong+ externalProperty.Lingdong;
            biological.Wuxing =body.Wuxing+ externalProperty.Wuxing;
        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new BiologicalExternalProperty();
        return job.Schedule(this, inputDeps);
    }
}

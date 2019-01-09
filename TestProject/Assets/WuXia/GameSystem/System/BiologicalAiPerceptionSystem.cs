using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;


namespace GameSystem
{
    public class BiologicalAiPerceptionSystem : JobComponentSystem
    {

        struct BiologicalGroup
        {
            public readonly int Length;
            public ComponentDataArray<Biological> Biological;
            public ComponentDataArray<BiologicalStatus> Status;
        }
        [Inject]
        BiologicalGroup _biologicalGroup;

        [BurstCompile]
        struct AiPerception : IJobParallelFor
        {
            public ComponentDataArray<Biological> Biological;
            public ComponentDataArray<BiologicalStatus> Status;

            public void Execute(int index)
            {
                Biological biological = Biological[index];
                BiologicalStatus status = Status[index];

                for (int i = 0; i < Biological.Length; i++)
                {
                    if (status.TargetId == Biological[i].BiologicalId)  //找到目标 
                    {
                    }  
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {

            var perception = new AiPerception
            {
                Biological = _biologicalGroup.Biological,
                Status = _biologicalGroup.Status,

            };

            return perception.Schedule(_biologicalGroup.Length, 32, inputDeps);
        }

    }




}

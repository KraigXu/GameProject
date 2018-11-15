using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace WX
{
    
    public class BiologicalInteractionSystem : JobComponentSystem
    {
        struct BiologicalGroup
        {
            public readonly int Length;
            public ComponentDataArray<Biological> Biological;
            public ComponentDataArray<BiologicalStatus> Status;
        }
        [Inject]
        BiologicalGroup _biologicalGroup;

        struct InteractionGroup
        {
            public readonly int Length;
            public ComponentDataArray<InteractionElement> Interaction;
        }
        [Inject]
        InteractionGroup _interation;

        [BurstCompile]
        struct BaseInteraction : IJobParallelFor
        {
            [ReadOnly] public ComponentDataArray<Biological> Biological;
            public ComponentDataArray<BiologicalStatus> Status;

            [ReadOnly] public ComponentDataArray<InteractionElement> Interaction;

            public void Execute(int index)
            {
                Vector3 point = Status[index].Position;
                for (int i = 0; i < Interaction.Length; i++)
                {
                    if (Vector3.Distance(point, Interaction[i].Position) < Interaction[i].Distance &&
                        Status[index].TargetId== Interaction[i].Id&&
                        Status[index].StatusRealTime != Interaction[i].InteractionType&&
                        Status[index].StatusRealTime != Interaction[i].InteractionEnterType &&
                        Status[index].StatusRealTime != Interaction[i].InteractionExitType)
                    {
                        BiologicalStatus status = Status[index];
                        status.StatusRealTime = Interaction[i].InteractionEnterType;
                        Status[index] = status;
                    }
                }
            }
        }


        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var baseInt = new BaseInteraction
            {
                Biological = _biologicalGroup.Biological,
                Status = _biologicalGroup.Status,
                Interaction = _interation.Interaction
            };


            return baseInt.Schedule(_biologicalGroup.Length, 32, inputDeps);
        }
    }
}
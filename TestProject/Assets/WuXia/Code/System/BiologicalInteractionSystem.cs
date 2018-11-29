using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace WX
{
    
    public class BiologicalInteractionSystem : JobComponentSystem
    {
        struct BiologicalGroup
        {
            public readonly int Length;
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
            public ComponentDataArray<BiologicalStatus> Status;

            [ReadOnly] public ComponentDataArray<InteractionElement> Interaction;

            public void Execute(int index)
            {

                var position = Status[index].Position;
                BiologicalStatus status = Status[index];
                for (int i = 0; i < Interaction.Length; i++)
                {
                    if (Vector3.Distance(position, Interaction[i].Position) <= Interaction[i].Distance && Status[index].TargetId == Interaction[i].Id &&
                        status.TargetId==Interaction[i].Id&& status.TargetType==Interaction[i].Type&&
                        Status[index].LocationType != Interaction[i].InteractionType &&
                        Status[index].LocationType != Interaction[i].InteractionEnterType &&
                        Status[index].LocationType != Interaction[i].InteractionExitType
                        )
                    {

                        status.LocationType = Interaction[i].InteractionEnterType;
                        Status[index] = status;
                    }
                }
            }
        }


        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var baseInt = new BaseInteraction
            {
                Status = _biologicalGroup.Status,
                Interaction = _interation.Interaction
            };


            return baseInt.Schedule(_biologicalGroup.Length, 32, inputDeps);
        }
    }
}
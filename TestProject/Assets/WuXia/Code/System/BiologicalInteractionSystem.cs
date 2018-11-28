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
            public ComponentDataArray<BiologicalStatus> Status;
            public ComponentDataArray<Position> BiologicalPosition;

            [ReadOnly] public ComponentDataArray<InteractionElement> Interaction;
            [ReadOnly] public ComponentDataArray<Position> InteractionPosition;

            public void Execute(int index)
            {

                var position = BiologicalPosition[index].Value;
                BiologicalStatus status = Status[index];
                //   Vector3 point = BiologicalPosition[]  Status[index].Position;

                for (int i = 0; i < Interaction.Length; i++)
                {
                    if (Vector3.Distance(position, Interaction[i].Position) <= Interaction[i].Distance &&Status[index].TargetId == Interaction[i].Id &&
                        status.TargetId==Interaction[i].Id&& status.TargetType==In
                        )
                    {


                    }

                    if (Vector3.Distance(point, Interaction[i].Position) < Interaction[i].Distance &&
                        Status[index].TargetId== Interaction[i].Id&&
                        Status[index].LocationType != Interaction[i].InteractionType&&
                        Status[index].LocationType != Interaction[i].InteractionEnterType &&
                        Status[index].LocationType != Interaction[i].InteractionExitType)
                    {
                        BiologicalStatus status = Status[index];
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
                Biological = _biologicalGroup.Biological,
                Status = _biologicalGroup.Status,
                Interaction = _interation.Interaction
            };


            return baseInt.Schedule(_biologicalGroup.Length, 32, inputDeps);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace GameSystem
{
    public class BiologicalInteractionSystem : JobComponentSystem
    {
        struct BiologicalGroup
        {
            public readonly int Length;
            public ComponentDataArray<BiologicalStatus> Status;
            public EntityArray Entitys;
        }
        [Inject]
        BiologicalGroup _biologicalGroup;

        struct InteractionGroup
        {
            public readonly int Length;
            public ComponentDataArray<InteractionElement> Interaction;
            public EntityArray Entitys;
        }
        [Inject]
        InteractionGroup _interation;

        [BurstCompile]
        struct BaseInteraction : IJobParallelFor
        {
            public ComponentDataArray<BiologicalStatus> Status;
            public EntityArray EntitysStatus;

            public ComponentDataArray<InteractionElement> Interaction;
            public EntityArray EntityInteraction;

            // public EntityArchetype Archetype;
            // public EntityCommandBuffer CommandBuffer;

            public void Execute(int index)
            {
                var position = Status[index].Position;
                BiologicalStatus status = Status[index];
                for (int i = 0; i < Interaction.Length; i++)
                {
                    //if (Vector3.Distance(position, Interaction[i].Position) <= Interaction[i].Distance && Status[index].TargetId == Interaction[i].Id &&
                    //    status.TargetId == Interaction[i].Id && status.TargetType == Interaction[i].Type &&
                    //    Status[index].LocationType != Interaction[i].InteractionType &&
                    //    Status[index].LocationType != Interaction[i].InteractionEnterType &&
                    //    Status[index].LocationType != Interaction[i].InteractionExitType)
                    //{
                    //    status.LocationType = Interaction[i].InteractionEnterType;
                    //    Status[index] = status;

                    //    if (status.LocationType == LocationType.LivingAreaEnter)
                    //    {
                    //        LivingAreaEnterInfo livingAreaEnterInfo = new LivingAreaEnterInfo();
                    //        //livingAreaEnterInfo.LivingAreaEntity
                    //        //CommandBuffer.CreateEntity(Archetype);
                    //        //CommandBuffer.SetComponent(livingAreaEnterInfo);
                    //        //CommandBuffer.CreateEntity(ShotArchetype);
                    //        //CommandBuffer.SetComponent(spawn);
                    //        //Commandbu
                    //        //StrategySceneInit
                    //    }
                    //    else if
                    //        (status.LocationType == LocationType.LivingAreaIn)
                    //    {

                    //    }

                    //}
                }
            }
        }


        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var baseInt = new BaseInteraction
            {
                Status = _biologicalGroup.Status,
                EntitysStatus = _biologicalGroup.Entitys,
                Interaction = _interation.Interaction,
                EntityInteraction = _interation.Entitys

                //Archetype = LivingAreaEnterArchetype

            };


            return baseInt.Schedule(_biologicalGroup.Length, 32, inputDeps);
        }
    }
}
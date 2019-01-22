using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
namespace GameSystem
{
    public class BiologicalBarrier : BarrierSystem
    { }
    public class BiologicalInteractionSystem : JobComponentSystem
    {

        struct Data
        {
            public readonly int Length;
            public ComponentDataArray<InteractionElement> Insteraction;
        }

        [Inject]
        private Data _data;
        [Inject]
        private BiologicalBarrier _biologicalBarrier;

        public

        struct BiologicalCollision : IJobProcessComponentData<Biological, BiologicalStatus>
        {
            [ReadOnly]
            public ComponentDataArray<InteractionElement> Insteraction;

            [ReadOnly]
            public EntityCommandBuffer CommandBuffer;
            public EntityArchetype EventInfoArchetype;
            public void Execute(ref Biological biological, ref BiologicalStatus status)
            {
                for (int i = 0; i < Insteraction.Length; i++)
                {
                    var interaction = Insteraction[i];
                    if (Vector3.Distance(status.Position, interaction.Position) <= interaction.Distance
                        && status.TargetId == interaction.Id
                        && status.TargetType == interaction.Type
                    )
                    {
                        EventInfo eventInfo = new EventInfo();
                        eventInfo.Aid = biological.BiologicalId;
                        eventInfo.Bid = interaction.Id;
                        eventInfo.EventCode = interaction.EventCode;
                        CommandBuffer.CreateEntity(EventInfoArchetype);
                        CommandBuffer.SetComponent(eventInfo);
                    }
                }



            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new BiologicalCollision()
            {
                Insteraction = _data.Insteraction,
                CommandBuffer = _biologicalBarrier.CreateCommandBuffer(),
                EventInfoArchetype = StrategySceneInit.EventInfotype
            };
            return job.Schedule(this, inputDeps);
        }
    }


}

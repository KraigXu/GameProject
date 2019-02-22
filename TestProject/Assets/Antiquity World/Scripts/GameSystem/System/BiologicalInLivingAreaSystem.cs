using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace GameSystem
{
    public class BiologicalInLivingAreaSystem : JobComponentSystem
    {
        struct BiologicalData
        {
            public readonly int Length;
            public ComponentDataArray<Biological> Biological;
            public ComponentDataArray<Position> Position;
            public ComponentDataArray<BehaviorData> BehviorData;
        }

        struct InteractionData
        {
            public readonly int Length;
            public ComponentDataArray<Position> Position;
            public EntityArray Entitys;
        }

        [Inject] private BiologicalData _biologicalData;
        [Inject] private InteractionData _interactionData;

        [BurstCompile]
        struct CollisionJob : IJobParallelFor
        {
            public float CollisionRadiusSquared;
            public ComponentDataArray<Biological> Biological;
            [ReadOnly] public ComponentDataArray<Position> BPosition;
            public ComponentDataArray<BehaviorData> BBehviorData;

            [ReadOnly] public ComponentDataArray<Position> IPosition;
            public EntityArray Entitys;

            public void Execute(int index)
            {
                var bpos = BPosition[index].Value;
                var behavior = BBehviorData[index];

                for (int i = 0; i < Entitys.Length; i++)
                {
                    var ipos = IPosition[i].Value;
                    if (Vector3.Distance(bpos,ipos)<= CollisionRadiusSquared &&behavior.TargetEntity==Entitys[i])
                    {
                        behavior.TimeToLive = 0.0f;
                        BBehviorData[index] = behavior;
                    }
                }
            }
        }
    

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var biological = new CollisionJob
            {
                Biological = _biologicalData.Biological,
                BPosition = _biologicalData.Position,
                BBehviorData = _biologicalData.BehviorData,
                CollisionRadiusSquared = StrategySceneInit.Settings.LivingAreaCollisionRadius*StrategySceneInit.Settings.PlayerCollisionRadius,
                IPosition = _interactionData.Position,
                Entitys=_interactionData.Entitys
                
            }.Schedule(_biologicalData.Length, 1, inputDeps);

            return biological;
        }
    }


}

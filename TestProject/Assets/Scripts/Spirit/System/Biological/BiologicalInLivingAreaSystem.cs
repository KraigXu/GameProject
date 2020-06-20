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
            public Biological Biological;
            public Position Position;
            public BehaviorData BehviorData;
        }

        struct InteractionData
        {
            public readonly int Length;
            public Position Position;
            
        }

         private BiologicalData _biologicalData;
         private InteractionData _interactionData;

        [BurstCompile]
        struct CollisionJob : IJobParallelFor
        {
            public float CollisionRadiusSquared;
            public Biological Biological;
            [ReadOnly] public Position BPosition;
            public BehaviorData BBehviorData;

            [ReadOnly] public Position IPosition;
            

            public void Execute(int index)
            {
                //var bpos = BPosition[index].Value;
                //var behavior = BBehviorData[index];

                //for (int i = 0; i < Entitys.Length; i++)
                //{
                //    var ipos = IPosition[i].Value;
                //    if (Vector3.Distance(bpos,ipos)<= CollisionRadiusSquared &&behavior.TargetEntity==Entitys[i])
                //    {
                //        behavior.TimeToLive = 0.0f;
                //        BBehviorData[index] = behavior;
                //    }
                //}
            }
        }
    

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var biological = new CollisionJob
            {
                //Biological = _biologicalData.Biological,
                //BPosition = _biologicalData.Position,
                //BBehviorData = _biologicalData.BehviorData,
                //CollisionRadiusSquared = GameSceneInit.LivingAreaCollisionRadius*GameSceneInit.PlayerCollisionRadius,
                //IPosition = _interactionData.Position,
                //Entitys=_interactionData.Entitys
                
            }.Schedule(_biologicalData.Length, 1, inputDeps);

            return biological;
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace GameSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class InteractionFeedbackSystem : JobComponentSystem
    {

        


        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return base.OnUpdate(inputDeps);
        }
    }

}
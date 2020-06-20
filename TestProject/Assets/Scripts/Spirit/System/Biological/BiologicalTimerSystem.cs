using System.Collections;
using System.Collections.Generic;
using GameSystem;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class BiologicalTimerSystem : JobComponentSystem
{


    //struct BiologicalWait:IJobProcessComponentData<Biological,Timer>
    //{
    //    public void Execute(ref Biological biological, ref Timer timer)
    //    {
    //        if (timer.DayEnd == 1)
    //        {
    //            biological.Age += 1;
    //            timer.ExpendDay--;
    //            timer.DayEnd = 0;
    //        }
    //        else
    //        {

    //        }


    //    }

    //}





    //protected override JobHandle OnUpdate(JobHandle inputDeps)
    //{
    //    var job = new BiologicalWait();

    //    return inputDeps;
    //}
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        return inputDeps;
    }
}

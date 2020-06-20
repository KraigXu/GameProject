using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace  GameSystem
{
    public class EquipmentUseSystem : JobComponentSystem
    {


        //[BurstCompile]
        //struct RotationSpeedRotation : IJobProcessComponentData<Rotation, RotationSpeed>
        //{
        //    public float dt;

        //    // IJobProcessComponentData 声明了需要读取 RotationSpeed 和写入 Rotation.
        //    public void Execute(ref Rotation rotation, [ReadOnly]ref RotationSpeed speed)
        //    {
        //        rotation.Value = math.mul(math.normalize(rotation.Value), math.axisAngle(math.up(), speed.Value * dt));
        //    }
        //}

        //// 继承自JobComponentSystem会让系统为Job提供必要的依赖关系，
        //// 其它之前任何写入Rotation或RotationSpeed的JobComponentSystem都将参与依赖计算.
        //// 这里必须返回调度后的JobHandle，以便系统处理依赖执行顺序。
        //// 这样处理的优点：
        ////  * 主线程是非阻塞的，只需考虑依赖关系调度Job，当依赖项全部执行完成，Job才会执行。
        ////  * 依赖项的构成是自动计算的，因此我们可以模块化的编写多线程代码。
        //protected override JobHandle OnUpdate(JobHandle inputDeps)
        //{
        //    var job = new RotationSpeedRotation() { dt = Time.deltaTime };
        //    return job.Schedule(this, 64, inputDeps);
        //}
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return inputDeps;
        }
    }


}

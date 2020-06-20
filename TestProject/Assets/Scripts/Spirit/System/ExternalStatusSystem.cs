using GameSystem;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ExternalStatusSystem : JobComponentSystem
{





    struct Data
    {
        public readonly int Length;
        
        public ExternalProperty ExternalPropertys;
        public EquipmentHelmet EquipmentHelmets;

    }

     private Data _data;

    [BurstCompile]
    struct ExternalStatusUpdate : IJobParallelFor
    {
        public ExternalProperty ExternalPropertys;
        public EquipmentHelmet EquipmentHelmets;



        /// <summary>
        /// 并行执行for循环 i 根据length计算 打印的一直是0
        /// </summary>
        /// <param name="i"></param>
        public void Execute(int i)
        {
         //   var external = ExternalPropertys[i];


            //EquipmentHelmets



            ////Debug.Log(i); //打印的一直是0 虽然可以打印，但是会报错，希望官方会出针对 ECS 的 Debug.Log

            ////运行时间
            //float t = moveAlongCircles[i].t + (dt * moveSpeeds[i].speed);
            ////位置偏移量
            //float offsetT = t + (0.01f * i);
            //float x = moveAlongCircles[i].center.x + (math.cos(offsetT) * moveAlongCircles[i].radius);
            //float y = moveAlongCircles[i].center.y;
            //float z = moveAlongCircles[i].center.z + (math.sin(offsetT) * moveAlongCircles[i].radius);

            //moveAlongCircles[i] = new MoveAlongCircle
            //{
            //    t = t,
            //    center = moveAlongCircles[i].center,
            //    radius = moveAlongCircles[i].radius
            //};
            ////更新Logo的位置
            //positions[i] = new Position
            //{
            //    Value = new float3(x, y, z)
            //};

          //  ExternalPropertys[i] = external;
        }

    }

    //数据初始化
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var externalStatusUpdate = new ExternalStatusUpdate();

        externalStatusUpdate.EquipmentHelmets = _data.EquipmentHelmets;
        externalStatusUpdate.ExternalPropertys = _data.ExternalPropertys;

        return externalStatusUpdate.Schedule(_data.Length, 64, inputDeps);
    }


}

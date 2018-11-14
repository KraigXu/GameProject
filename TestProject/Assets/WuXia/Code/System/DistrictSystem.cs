using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace WX
{
    public class DistrictSystem : JobComponentSystem
    {
        struct DistrictGroup
        {
            public readonly int Length;
            public ComponentDataArray<District> District;

        }

        [Inject]
        private DistrictGroup _district;


        //struct RotationSpeedResetSphereRotation : IJobParallelFor
        //{
        //    //public ComponentDataArray<RotationSpeed> rotationSpeeds;
        //    //[ReadOnly] public ComponentDataArray<Position> positions;

        //    //[ReadOnly] public ComponentDataArray<RotationSpeedResetSphere> rotationSpeedResetSpheres;
        //    //[ReadOnly] public ComponentDataArray<Radius> spheres;
        //    //[ReadOnly] public ComponentDataArray<Position> rotationSpeedResetSpherePositions;

        //    //public void Execute(int i)
        //    //{
        //    //    var center = positions[i].Value;

        //    //    for (int positionIndex = 0; positionIndex < rotationSpeedResetSpheres.Length; positionIndex++)
        //    //    {
        //    //        if (math.distance(rotationSpeedResetSpherePositions[positionIndex].Value, center) < spheres[positionIndex].radius)
        //    //        {
        //    //            rotationSpeeds[i] = new RotationSpeed
        //    //            {
        //    //                Value = rotationSpeedResetSpheres[positionIndex].speed
        //    //            };
        //    //        }
        //    //    }
        //    //}
        //}

        ////public static void SetupComponentData(EntityManager entityManager)
        ////{
        ////    List<DistrictData> districtDatas = SqlData.GetAllDatas<DistrictData>();

        ////    District[] districtCom = GameObject.Find("StrategyManager").GetComponentsInChildren<District>();


        ////    for (int i = 0; i < districtDatas.Count; i++)
        ////    {
        ////        for (int j = 0; j < districtCom.Length; j++)
        ////        {
        ////            if (districtDatas[i].Id == districtCom[j].Id)
        ////            {
        ////                districtCom[j].Name = districtDatas[i].Name;

        ////                districtDatas.Remove(districtDatas[i]);
        ////                continue;
        ////            }
        ////        }
        ////    }
        ////}

        //protected override JobHandle OnUpdate(JobHandle inputDeps)
        //{
        //    //var rotationSpeedResetSphereRotationJob = new RotationSpeedResetSphereRotation
        //    //{
        //    //    rotationSpeedResetSpheres = m_RotationSpeedResetSphereGroup.rotationSpeedResetSpheres,
        //    //    spheres = m_RotationSpeedResetSphereGroup.spheres,
        //    //    rotationSpeeds = m_RotationSpeedGroup.rotationSpeeds,
        //    //    rotationSpeedResetSpherePositions = m_RotationSpeedResetSphereGroup.positions,
        //    //    positions = m_RotationSpeedGroup.positions
        //    //};
        //    return rotationSpeedResetSphereRotationJob.Schedule(m_RotationSpeedGroup.Length, 32, inputDeps);
        //}


    }

}


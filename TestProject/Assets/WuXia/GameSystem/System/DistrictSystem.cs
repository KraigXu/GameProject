using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace GameSystem
{
    public class DistrictSystem : ComponentSystem
    {
        struct DistrictGroup
        {
            public readonly int Length;
            public ComponentDataArray<District> District;
        }

        [Inject]
        private DistrictGroup _district;

        protected override void OnUpdate()
        {
            for (int i = 0; i <  _district.Length; i++)
            {
                District district = _district.District[i];

            }
            
        }


    }

}


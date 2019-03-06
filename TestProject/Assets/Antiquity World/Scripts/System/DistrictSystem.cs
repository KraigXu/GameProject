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
            public ComponentDataArray<PeriodTime> Time;
        }

        [Inject]
        private DistrictGroup _district;

        protected override void OnUpdate()
        {
            for (int i = 0; i <  _district.Length; i++)
            {
                var time = _district.Time[i];
                if (time.Value > 0)
                {
                    District district = _district.District[i];
                    district.Value++;
                    _district.District[i] = district;
                }
                time.Value = 0;
                _district.Time[i] = time;
            }
        }
    }

}


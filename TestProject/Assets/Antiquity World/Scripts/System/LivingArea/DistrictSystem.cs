using System.Collections;
using System.Collections.Generic;

using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace GameSystem
{

    /// <summary>
    /// 区域系统
    /// </summary>
    public class DistrictSystem : ComponentSystem
    {

        struct Data
        {
            public readonly int Length;
            public ComponentDataArray<District> District;

        }

        [Inject] private Data _data;

        struct DistrictGroup
        {
            public readonly int Length;
            public ComponentDataArray<District> District;
            public ComponentDataArray<PeriodTime> Time;
        }

        [Inject]
        private DistrictGroup _district;

        public void SetupComponentData(EntityManager entityManager)
        {

        }

        protected override void OnUpdate()
        {

            for (int i = 0; i < _data.Length; i++)
            {
                var district = _data.District[i];

                if (district.Type == 1)
                {
                    district.Value = district.Type * 3;
                }
                else if (district.Type == 2)
                {
                    district.Value = district.Type * 6;
                }
                else if (district.Type == 3)
                {
                    district.Value = district.Type * 9;
                }
                else if (district.Type == 4)
                {
                    district.Value = district.Type * 12;
                }
                else
                {
                    district.Value = district.Type * 1;
                }

                _data.District[i] = district;
            }


            for (int i = 0; i < _district.Length; i++)
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


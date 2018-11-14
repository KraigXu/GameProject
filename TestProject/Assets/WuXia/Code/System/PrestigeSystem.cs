﻿using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace WX
{
    public class PrestigeSystem : JobComponentSystem
    {
        struct PrestigeCheckValue
        {
            public int Max;
            public int Min;
            public int Level;
        }

        private static List<PrestigeCheckValue> _check=new List<PrestigeCheckValue>();
        //private static List<int>

        public static void SetupComponentData(EntityManager entityManager,List<int> max,List<int> min,List<int> levels)
        {

            for (int i = 0; i < levels.Count; i++)
            {
                _check.Add(new PrestigeCheckValue
                {
                    Level=levels[i],
                    Max = max[i],
                    Min = min[i],
                });
            }


        }

    }

}


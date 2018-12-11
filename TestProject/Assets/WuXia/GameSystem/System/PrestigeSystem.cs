using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    /// <summary>
    /// 声望系统
    /// </summary>
    public class PrestigeSystem : ComponentSystem
    {
        struct PrestigeCheckValue
        {
            public int Max;
            public int Min;
            public int Level;
        }

        private static List<PrestigeCheckValue> _check=new List<PrestigeCheckValue>();
        //private static List<int>

        private static Dictionary<int,PrestigeData> _prestigeDataDic=new Dictionary<int, PrestigeData>();

        public static void SetupComponentData(EntityManager entityManager)
        {
            //for (int i = 0; i < levels.Count; i++)
            //{
            //    _check.Add(new PrestigeCheckValue
            //    {
            //        Level=levels[i],
            //        Max = max[i],
            //        Min = min[i],
            //    });
            //}

            List<PrestigeData> prestigeDatas = SqlData.GetAllDatas<PrestigeData>();

            //List<int> max = new List<int>();
            //List<int> min = new List<int>();
            List<int> level = new List<int>();
            for (int i = 0; i < prestigeDatas.Count; i++)
            {
                GameStaticData.PrestigeBiolgicalDic.Add(prestigeDatas[i].LevelCode, prestigeDatas[i].BiologicalTitle);
                GameStaticData.PrestigeDistrictDic.Add(prestigeDatas[i].LevelCode, prestigeDatas[i].DistrictTitle);
                GameStaticData.PrestigeLivingAreaDic.Add(prestigeDatas[i].LevelCode, prestigeDatas[i].LivingAreaTitle);
                _prestigeDataDic.Add(prestigeDatas[i].LevelCode,prestigeDatas[i]);
               // max.Add(prestigeDatas[i].ValueMax);
              //  min.Add(prestigeDatas[i].ValueMin);
                level.Add(prestigeDatas[i].LevelCode);
            }
        }

        struct PrestigeGroup
        {
            public readonly int Length;
            public ComponentDataArray<PrestigeValue> PrestigeValue;
        }


        protected override void OnUpdate()
        {


        }
    }

}


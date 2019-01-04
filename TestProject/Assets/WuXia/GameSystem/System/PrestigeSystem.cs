﻿using System.Collections;
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
        private static List<PrestigeData> _prestigeDatas=new List<PrestigeData>();

        public static void SetupComponentData(EntityManager entityManager)
        {
             _prestigeDatas = SqlData.GetAllDatas<PrestigeData>();

            for (int i = 0; i < _prestigeDatas.Count; i++)
            {
                GameStaticData.PrestigeTitle.Add(_prestigeDatas[i].Id, _prestigeDatas[i].Title);
                
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

        public string CheckValue(int value,ElementType type)
        {
            for (int i = 0; i < _prestigeDatas.Count; i++)
            {
                if (_prestigeDatas[i].Type == (int) type && value >= _prestigeDatas[i].ValueMin && value <= _prestigeDatas[i].ValueMax)
                {
                    return _prestigeDatas[i].Title;
                }
            }
            return "";
        }
    }

}


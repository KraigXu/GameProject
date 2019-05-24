﻿using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class FamilySystem : ComponentSystem
    {
        struct Data
        {
            public readonly int Length;
            public EntityArray Entitys;
            public ComponentDataArray<Family> Familys;
        }

        [Inject] private Data _data;

        private static Dictionary<int, FamilyData> _familyDatas = new Dictionary<int, FamilyData>();

        public static void SetupComponentData(EntityManager entityManager)
        {

            _familyDatas.Clear();
            List<FamilyData> familyDatas = SQLService.Instance.QueryAll<FamilyData>();

            for (int i = 0; i < familyDatas.Count; i++)
            {
                _familyDatas.Add(familyDatas[i].Id, familyDatas[i]);
                GameStaticData.FamilyName.Add(familyDatas[i].Id, familyDatas[i].Name);
            }
        }

        protected override void OnUpdate()
        {
        }



    }

}
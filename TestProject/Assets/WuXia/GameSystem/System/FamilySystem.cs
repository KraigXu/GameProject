using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class FamilySystem : ComponentSystem
    {

        private static Dictionary<int,FamilyData> _familyDatas=new Dictionary<int, FamilyData>();

        public static void SetupComponentData(EntityManager entityManager)
        {

            _familyDatas.Clear();
            List<FamilyData> familyDatas = SqlData.GetAllDatas<FamilyData>();

            for (int i = 0; i < familyDatas.Count; i++)
            {
                _familyDatas.Add(familyDatas[i].Id,familyDatas[i]);
                GameStaticData.FamilyName.Add(familyDatas[i].Id, familyDatas[i].Name);
            }
        }

        protected override void OnUpdate()
        {
        }
    }

}
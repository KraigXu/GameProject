using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using UnityEngine;

namespace WX
{
    public class DistrictSystem : ComponentSystem
    {

        public static void SetupComponentData(EntityManager entityManager)
        {
            List<DistrictData> districtDatas = SqlData.GetAllDatas<DistrictData>();

            District[] districtCom = GameObject.Find("StrategyManager").GetComponentsInChildren<District>();


            for (int i = 0; i < districtDatas.Count; i++)
            {
                for (int j = 0; j < districtCom.Length; j++)
                {
                    if (districtDatas[i].Id == districtCom[j].Id)
                    {
                        districtCom[j].Name = districtDatas[i].Name;

                        districtDatas.Remove(districtDatas[i]);
                        continue;
                    }
                }
            }
        }

        protected override void OnUpdate()
        {
        }
    }

}


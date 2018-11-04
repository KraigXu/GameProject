using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Newtonsoft.Json;
using Strategy;
using TinyFrameWork;
using Unity.Entities;
using UnityEngine;

namespace  s
{

    public class PlayerControlSystem : ComponentSystem
    {
        public bool IsShowUi = true;
        public bool CurShowUi = false;

        [Inject] ItemGroup Node;
        struct ItemGroup
        {
            public ComponentArray<District> DistrictArray;
            public ComponentArray<LivingArea> LivingAreaArray;
        }


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

            LivingArea[] livingAreaCom = GameObject.Find("StrategyManager").GetComponentsInChildren<LivingArea>();
            List<LivingAreaData> livingAreaDatas = SqlData.GetAllDatas<LivingAreaData>();
            for (int i = 0; i < livingAreaCom.Length; i++)
            {
                for (int j = 0; j < livingAreaDatas.Count; j++)
                {
                    if (livingAreaCom[i].Id == livingAreaDatas[j].Id)
                    {
                        livingAreaCom[i].Name = livingAreaDatas[j].Name;
                        livingAreaCom[i].Description = livingAreaDatas[j].Description;
                        livingAreaCom[i].PersonNumber = livingAreaDatas[j].PersonNumber;
                        livingAreaCom[i].CurLevel = livingAreaDatas[j].LivingAreaLevel;
                        livingAreaCom[i].MaxLevel = livingAreaDatas[j].LivingAreaMaxLevel;
                        livingAreaCom[i].Type = (LivingAreaType)livingAreaDatas[j].LivingAreaType;
                        livingAreaCom[i].Money = livingAreaDatas[j].Money;
                        livingAreaCom[i].MoneyMax = livingAreaDatas[j].MoneyMax;
                        livingAreaCom[i].Iron = livingAreaDatas[j].Iron;
                        livingAreaCom[i].IronMax = livingAreaDatas[j].IronMax;
                        livingAreaCom[i].Wood = livingAreaDatas[j].Wood;
                        livingAreaCom[i].WoodMax = livingAreaDatas[j].WoodMax;
                        livingAreaCom[i].Food = livingAreaDatas[j].Food;
                        livingAreaCom[i].FoodMax = livingAreaDatas[j].FoodMax;
                        livingAreaCom[i].DefenseStrength = livingAreaDatas[j].DefenseStrength;
                        livingAreaCom[i].StableValue = livingAreaDatas[j].StableValue;
                        livingAreaCom[i].BuildingObjects = JsonConvert.DeserializeObject<BuildingObject[]>(livingAreaDatas[j].BuildingInfoJson);
                    }
                }
            }

            //var arch = entityManager.CreateArchetype(typeof(District));
            //var stateEntity = entityManager.CreateEntity(arch);
            // entityManager.SetComponentData(stateEntity, new District{Id =99,FactionId = 3,GrowingModulus = 4});

        }
        protected override void OnCreateManager()
        {
            Debuger.Log(">>>");
            //m_MainGroup = GetComponentGroup(typeof(District));
        }

        protected override void OnStartRunning()
        {
            Debug.Log(">?>");
            base.OnStartRunning();
        }

        //protected override void OnUpdate()
        //{
        //   Debug.Log(">>>SSSSSSSSSSSSSSSSS");
        //}
        //override protected void OnUpdate()
        //{
        //     这里可以看第一个优化点：
        //     我们知道所有Rotator所经过的deltaTime是一样的，
        //     因此可以将deltaTime先保存至一个局部变量中供后续使用，
        //     这样避免了每次调用Time.deltaTime的开销。
        //    float deltaTime = Time.deltaTime;

        //     ComponentSystem.GetEntities<Group>可以高效的遍历所有符合匹配条件的GameObject
        //     匹配条件：即包含Transform又包含Rotator组件（在上面struct Group中定义）
        //    foreach (var e in GetEntities<ItemGroup>())
        //    {
        //        e.Transform.rotation *= Quaternion.AngleAxis(e.Rotator.Speed * deltaTime, Vector3.up);
        //    }
        //}
        protected override void OnUpdate()
        {
            Debug.Log("ss");
            float deltaTime = Time.deltaTime;
            // Debug.Log(m_MainGroup.GetEntityArray()[0].);

            //for (int i = 0; i < m_State.DistrictArray.Length; i++)
            //{
            //    Debug.Log(m_State.DistrictArray[i].Id);
            //}
            // Debug.Log(m_State.DistrictArray.Length);
            //for (int i = 0; i < m_State.DistrictPosition.Length; i++)
            //{
            //    //Debug.Log(m_State.DistrictPosition[i].Value.x+">>>"+ m_State.DistrictPosition[i].Value.y+">>>"+ m_State.DistrictPosition[i].Value.z);
            //}

            // Debug.Log(m_State.Length+">>>");
            //foreach (var e in GetEntities<DistrictGroup>())
            //{
            //    Debuger.Log(e.district.Id);
            //}
            Debug.Log("IsShowUi??" + IsShowUi + "???CurShowUi???" + CurShowUi);
            if (IsShowUi == true && CurShowUi == false)
            {
                Debug.Log("><");
                //ShowWindowData data = new ShowWindowData();
                //data.contextData = new WindowContextLivingAreaData(Node.LivingAreaArray.ToArray());
                //UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaTitleWindow, data);
                CurShowUi = true;
            }
        }
    }
}


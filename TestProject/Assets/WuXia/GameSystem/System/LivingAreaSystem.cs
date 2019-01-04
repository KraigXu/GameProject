using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using GameSystem.Ui;

namespace GameSystem
{
    public class LivingAreaSystem : ComponentSystem
    {
        struct LivingAreaGroup
        {
            public readonly int Length;
            public ComponentDataArray<LivingArea> LivingAreaNode;
            public ComponentArray<Transform> LivingAreaPositon;
            public ComponentDataArray<PeriodTime> PeriodTime;
            public EntityArray Entity;
        }
        [Inject]
        private LivingAreaGroup _livingAreas;
        [Inject]
        private BuildingSystem _buildingSystem;

        protected override void OnUpdate()
        {
            for (int i = 0; i < _livingAreas.Length; i++)
            {
                var livingArea = _livingAreas.LivingAreaNode[i];

                if (livingArea.TitleUiId == 0)
                {
                    livingArea.TitleUiId= UICenterMasterManager.Instance .GetGameWindowScript<FixedTitleWindow>(WindowID.FixedTitleWindow).AddTitle(ElementType.LivingArea, livingArea.Id, _livingAreas.LivingAreaPositon[i].position);
                }

                var time = _livingAreas.PeriodTime[i];
                if (time.Value > 0)
                {
                    livingArea.Food += 100;
                    livingArea.Iron += 100;
                    time.Value = 0;
                }

                _livingAreas.PeriodTime[i] = time;
                _livingAreas.LivingAreaNode[i] = livingArea;
            }
        }


        public void ShowMainWindow(int id, ShowWindowData data)
        {

            UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, data);
        }

        /// <summary>
        /// 获取UI数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LivingAreaWindowCD GetLivingAreaData(int id)
        {
            LivingAreaWindowCD uidata = new LivingAreaWindowCD();
            for (int i = 0; i < _livingAreas.Length; i++)
            {
                if (_livingAreas.LivingAreaNode[i].Id != id)
                {
                    continue;
                }
                var livingArea = _livingAreas.LivingAreaNode[i];
                uidata.LivingAreaId = _livingAreas.LivingAreaNode[i].Id;
                //uidata.PowerId = _livingAreas.LivingAreaNode[i].Id;
                //uidata.ModelId = _livingAreas.LivingAreaNode[i].ModelId;
                //uidata.PersonId = _livingAreas.LivingAreaNode[i].Id;
                //uidata.PersonNumber = livingArea.PersonNumber;
                //uidata.Money = livingArea.Money;
                //uidata.MoneyMax = livingArea.MoneyMax;
                //uidata.Iron = livingArea.Iron;
                //uidata.IronMax = livingArea.IronMax;
                //uidata.Wood = livingArea.Wood;
                //uidata.WoodMax = livingArea.WoodMax;
                //uidata.Food = livingArea.Food;
                //uidata.FoodMax = livingArea.FoodMax;
                //uidata.LivingAreaLevel = livingArea.CurLevel;
                //uidata.LivingAreaMaxLevel = livingArea.MaxLevel;
                //uidata.LivingAreaType = livingArea.TypeId;
                //uidata.DefenseStrength = livingArea.DefenseStrength;
            }

            uidata.BuildingiDataItems=_buildingSystem.GetUiData(id);
            return uidata;

        }

        public LivingArea GetLivingAreaInfo(int id)
        {
            for (int i = 0; i < _livingAreas.Length; i++)
            {
                if (_livingAreas.LivingAreaNode[i].Id != id)
                {
                    return _livingAreas.LivingAreaNode[i];
                }
            }
           return  new LivingArea();
        }

        public Entity GetLivingAreaEntity(int id)
        {
            for (int i = 0; i < _livingAreas.Length; i++)
            {
                if (_livingAreas.LivingAreaNode[i].Id != id)
                {
                    return _livingAreas.Entity[i];
                }
            }
            return new Entity();
        }

        /// <summary>
        /// 检测这个ID是否存在数据
        /// </summary>
        /// <param name="livingAreaId"></param>
        /// <returns></returns>
        public bool IsTrue(int livingAreaId)
        {
            ComponentDataArray<LivingArea> livingAreas = _livingAreas.LivingAreaNode;

            for (int i = 0; i < livingAreas.Length; i++)
            {
                if (livingAreas[i].Id == livingAreaId)
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 获取指定Transform的数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public LivingArea GetLivingArea(Transform node)
        {
            ComponentArray<Transform> livingAreas = _livingAreas.LivingAreaPositon;

            for (int i = 0; i < livingAreas.Length; i++)
            {
                if (livingAreas[i] == node)
                {
                    return _livingAreas.LivingAreaNode[i];
                }
            }
            return new LivingArea();
        }

        public List<int> GetLivingAreaIds()
        {
            List<int> ids=new List<int>();

            for (int i = 0; i < _livingAreas.Length; i++)
            {
                ids.Add(_livingAreas.LivingAreaNode[i].Id);
            }
            return ids;
        }

        


    }

}



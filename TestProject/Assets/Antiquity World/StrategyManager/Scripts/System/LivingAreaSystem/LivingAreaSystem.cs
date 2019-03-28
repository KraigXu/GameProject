﻿using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using GameSystem.Ui;
using Unity.Transforms;

namespace GameSystem
{
    public class LivingAreaSystem : ComponentSystem
    {
        struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
            public GameObjectArray GameObjects;
            public ComponentDataArray<LivingArea> LivingArea;
        }
        [Inject]
        private Data _data;
        [Inject]
        private BuildingSystem _buildingSystem;
        
        private Type[] BuildingTypes=new Type[30];

        
        /// <summary>
        /// Key为LivingAreaEntity, Value=Building集的Entity
        /// </summary>
        private Dictionary<Entity,List<Entity>> _livingAreaBuildMap=new Dictionary<Entity, List<Entity>>();


        /// <summary>
        /// 
        /// </summary>
        public static void SetupInfo()
        {
            GameEventSystem.SetEvent(1000, LivingAreaEntity);
        }

        public LivingAreaSystem()
        {
            BuildingTypes[0] = typeof(BuildingBlacksmith);
        }

        public List<Entity> GetBuilding(Entity livingEntity)
        {
            if (_livingAreaBuildMap.ContainsKey(livingEntity))
            {
                return _livingAreaBuildMap[livingEntity];
            }

            return null;

        }

        /// <summary>
        /// 初始化Data
        /// </summary>
        public void DataInit()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                var entity = _data.Entity[i];
                
                
            }
        }

        public void LivingAreaAddBuilding(Entity livingentity,Entity buildingentity)
        {
            if (_livingAreaBuildMap.ContainsKey(livingentity))
            {
                _livingAreaBuildMap[livingentity].Add(buildingentity);
            }
            else
            {
                _livingAreaBuildMap.Add(livingentity, new List<Entity>
                {
                    buildingentity
                });
            }
        }


        protected override void OnUpdate()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                var livingArea = _data.LivingArea[i];
                


                //var livingArea = _data.LivingAreaNode[i];

                //if (livingArea.TitleUiId == 0)
                //{
                //    // livingArea.TitleUiId= UICenterMasterManager.Instance .GetGameWindowScript<FixedTitleWindow>(WindowID.FixedTitleWindow).AddTitle(ElementType.LivingArea, livingArea.Id, _livingAreas.LivingAreaPositon[i].position);
                //}

                //var time = _data.PeriodTime[i];
                //if (time.Value > 0)
                //{
                //    livingArea.Food += 100;
                //    livingArea.Iron += 100;
                //    time.Value = 0;
                //}
                _data.LivingArea[i] = livingArea;
                
                //_data.PeriodTime[i] = time;
                //_data.LivingAreaNode[i] = livingArea;
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
            //for (int i = 0; i < _livingAreas.Length; i++)
            //{
            //    if (_livingAreas.LivingAreaNode[i].Id != id)
            //    {
            //        continue;
            //    }
            //    var livingArea = _livingAreas.LivingAreaNode[i];
            //    uidata.LivingAreaId = _livingAreas.LivingAreaNode[i].Id;
            //    //uidata.PowerId = _livingAreas.LivingAreaNode[i].Id;
            //    //uidata.ModelId = _livingAreas.LivingAreaNode[i].ModelId;
            //    //uidata.PersonId = _livingAreas.LivingAreaNode[i].Id;
            //    //uidata.PersonNumber = livingArea.PersonNumber;
            //    //uidata.Money = livingArea.Money;
            //    //uidata.MoneyMax = livingArea.MoneyMax;
            //    //uidata.Iron = livingArea.Iron;
            //    //uidata.IronMax = livingArea.IronMax;
            //    //uidata.Wood = livingArea.Wood;
            //    //uidata.WoodMax = livingArea.WoodMax;
            //    //uidata.Food = livingArea.Food;
            //    //uidata.FoodMax = livingArea.FoodMax;
            //    //uidata.LivingAreaLevel = livingArea.CurLevel;
            //    //uidata.LivingAreaMaxLevel = livingArea.MaxLevel;
            //    //uidata.LivingAreaType = livingArea.TypeId;
            //    //uidata.DefenseStrength = livingArea.DefenseStrength;
            //}

            uidata.BuildingiDataItems = _buildingSystem.GetUiData(id);
            return uidata;

        }

        public LivingArea GetLivingAreaInfo(int id)
        {
            //for (int i = 0; i < _livingAreas.Length; i++)
            //{
            //    if (_livingAreas.LivingAreaNode[i].Id == id)
            //    {
            //        return _livingAreas.LivingAreaNode[i];
            //    }
            //}
            return new LivingArea();
        }
        public Entity GetLivingAreaEntity(int id)
        {
            //for (int i = 0; i < _livingAreas.Length; i++)
            //{
            //    if (_livingAreas.LivingAreaNode[i].Id == id)
            //    {
            //        return _livingAreas.Entity[i];
            //    }
            //}
            return new Entity();
        }

        /// <summary>
        /// 检测这个ID是否存在数据
        /// </summary>
        /// <param name="livingAreaId"></param>
        /// <returns></returns>
        public bool IsTrue(int livingAreaId)
        {
            //ComponentDataArray<LivingArea> livingAreas = _livingAreas.LivingAreaNode;

            //for (int i = 0; i < livingAreas.Length; i++)
            //{
            //    if (livingAreas[i].Id == livingAreaId)
            //    {
            //        return true;
            //    }
            //}

            return false;
        }
        /// <summary>
        /// 获取指定Transform的数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public LivingArea GetLivingArea(Transform node)
        {
            //ComponentArray<Transform> livingAreas = _livingAreas.LivingAreaPositon;

            //for (int i = 0; i < livingAreas.Length; i++)
            //{
            //    if (livingAreas[i] == node)
            //    {
            //        return _livingAreas.LivingAreaNode[i];
            //    }
            //}
            return new LivingArea();
        }
        /// <summary>
        /// 是否可以进入城市
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int IsEnterLivingArea(Entity entity, Entity livingAreaEntity)
        {
            return 0;
        }

        /// <summary>
        /// 进入城市
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="livingAreaEntity"></param>
        public static void EnterLivingArea(Entity entity, Entity livingAreaEntity)
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            entityManager.AddComponentData(entity,new LivingAreaAssociated
            {
                LivingAreaEntity = livingAreaEntity,
                BuildingEntity=Entity.Null,
            });
        }


        public List<int> GetLivingAreaIds()
        {
            List<int> ids = new List<int>();

            //for (int i = 0; i < _livingAreas.Length; i++)
            //{
            //    ids.Add(_livingAreas.LivingAreaNode[i].Id);
            //}
            return ids;
        }


        /// <summary>
        /// LivingAreaEnter  //进入方法
        /// </summary>
        /// <param name="info"></param>
        public static void LivingAreaEntity(EventInfo info)
        {
            //var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            //Entity biologicalEntity = SystemManager.Get<BiologicalSystem>().GetBiologicalEntity(info.Aid);

            //Biological biological = entityManager.GetComponentData<Biological>(biologicalEntity);
            //BiologicalStatus status = entityManager.GetComponentData<BiologicalStatus>(biologicalEntity);

            //LivingArea livingArea = SystemManager.Get<LivingAreaSystem>().GetLivingAreaInfo(info.Bid);

            //status.LocationType = LocationType.City;
            //status.LocationId = livingArea.Id;

            //status.TargetType = ElementType.None;
            //status.TargetId = 0;

            //SystemManager.Get<BiologicalSystem>().SetBiologicalStatus(biological.BiologicalId, status);

        }

        /// <summary>
        /// 进入城市
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="livingAreaEntity"></param>
        public void LivingAreaEntityCheck(Entity entity, Entity livingAreaEntity)
        {
            if (SystemManager.Contains<NpcInput>(entity))
            {


            }
            else if (SystemManager.Contains<PlayerInput>(entity))
            {
                LivingArea livingArea = SystemManager.GetProperty<LivingArea>(livingAreaEntity);

                ShowWindowData windowData = new ShowWindowData();
                LivingAreaWindowCD livingAreaWindowCd = new LivingAreaWindowCD();
                livingAreaWindowCd.LivingAreaId = livingArea.Id;
                windowData.contextData = livingAreaWindowCd;

                UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, windowData);
                //SystemManager.Get<LivingAreaSystem>().ShowMainWindow(m_Players.Status[i].TargetId, windowData);
                //// newtarget.Target = bounds.center;
                //newStatus.LocationType = LocationType.LivingAreaIn;

                // = entityManager.GetComponentData<Biological>(biologicalEntity);

            }


        }

        /// <summary>
        /// 打开建筑内景视图
        /// </summary>
        /// <param name="buildingEntity"></param>
        /// <param name="biological"></param>
        public static void ShowBuildingInside(Entity buildingEntity, Entity biologicalentity, Entity livingarEntity)
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            if (SystemManager.Contains<BuildingBlacksmith>(buildingEntity) == true )   //铁匠铺
            {
                ShowWindowData show=new ShowWindowData();
                show.contextData = new BuildingUiInfo()
                {

                };

                BuildingWindow window = (BuildingWindow)UICenterMasterManager.Instance.ShowWindow(WindowID.BuildingWindow, show);
            }
        }
    }

}



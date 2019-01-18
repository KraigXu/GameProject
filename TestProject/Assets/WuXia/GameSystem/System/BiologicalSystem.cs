using System;
using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using GameSystem.Ui;
using Newtonsoft.Json;
using Unity.Rendering;

namespace GameSystem
{
    public class BiologicalGroup
    {
        public int GroupId;
        public Biological LeaderId;
        public List<Biological> Partners = new List<Biological>();
    }
    public enum BiologicalModelType
    {
        HumanMen,
        HumanWoMen,
    }

    public enum LocationType
    {
        None = 0,
        Field = 10,
        City = 20,
        LivingAreaExit = 30,
        LivingAreaEnter = 40,
        LivingAreaIn = 50,
        SocialDialogIn = 60,
        SocialDialogEnter = 70,
        SocialDialogExit = 80,
        BuildingIn = 90,
        BuildingExit = 100,
        BuildingEnter=110,
    }
    public class BiologicalSystem : ComponentSystem
    {
        //struct BiologicalGroup
        //{
        //    public readonly int Length;
        //    public EntityArray Entity;
        //    public ComponentDataArray<Biological> Biological;
        //    public ComponentDataArray<BiologicalStatus> Status;
        //    public ComponentDataArray<InteractionElement> Interaction;
        //    public ComponentArray<CapsuleCollider> Renderer;
        //}
        //[Inject]
        //private BiologicalGroup _data;

        struct Data
        {
            public readonly int Length;
            public EntityArray Entitys;
            public ComponentDataArray<Biological> Biological;
            public ComponentDataArray<BiologicalStatus> Status;
        }
        
        [Inject]
        private Data _data;

        private EntityManager _entityManager;

        public ComponentDataArray<Biological> GetBiological
        {
            get { return _data.Biological; }
        }
        private TipsWindow _tipsWindow;

        public BiologicalSystem()
        {
            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
        }


        protected override void OnUpdate()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                var biological = _data.Biological[i];
                var status = _data.Status[i];
                var entity = _data.Entitys[i];

                biological.Jing = Convert.ToInt16(biological.Tizhi + (biological.Wuxing * 0.3f) + (biological.Lidao * 0.5f));
                biological.Qi = Convert.ToInt16(biological.Jingshen + (biological.Tizhi * 0.5f) + (biological.Wuxing * 0.5f));
                biological.Shen = Convert.ToInt16(biological.Wuxing + biological.Lidao * 0.3);

                //if (status.LocationIsInit == 0)
                //{
                //    switch (status.LocationType)
                //    {
                //        case LocationType.None:
                //            PostUpdateCommands.RemoveComponent<MeshInstanceRenderer>(entity);
                //            ModelManager.Instance.ModelStatus(status.RunModelCode, false);
                //            break;
                //        case LocationType.Field:
                //            ModelManager.Instance.ModelStatus(status.RunModelCode,true);

                //            if (biological.SexId == 0)
                //            {
                //                PostUpdateCommands.AddSharedComponent(entity, StrategySceneInit.BiologicalNormalLook);
                //            }
                //            else if (biological.SexId == 1)
                //            {
                //                PostUpdateCommands.AddSharedComponent(entity, StrategySceneInit.BiologicalManLook);
                //            }
                //            else if (biological.SexId == 2)
                //            {
                //                PostUpdateCommands.AddSharedComponent(entity, StrategySceneInit.BiologicalFemaleLook);
                //            }
                //            break;
                //        case LocationType.City:
                //            ModelManager.Instance.ModelStatus(status.RunModelCode,false);
                //            PostUpdateCommands.RemoveComponent<MeshInstanceRenderer>(entity);
                //            break;
                //        default:
                //            break;
                //    }
                //    status.LocationIsInit = 1;
                //}
                //ModelManager.Instance.ModelStatus();

                _data.Biological[i] = biological;
                _data.Status[i] = status;
                
            }
        }


        /// <summary>
        /// 根据当前状态获取Biologicals
        /// </summary>
        /// <param name="type">本地状态</param>
        /// <param name="id">本地ID</param>
        /// <returns>Entity 集合，使用ECS获取所需数据</returns>
        public List<Entity> GetBiologicalOnLocation(LocationType type, int id)
        {
            List<Entity> entities = new List<Entity>();
            for (int i = 0; i < _data.Length; i++)
            {
                if (_data.Status[i].LocationType == type && _data.Status[i].LocationId == id)
                {
                    entities.Add(_data.Entitys[i]);
                }
            }
            return entities;
        }

        /// <summary>
        /// 获取所有生物名字
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllBiologicalName()
        {
            List<int> result = new List<int>();

            for (int i = 0; i < _data.Length; i++)
            {
                result.Add(_data.Biological[i].BiologicalId);
            }
            return result;
        }

        /// <summary>
        /// 获取指定ID的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Biological GetBiologicalInfo(int id)
        {
            for (int i = 0; i < _data.Length; i++)
            {
                if (id == _data.Biological[i].BiologicalId)
                {
                    return _data.Biological[i];
                }
            }
            return new Biological();
        }


        public void EnitiyAppendComponent<T>(int id)
        {
            for (int i = 0; i < _data.Length; i++)
            {
                if (id == _data.Biological[i].BiologicalId)
                {
                    Entity entity = _data.Entitys[i];
                    _entityManager.AddComponent(entity, ComponentType.Create<T>());
                }
            }
        }

        public Entity GetBiologicalEntity(int id)
        {
            for (int i = 0; i < _data.Length; i++)
            {
                if (id == _data.Biological[i].BiologicalId)
                {
                    return _data.Entitys[i];
                }
            }
            return new Entity();
        }

        //public Vector3 GetBiologicalPosition(int id)
        //{
        //    for (int i = 0; i < _data.Length; i++)
        //    {
        //        if (id == _data.Biological[i].BiologicalId)
        //        {
        //            return _data.Position[i].Value;
        //        }
        //    }
        //    return Vector3.zero;
        //}

        //PlayerControl

        public void SetBiologicalInfo(int bid)
        {
            for (int i = 0; i < _data.Length; i++)
            {
                if (_data.Status[i].BiologicalIdentity == 1)
                {

                }
                return;
            }
        }



    }

}


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
using UnityStandardAssets.Characters.ThirdPerson;

namespace GameSystem
{
    public enum LocationType
    {
        None = 0,
        Field = 10,
        City = 20,
    }
    public class BiologicalSystem : ComponentSystem
    {
        struct Data
        {
            public readonly int Length;
            public EntityArray Entitys;
            public GameObjectArray GameObjects;
            public ComponentDataArray<Biological> Biological;
            public ComponentDataArray<BodyProperty> Body;
        }
        
        [Inject]
        private Data _data;

        private EntityManager _entityManager;
        private TipsWindow _tipsWindow;

        public class ComponentGroup
        {
            public AICharacterControl AiCharacter;
            public Animator Animator;
        }

        private Dictionary<Entity, ComponentGroup> ComponentDic;


        public BiologicalSystem()
        {
            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
        }

        /// <summary>
        /// 缓存组件
        /// </summary>
        public void InitComponent()
        {
            ComponentDic=new Dictionary<Entity, ComponentGroup>();

            for (int i = 0; i < _data.Length; i++)
            {
                var go = _data.GameObjects[i];

                ComponentGroup group=new ComponentGroup();
                group.AiCharacter = go.GetComponent<AICharacterControl>();
                group.Animator = go.GetComponent<Animator>();
                
                ComponentDic.Add(_data.Entitys[i],group);

            }
        }

        protected override void OnUpdate()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                var biological = _data.Biological[i];
                var body = _data.Body[i];
                var entity = _data.Entitys[i];

                biological.Sex = body.Fertility;
                biological.CharmValue = (20 * (body.Appearance / 100)) + (10 * (body.Dress / 100)) + (30 * (body.Skin / 100));
                biological.Mobility = (3 * (body.RightLeg / 100)) + (3 * (body.LeftLeg / 100));
                biological.OperationalAbility = (3 * (body.RightHand / 100)) + (3 * (body.LeftHand / 100));
                biological.LogicalThinking = (100 * (body.Thought / 100));

                biological.Jing= Convert.ToInt16(biological.Tizhi + (biological.Wuxing * 0.3f) + (biological.Lidao * 0.5f));
                biological.Qi = Convert.ToInt16(biological.Jingshen + (biological.Tizhi * 0.5f) + (biological.Wuxing * 0.5f));
                biological.Shen = Convert.ToInt16(biological.Wuxing + biological.Lidao * 0.3);

                _data.Biological[i] = biological;
                _data.Body[i] = body;
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
                //if (_data.Status[i].LocationType == type && _data.Status[i].LocationId == id)
                //{
                //    entities.Add(_data.Entitys[i]);
                //}
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
        //public void SetBiologicalStatus(int id, BiologicalStatus status)
        //{
        //    for (int i = 0; i < _data.Length; i++)
        //    {
        //        if (_data.Biological[i].BiologicalId == id)
        //        {
        //            _data.Status[i] = status;
        //            return;
        //        }
        //    }
        //}



        /// <summary>
        /// Biological状态改变
        /// </summary>
        /// <param name="info"></param>
        public void BiologicalStatusChange(EventInfo info)
        {



        }

    }

}


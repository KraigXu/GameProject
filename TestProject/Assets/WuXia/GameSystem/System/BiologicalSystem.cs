using System;
using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using GameSystem.Ui;

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
        None=0,
        Field = 1,    
        InLivingArea = 4,
        LivingAreaExit = 5,
        LivingAreaEnter = 6,
        LivingAreaIn = 7,
        SocialDialogIn=8,
        SocialDialogEnter=9,
        SocialDialogExit= 10,
        BuildingIn=11,
        BuildingExit=12,
        BuildingEnter13,
        
    }


    public class BiologicalSystem : ComponentSystem
    {

        struct BiologicalGroup
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<Biological> Biological;
            public ComponentDataArray<BiologicalStatus> Status;
            public ComponentDataArray<InteractionElement> Interaction;
            public ComponentArray<CapsuleCollider> Renderer;
 
        }

        [Inject]
        private BiologicalGroup _biologicalGroup;

        public ComponentDataArray<Biological> GetBiological
        {
            get { return _biologicalGroup.Biological; }
        }




        private TipsWindow _tipsWindow;
        protected override void OnUpdate()
        {
            for (int i = 0; i < _biologicalGroup.Length; i++)
            {
                var property = _biologicalGroup.Biological[i];

                var status = _biologicalGroup.Status[i];

                property.Jing = Convert.ToInt16(property.Tizhi + (property.Wuxing * 0.3f) + (property.Lidao * 0.5f));
                property.Qi = Convert.ToInt16(property.Jingshen + (property.Tizhi * 0.5f) + (property.Wuxing * 0.5f));
                property.Shen = Convert.ToInt16(property.Wuxing + property.Lidao * 0.3);

                _biologicalGroup.Biological[i] = property;

                //Update Interaction
                InteractionElement element = _biologicalGroup.Interaction[i];
                element.Position = _biologicalGroup.Renderer[i].transform.position;

                _biologicalGroup.Interaction[i] = element;

            }
        }


        /// <summary>
        /// 根据当前状态获取Biologicals
        /// </summary>
        /// <param name="type">本地状态</param>
        /// <param name="id">本地ID</param>
        /// <returns>Entity 集合，使用ECS获取所需数据</returns>
        public List<Entity> GetBiologicalOnLocation(LocationType type,int id)
        {
            List<Entity> entities =new List<Entity>();
            for (int i = 0; i < _biologicalGroup.Length; i++)
            {
                if (_biologicalGroup.Status[i].LocationType == type &&_biologicalGroup.Status[i].LocationId==id)
                {
                    entities.Add(_biologicalGroup.Entity[i]);
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
            List<int> result=new List<int>();

            for (int i = 0; i < _biologicalGroup.Length; i++)
            {
                result.Add(_biologicalGroup.Biological[i].BiologicalId);
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
            for (int i = 0; i < _biologicalGroup.Length; i++)
            {
                if (id == _biologicalGroup.Biological[i].BiologicalId)
                {
                    return _biologicalGroup.Biological[i];
                }
            }
            return new Biological();
        }
        public Entity GetBiologicalEntity(int id)
        {
            for (int i = 0; i < _biologicalGroup.Length; i++)
            {
                if (id == _biologicalGroup.Biological[i].BiologicalId)
                {
                    return _biologicalGroup.Entity[i];
                }
            }
            return new Entity();
        }





    }

}


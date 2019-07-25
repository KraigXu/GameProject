using System;
using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Newtonsoft.Json;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class TechniqueJsonData
    {
        public int Id;
        public List<KeyValuePair<int, int>> Content = new List<KeyValuePair<int, int>>();
    }


    public class TechniquesSystem : ComponentSystem
    {
        struct TechniquesGroup
        {
            public readonly int Length;
            public ComponentDataArray<Techniques> Techniues;
        }

        [Inject] private TechniquesGroup _techniques;

        private EntityManager _entityManager;

        private static Dictionary<int, TechniqueJsonData> _techniqueDic = new Dictionary<int, TechniqueJsonData>();

        public static int SetData(string json)
        {
            TechniqueJsonData jsonData = JsonConvert.DeserializeObject<TechniqueJsonData>(json);
            if (_techniqueDic.ContainsKey(jsonData.Id) == true)
            {
                Debug.Log("有重复数据！ Technique");
            }
            else
            {
                _techniqueDic.Add(jsonData.Id, jsonData);
            }

            return jsonData.Id;
        }


        protected override void OnCreateManager()
        {
            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
        }


        protected override void OnUpdate()
        {
        }

        public void SetupComponentData(EntityManager entityManager)
        {
            //List<TechniquesData> techniquesDatas = SQLService.Instance.QueryAll<TechniquesData>();
            //for (int i = 0; i < techniquesDatas.Count; i++)
            //{
            //    Entity techniques = entityManager.CreateEntity(GameSceneInit.TechniquesArchetype);
            //    entityManager.SetComponentData(techniques, new Techniques
            //    {
            //        Id = techniquesDatas[i].Id,
            //        ParentId = techniquesDatas[i].ParentId,
            //    });

            //    GameStaticData.TechniquesName.Add(techniquesDatas[i].Id, techniquesDatas[i].Name);
            //    GameStaticData.TechniquesDescription.Add(techniquesDatas[i].Id, techniquesDatas[i].Description);
            //    GameStaticData.TechniqueSprites.Add(techniquesDatas[i].Id, Resources.Load<Sprite>(techniquesDatas[i].AvatarPath));
            //}
        }

        /// <summary>
        /// 为一个实体增加技术属性
        /// </summary>
        /// <param name="target"></param>
        /// <param name="id"></param>
        public void SpawnTechnique(Entity target, int id)
        {

            _entityManager.AddComponent(target, typeof(TechniquesProperty));
            _entityManager.SetComponentData(target, new TechniquesProperty
            {
                IncreaseRate = 3,
                LowerRate = 1
            });

            List<TechniquesData> articleDatas = SQLService.Instance.SimpleQuery<TechniquesData>(" Bid=?", id);

            for (int i = 0; i < articleDatas.Count; i++)
            {

                Entity entity = _entityManager.CreateEntity(GameSceneInit.TechniquesArchetype);

                _entityManager.SetComponentData(entity, new Techniques
                {
                    BiologicalTarget = target,
                    Level = 3,

                    TechniquesValue = 300,
                    Effect = 1,
                    Value = 1
                });

            }
        }
    }

}
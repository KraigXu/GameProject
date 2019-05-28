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

        [Inject]
        private TechniquesGroup _techniques;

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



        protected override void OnUpdate()
        {


        }

        public void SetupComponentData(EntityManager entityManager)
        {
            List<TechniquesData> techniquesDatas = SQLService.Instance.QueryAll<TechniquesData>();
            for (int i = 0; i < techniquesDatas.Count; i++)
            {
                Entity techniques = entityManager.CreateEntity(GameSceneInit.TechniquesArchetype);
                entityManager.SetComponentData(techniques, new Techniques
                {
                    Id = techniquesDatas[i].Id,
                    ParentId = techniquesDatas[i].ParentId,
                });

                GameStaticData.TechniquesName.Add(techniquesDatas[i].Id, techniquesDatas[i].Name);
                GameStaticData.TechniquesDescription.Add(techniquesDatas[i].Id, techniquesDatas[i].Description);
                GameStaticData.TechniqueSprites.Add(techniquesDatas[i].Id, Resources.Load<Sprite>(techniquesDatas[i].AvatarPath));
            }
        }


        /// <summary>
        /// 获取符合BiologicalId 的数据 ，如果没有则是一个长度为0的集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Techniques> GetIdTechniques(int id)
        {
            List<Techniques> techniqueses = new List<Techniques>();

            for (int i = 0; i < _techniques.Length; i++)
            {
                if (_techniques.Techniues[i].BiologicalId == id)
                {
                    techniqueses.Add(_techniques.Techniues[i]);
                }
            }
            return techniqueses;
        }

        public static TechniqueJsonData GetTechnique(int id)
        {
            return _techniqueDic[id];
        }

        public static void SpawnTechnique(Entity target, string json)
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            Biological biological = entityManager.GetComponentData<Biological>(target);

            Dictionary<int, string> jsonDic = JsonConvert.DeserializeObject<Dictionary<int, string>>(json);

            foreach (var map in jsonDic)
            {

                TechniquesData techniques = SQLService.Instance.QueryUnique<TechniquesData>(" Id=? ", map.Key);

                Entity entity = entityManager.CreateEntity(GameSceneInit.TechniquesArchetype);

                entityManager.SetComponentData(entity, new Techniques
                {
                    BiologicalId = biological.BiologicalId,
                    BiologicalTarget = target,
                    Level = Int32.Parse(map.Value),

                    Id = techniques.Id,
                    ParentId = techniques.ParentId
                });

            }
        }
    }
}
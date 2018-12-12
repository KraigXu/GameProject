using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class RelationSystem : ComponentSystem
    {
        /// <summary>
        /// key 为biological ID，
        /// </summary>
        private static Dictionary<DoubleKey,RelationData> _relationDic =new Dictionary<DoubleKey, RelationData>();

        struct DoubleKey
        {
            public int Key2;
            public int Key1;
        }

        public static void SetupComponentData(EntityManager entityManager)
        {
            List<RelationData> relationDatas = SqlData.GetAllDatas<RelationData>();

            for (int i = 0; i < relationDatas.Count; i++)
            {
               
                DoubleKey newKey=new DoubleKey
                {
                    Key1 = relationDatas[i].MainId,
                    Key2 = relationDatas[i].AimsId
                };

                if (_relationDic.ContainsKey(newKey) == true)
                {
                    _relationDic[newKey] = relationDatas[i];
                }
                else
                {
                    _relationDic.Add(newKey, relationDatas[i]);
                }
            }
        }

        public static int GetRelationValue(int mainId, int targetId)
        {
            DoubleKey key=new DoubleKey
            {
                Key1=mainId,
                Key2 = targetId
            };

            DoubleKey key2=new DoubleKey
            {
                Key2 = mainId,
                Key1 = targetId
            };

            if (_relationDic.ContainsKey(key))
            {
                return _relationDic[key].RelationshipValue;

            }else if (_relationDic.ContainsKey(key2))
            {
                return _relationDic[key2].RelationshipValue;
            }
            else
            {
                return 0;
            }
        }


        protected override void OnUpdate()
        {
          
        }


    }

}
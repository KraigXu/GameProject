using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class TechniquesSystem : ComponentSystem
    {

        struct TechniquesGroup
        {
            public readonly int Length;
            public ComponentDataArray<Techniques> Techniues;
        }
        
        [Inject]
        private TechniquesGroup _techniques;

        protected override void OnUpdate()
        {
          
        }



        /// <summary>
        /// 获取符合BiologicalId 的数据 ，如果没有则是一个长度为0的集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Techniques> GetIdTechniques(int id)
        {
            List<Techniques> techniqueses=new List<Techniques>();

            for (int i = 0; i < _techniques.Length; i++)
            {
                if (_techniques.Techniues[i].BiologicalId == id)
                {
                    techniqueses.Add(_techniques.Techniues[i]);
                }
            }
            return techniqueses;
        }
        

        

    }
}
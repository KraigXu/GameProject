using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace GameSystem
{
    public class ModelStatusSystem : ComponentSystem
    {
        struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<ModelElement> Elements;
        }



        [Inject]
        private Data _data;

        protected override void OnUpdate()
        {

            for (int i = 0; i < _data.Length; i++)
            {
                if (_data.Elements[i].IsInit == false)
                {
                    InitRendere(_data.Elements[i]);

                }





            }

        }


        public void InitRendere(ModelElement element)
        {
            





            element.IsInit = true;
        }
    }


}

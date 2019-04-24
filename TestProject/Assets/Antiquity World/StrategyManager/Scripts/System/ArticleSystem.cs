using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
namespace GameSystem
{
    public class ArticleSystem : ComponentSystem
    {

        struct Data
        {
            public readonly int Length;
            public EntityArray Entitys;
            public ComponentDataArray<ArticleItem> Items;

        }

        [Inject]
        private Data _data;


        protected override void OnUpdate()
        {

        }

        public List<Entity> GetEntities(Entity target)
        {
            List<Entity> entities=new List<Entity>();
            for (int i = 0; i < _data.Length; i++)
            {
                if (_data.Items[i].BiologicalEntity == target)
                {
                   entities.Add(_data.Entitys[i]);
                }
            }

            return entities;
        }

    }

}

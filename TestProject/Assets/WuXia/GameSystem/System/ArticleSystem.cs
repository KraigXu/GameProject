using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;


namespace GameSystem
{
    public class ArticleJsonData
    {
        public int Id;
    }

    public class ArticleSystem : ComponentSystem
    {

        struct ArticleGroup
        {
            public readonly int Length;
            public ComponentDataArray<Article> Articles;
        }

        [Inject]
        private ArticleGroup _group;

      
        protected override void OnUpdate()
        {

        }

        
    }

}
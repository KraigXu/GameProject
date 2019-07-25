using System.Collections;
using System.Collections.Generic;
using GameSystem;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class ArticleBelongSystem : JobComponentSystem
{

    struct ArticleGroup
    {
        public readonly int Length;
        public ComponentDataArray<ArticleItem> ArticleItem;
    }

    [Inject] private ArticleGroup _articleGroup;

    struct KnapsackGroup
    {
        public readonly int Length;
        public ComponentDataArray<Knapsack> Knapsack;
        public EntityArray Entitys;
    }

    [Inject] private KnapsackGroup _knapsackGroup;

    [BurstCompile]
    struct ArticleInfoControl : IJobParallelFor
    {
        [ReadOnly]
        public ComponentDataArray<ArticleItem> Items;
        [ReadOnly]
        public EntityArray EntityArray;
        [NativeDisableParallelForRestriction]
        public ComponentDataArray<Knapsack> Knapsacks;

        public void Execute(int index)
        {
            var item = EntityArray[index];
            int value=0;
            var pro = Knapsacks[index];

            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].BiologicalEntity == item)
                {
                    value += Items[i].Count * Items[i].Weight;
                }
            }

            pro.CurUpper = value;
            Knapsacks[index] = pro;
        }

    }


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        var articleAndKnpsacks = new ArticleInfoControl
        {
            Items = _articleGroup.ArticleItem,
            EntityArray = _knapsackGroup.Entitys,
            Knapsacks = _knapsackGroup.Knapsack
        }.Schedule(_knapsackGroup.Length, 1, inputDeps);
        return articleAndKnpsacks;
    }
}

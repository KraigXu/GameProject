using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    /// <summary>
    /// 人体信息
    /// </summary>
    public class HumanOperationSystem : ComponentSystem
    {
        struct Data
        {
            public readonly int Length;
            public EntityArray Entitys;
            public ComponentDataArray<Biological> Biological;
            public ComponentDataArray<BodyProperty> Body;
        }

        [Inject] private Data _data;

        protected override void OnUpdate()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                var biological = _data.Biological[i];
                var body = _data.Body[i];

                biological.Sex = body.Fertility;
                biological.CharmValue = (20 * (body.Appearance / 100)) + (10 * (body.Dress / 100)) + (30 * (body.Skin / 100));
                biological.Mobility = (3 * (body.RightLeg / 100)) + (3 * (body.LeftLeg / 100));
                biological.OperationalAbility= (3 * (body.RightHand / 100)) + (3 * (body.LeftHand / 100));
                biological.LogicalThinking = (100 * (body.Thought/100));
                




                _data.Biological[i] = biological;

            }
        }
    }
}
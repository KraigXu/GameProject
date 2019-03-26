using GameSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace AntiquityWorld.StrategyManager
{
    /// <summary>
    /// 关联游戏对象，主要是因为ECS中没有动画和物理系统
    /// 纯数据的对象没有必要关联游戏对象
    /// </summary>
    public class GameObjectAssociationSystem : ComponentSystem
    {

        struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<AssociationPropertyData> Property;
        }
        [Inject]
        private Data _data;

        protected override void OnUpdate()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                if (_data.Property[i].IsEntityOver == 1 && _data.Property[i].IsGameObjectOver == 0)
                {
                    //var pro = _data.Property[i];
                    //Transform node = WXPoolManager.Pools[Define.GeneratedPool].Spawn(GameStaticData.ModelPrefab[pro.ModelUid].transform);
                    //AssociationEcsComponent component = node.GetComponent<AssociationEcsComponent>();
                    //if (component.Association(_data.Entity[i], pro.Position) == true)
                    //{
                    //    pro.IsGameObjectOver = 1;
                    //    _data.Property[i] = pro;
                    //}
                  
                }

            }





        }
    }


}

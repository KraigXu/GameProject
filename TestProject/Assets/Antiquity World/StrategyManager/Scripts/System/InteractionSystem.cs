using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using GameSystem.Ui;


namespace GameSystem
{
    /// <summary>
    /// 可交互物， 泛指带有模型的实体
    /// </summary>
    public class InteractionSystem : ComponentSystem
    {

        struct Data
        {
            public readonly int Length;
            public ComponentDataArray<InteractionElement> Interaction;
            
            public ComponentDataArray<Element> Element;
            public EntityArray Entity;
        }
        [Inject]
        private Data _data;

        protected override void OnUpdate()
        {
            var em = PostUpdateCommands;
            for (int i = 0; i < _data.Length; i++)
            {
                var interaction = _data.Interaction[i];
                var element = _data.Element[i];
                
            }


            //for (int i = 0; i < m_Data.Length; ++i)
            //{
            //    var sd = m_Data.SpawnData[i];
            //    var shotEntity = m_Data.SpawnedEntities[i];

            //    em.RemoveComponent<ShotSpawnData>(shotEntity);
            //    em.AddComponent(shotEntity, default(MoveForward));
            //    em.AddComponent(shotEntity, sd.Shot);
            //    em.AddComponent(shotEntity, sd.Position);
            //    em.AddComponent(shotEntity, sd.Rotation);
            //    if (sd.Faction == Factions.kPlayer)
            //    {
            //        em.AddComponent(shotEntity, new PlayerShot());
            //        em.AddComponent(shotEntity, new MoveSpeed { speed = TwoStickBootstrap.Settings.bulletMoveSpeed });
            //        em.AddSharedComponent(shotEntity, TwoStickBootstrap.PlayerShotLook);
            //    }
            //    else
            //    {
            //        em.AddComponent(shotEntity, new EnemyShot());
            //        em.AddComponent(shotEntity, new MoveSpeed { speed = TwoStickBootstrap.Settings.enemyShotSpeed });
            //        em.AddSharedComponent(shotEntity, TwoStickBootstrap.EnemyShotLook);
            //    }
            //}
        }
    }

}


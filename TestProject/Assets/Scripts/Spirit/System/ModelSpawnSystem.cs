using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class ModelSpawnSystem : ComponentSystem
    {
        struct Data
        {
            public readonly int Length;
         //   public EntityArray SpawnedEntities;
            public ModelSpawnData ModelSpawn;
        }
   
        private Data _data;

        protected override void OnUpdate()
        {
            var em = PostUpdateCommands;

            //for (int i = 0; i < _data.Length; ++i)
            //{
            //    var ms = _data.ModelSpawn[i];
            //    var shotEntity = _data.SpawnedEntities[i];

            //    em.RemoveComponent<ModelSpawnData>(shotEntity);
            //    em.AddComponent(shotEntity, ms.ModelData);

            //    //if (sd.Faction == Factions.kPlayer)
            //    //{
            //    //    em.AddComponent(shotEntity, new PlayerShot());
            //    //    em.AddComponent(shotEntity, new MoveSpeed { speed = TwoStickBootstrap.Settings.bulletMoveSpeed });
            //    //    em.AddSharedComponent(shotEntity, TwoStickBootstrap.PlayerShotLook);
            //    //}
            //    //else
            //    //{
            //    //    em.AddComponent(shotEntity, new EnemyShot());
            //    //    em.AddComponent(shotEntity, new MoveSpeed { speed = TwoStickBootstrap.Settings.enemyShotSpeed });
            //    //    em.AddSharedComponent(shotEntity, TwoStickBootstrap.EnemyShotLook);
            //    //}
            //}
        }

        /// <summary>
        /// 新增模型
        /// </summary>
        public static void SetupComponentData(Entity entity,GameObject go )
        {

           // StrategyAssetManager.Instance.s


        }
    }

}

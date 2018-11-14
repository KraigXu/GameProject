using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace WX
{
    public class LivingAreaModelSystem : JobComponentSystem
    {
        struct LivingAreaData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<LivingAreaMain> Main;
        }
        [Inject]
        private LivingAreaData _data;

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return base.OnUpdate(inputDeps);
        }

        public void OpenLivingArea(int id)
        {
            //LivingAreaData data = SqlData.GetDataId<LivingAreaData>(id);

            //GameObject go = GameObject.Instantiate(StrategySceneInit.Settings.LivingAreaModelPrefab);
            //GameObject model = GameObject.Instantiate(Resources.Load<GameObject>(data.ModelMain), go.transform);

            //var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            //Entity entity = model.GetComponent<Entity>();
            //// entityManager.EntityCapacity=mo
            

        }
    }

}


using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class LivingAreaEnterSystem : ComponentSystem
    {

        struct LivingAreaEnterInfos
        {
            public readonly int Length;
            public ComponentDataArray<LivingAreaEnterInfo> Infos;
        }

        [Inject]
        private  LivingAreaEnterInfos _enterInfos;

        private EntityManager _entityManager;

        public LivingAreaEnterSystem()
        {
            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
        }

        protected override void OnUpdate()
        {
            for (int i = 0; i < _enterInfos.Length; i++)
            {
                var livingarea=  _entityManager.GetComponentData<LivingArea>(_enterInfos.Infos[i].LivingAreaEntity);

                var biological = _entityManager.GetComponentData<Biological>(_enterInfos.Infos[i].TargetEntity);

                biological.Lingdong += 1;
                _entityManager.SetComponentData(_enterInfos.Infos[i].TargetEntity,biological);
            }
        }



    }

}


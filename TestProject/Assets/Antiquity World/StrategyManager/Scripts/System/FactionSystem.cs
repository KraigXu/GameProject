using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class FactionSystem: ComponentSystem
    {
        public struct FactionGroup
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<Faction> Faction;
        }

        [Inject]
        private FactionGroup _faction;

        private static Dictionary<Entity,FactionData> _factionDatas=new Dictionary<Entity, FactionData>();
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="entityManager"></param>
        public  void SetupComponentData(EntityManager entityManager)
        {
            EntityArchetype  factionArchetype = entityManager.CreateArchetype(typeof(Faction));
            List<FactionData> factionDatas = SQLService.Instance.QueryAll<FactionData>();
            for (int i = 0; i < factionDatas.Count; i++)
            {
                Entity faction = entityManager.CreateEntity(factionArchetype);
                entityManager.SetComponentData(faction, new Faction
                {
                    Id = factionDatas[i].Id,
                    Level = factionDatas[i].FactionLevel,
                    Food = factionDatas[i].Food,
                    FoodMax = factionDatas[i].FoodMax,
                    Iron = factionDatas[i].Iron,
                    IronMax = factionDatas[i].IronMax,
                    Money = factionDatas[i].Money,
                    MoneyMax = factionDatas[i].MoneyMax,
                    Wood = factionDatas[i].Wood,
                    WoodMax = factionDatas[i].WoodMax
                });

                _factionDatas.Add(faction, factionDatas[i]);
                GameStaticData.FactionName.Add(factionDatas[i].Id, factionDatas[i].Name);
                GameStaticData.FactionDescription.Add(factionDatas[i].Id, factionDatas[i].Description);
            }
        }

        protected override void OnUpdate()
        {

        }

        public ComponentDataArray<Faction> GetFactions()
        {
            return _faction.Faction;
        }

    }

}
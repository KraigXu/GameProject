using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class FactionStatic
    {
        public Entity entity;
        public int id;
        public string Name;
        public string Description;
    }

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


        protected override void OnUpdate()
        {
        }

        public static void SetupData()
        {
            EntityArchetype factionArchetype = SystemManager.ActiveManager.CreateArchetype(typeof(Faction));
            Entity faction = SystemManager.ActiveManager.CreateEntity(factionArchetype);

            List<FactionData> factionDatas = SQLService.Instance.QueryAll<FactionData>();
            
            for (int i = 0; i < factionDatas.Count; i++)
            {
                var factionData = factionDatas[i];
                SystemManager.ActiveManager.SetComponentData(faction, new Faction
                {
                    Id = factionData.Id,
                    Level = factionData.FactionLevel,
                    Food = factionData.Food,
                    FoodMax = factionData.FoodMax,
                    Iron = factionData.Iron,
                    IronMax = factionData.IronMax,
                    Money = factionData.Money,
                    MoneyMax = factionData.MoneyMax,
                    Wood = factionData.Wood,
                    WoodMax = factionData.WoodMax,
                    Disposition = Random.Range(0, 500),
                    NeutralValue = Random.Range(0, 500),
                    LuckValue = Random.Range(0, 500),
                    PrestigeValue = Random.Range(0, 99999)
                });

                GameStaticData.FactionName.Add(factionData.Id, factionData.Name);
                GameStaticData.FactionDescription.Add(factionData.Id, factionData.Description);
            }
        }

        public ComponentDataArray<Faction> GetFactions()
        {
            return _faction.Faction;
        }


        public Entity RandomFaction()
        {
           int index=  Random.Range(0, _faction.Length - 1);
            return _faction.Entity[index];
        }

    }

}
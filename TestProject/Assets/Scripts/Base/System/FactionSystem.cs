using System.Collections;
using System.Collections.Generic;

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

        public FactionStatic(Entity entity, int id, string name,string desc)
        {
            this.entity = entity;
            this.id = id;
            this.Name = name;
            this.Description = desc;
        }

    }


    public class FactionSystem : ComponentSystem
    {
        public struct FactionGroup
        {

            public Transform Transform;
            public Faction Faction;

        }



       override protected void OnUpdate()
        {
            //foreach (var e in GetComponentDataFromEntity<FactionGroup>())
            //{

            //}

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


               // GameStaticData.FactionStatics.Add(factionData.Id,new FactionStatic());
             ////   GameStaticData.FactionName.Add(factionData.Id, factionData.Name);
             //   GameStaticData.FactionDescription.Add(factionData.Id, factionData.Description);
            }
        }






        public static void CreateFaction(EntityManager entityManager,Entity entity, FactionData factionData)
        {
            entityManager.SetComponentData(entity,new Faction
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
            GameStaticData.FactionStatics.Add(entity, new FactionStatic(entity,factionData.Id,factionData.Name,factionData.Description));
        }

        /// <summary>
        /// 增加Faction属性
        /// </summary>
        /// <param name="entityManager"></param>
        /// <param name="factionEntity"></param>
        /// <param name="targetEntity"></param>
        public static void AddFactionCom(EntityManager entityManager, Entity factionEntity,Entity targetEntity)
        {
            if (entityManager.HasComponent<FactionProperty>(targetEntity) == false)
            {
                entityManager.AddComponentData(targetEntity,new FactionProperty()
                {
                    FactionEntity = factionEntity,
                    FactionEntityId = 1,
                    Id = 2,
                    Level = 3
                });
            }
        }

    }

}
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{

    public class FamilyRunData
    {
        public Sprite FamilyIcon;
        public string Name;
        public string Desc;

        public FamilyRunData(string name, string desc, Sprite icon)
        {
            this.Name = name;
            this.Desc = desc;
            this.FamilyIcon = icon;
        }
    }

    public class FamilySystem : ComponentSystem
    {
        struct Data
        {
            public readonly int Length;
            public EntityArray Entitys;
            public ComponentDataArray<Family> Familys;
        }
        [Inject]
        private Data _data;

        public static void SetupComponentData(EntityManager entityManager)
        {
        }
        protected override void OnUpdate()
        {

        }

        public static void CreateFamily(EntityManager entityManager, Entity entity, FamilyData familyData)
        {
            entityManager.SetComponentData(entity, new Family()
            {



            });

            GameStaticData.FamilyRunDatas.Add(entity, new FamilyRunData(familyData.Name, familyData.Description, null));
        }

        public static void AddFamilyCom(EntityManager entityManager, Entity familyEntity, Entity targetEntity)
        {
            entityManager.AddComponentData(targetEntity,new FamilyProperty()
            {
                TargetEntity = familyEntity,
            });

        }




    }

}
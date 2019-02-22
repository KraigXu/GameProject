using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace GameSystem
{
    public class FightingInitData
    {
        public int SiteType;
        public List<PersonData> Persons=new List<PersonData>();
    }

    public class PersonData
    {
        public string Name;
        public int Life;
        public int Attack;
        public int Energy;
    }



    /// <summary>
    /// 战斗场景管理
    /// </summary>
    public class FightingScene : MonoBehaviour
    {
        public static EntityArchetype FightingPerson;

        public int PlayerId;
        public TextAsset Data;



        void Awake()
        {
         //   var entityManager = World.Active.GetOrCreateManager<EntityManager>();

         //   FightingPerson = entityManager.CreateArchetype(typeof(Position),typeof(Rotation),typeof(Life),typeof(Energy));




        }

        void Start()
        {

        }

        void Update()
        {

        }


    }
}

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

    public class FightingScene : MonoBehaviour
    {
        public static EntityArchetype FightingPerson;

        private static FightingScene _instance;
        public static FightingScene Instance
        {
            get { return _instance; }
        }

        public int PlayerId;
        public TextAsset Data;
        public GameObject PlayerGo;


        public Transform SkillPrefab;


        public List<Transform> Enemy=new List<Transform>();

        void Awake()
        {
            _instance = this;

        }

        void Start()
        {

        }

        void Update()
        {

        }


    }
}

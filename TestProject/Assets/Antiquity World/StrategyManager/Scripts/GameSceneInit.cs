using System;
using System.Collections.Generic;
using System.IO;
using DataAccessObject;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;
using GameSystem.Ui;
using Manager;
using Newtonsoft.Json;
using AntiquityWorld.StrategyManager;
using Unity.Collections;


namespace GameSystem
{


    /// <summary>
    /// 开局信息
    /// </summary>
    public struct OpeningInfo
    {

        public int Mapseed;
        public int Mapx;
        public int Mapz;
        public bool GenerateMaps;
        public bool Wrapping;
        public bool IsEditMode; //是否编辑模式
        public int MapFileVersion;
        public string MapFilePath;

        //Player select Info
        public string Name;
        public string SunName;
        public int    Sex;
        


        
        public void TestValue()
        {
            Mapseed = 1308905299;
            Mapx = 80;
            Mapz = 60;
            GenerateMaps = true;
            Wrapping = true;
            IsEditMode = false;

            MapFileVersion = 5;
            MapFilePath= Path.Combine(Application.persistentDataPath, "333.map");

            this.Name = "";
            this.SunName = "";
            this.Sex = 1;
            
        }



    }

    public sealed class GameSceneInit
    {
        public static float LivingAreaCollisionRadius = 1.0f;
        public static float PlayerCollisionRadius = 1.0f;
        public static  float KeySpeed = 3f;

        public static EntityArchetype DistrictArchetype;
        public static EntityArchetype TechniquesArchetype;
        public static EntityArchetype RelationArchetype;
       
        public static EntityArchetype EventInfotype;
        public static EntityArchetype LivingAreaArchetype;
        public static EntityArchetype BiologicalSocialArchetype;
        public static EntityArchetype AncientTombArchetype;
        public static EntityArchetype ArticleArchetype;
        public static EntityArchetype FactionArchetype;

        public static OpeningInfo CurOpeningInfo;

        public static void InitializeWithScene()
        {

            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            DistrictArchetype = entityManager.CreateArchetype(typeof(District));
            LivingAreaArchetype = entityManager.CreateArchetype(typeof(Position), typeof(Rotation), typeof(LivingArea));
            TechniquesArchetype = entityManager.CreateArchetype(typeof(Techniques));
            RelationArchetype = entityManager.CreateArchetype(typeof(Relation));
           
            EventInfotype = entityManager.CreateArchetype(typeof(EventInfo));
            BiologicalSocialArchetype = entityManager.CreateArchetype(typeof(BiologicalSocial));
            AncientTombArchetype = entityManager.CreateArchetype(typeof(Position), typeof(Rotation));
            ArticleArchetype = entityManager.CreateArchetype(typeof(ArticleItem));
            FactionArchetype = entityManager.CreateArchetype(typeof(Faction));

        }

        private static MeshInstanceRenderer GetLookFromPrototype(string protoName)
        {
            var proto = GameObject.Find(protoName);
            var result = proto.GetComponent<MeshInstanceRendererComponent>().Value;
            UnityEngine.Object.Destroy(proto);
            return result;
        }

        public static void Load(string sceneName)
        {

            World.DisposeAllWorlds();
            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("SceneSwitcher/LoadCanvas"));
            SceneSwitcher ss = go.GetComponent<SceneSwitcher>();
            ss.SceneNameNext = sceneName;

        }



    }

}



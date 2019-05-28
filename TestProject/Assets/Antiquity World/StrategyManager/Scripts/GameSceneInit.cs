using System;
using System.Collections.Generic;
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
    public sealed class GameSceneInit
    {

        public static EntityArchetype DistrictArchetype;
        public static EntityArchetype TechniquesArchetype;
        public static EntityArchetype RelationArchetype;
        public static EntityArchetype BiologicalArchetype;
        public static EntityArchetype EventInfotype;
        public static EntityArchetype LivingAreaArchetype;
        public static EntityArchetype BiologicalSocialArchetype;
        public static EntityArchetype AncientTombArchetype;
        public static EntityArchetype ArticleArchetype;
        public static EntityArchetype FactionArchetype;

        public static DemoSetting Settings;

        public static void InitializeWithScene()
        {


            var settingsGo = GameObject.Find("Settings");
            if (settingsGo == null)
            {
                return;
            }
            else
            {
                Settings = settingsGo?.GetComponent<DemoSetting>();
                if (!Settings)
                {
                    return;
                }
            }
           
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            DistrictArchetype = entityManager.CreateArchetype(typeof(District));
            LivingAreaArchetype = entityManager.CreateArchetype(typeof(Position), typeof(Rotation), typeof(LivingArea));
            TechniquesArchetype = entityManager.CreateArchetype(typeof(Techniques));
            RelationArchetype = entityManager.CreateArchetype(typeof(Relation));
            BiologicalArchetype = entityManager.CreateArchetype(typeof(Element), typeof(Position), typeof(Rotation), typeof(Biological));
            EventInfotype = entityManager.CreateArchetype(typeof(EventInfo));
            BiologicalSocialArchetype = entityManager.CreateArchetype(typeof(BiologicalSocial));
            AncientTombArchetype = entityManager.CreateArchetype(typeof(Position), typeof(Rotation));
            ArticleArchetype = entityManager.CreateArchetype(typeof(ArticleItem));
            FactionArchetype = entityManager.CreateArchetype(typeof(Faction));

            SystemManager.Get<ArticleSystem>().SetupComponentData(entityManager);

            SystemManager.Get<LivingAreaSystem>().SetupComponentData(entityManager);

            SystemManager.Get<DistrictSystem>().SetupComponentData(entityManager);

            SystemManager.Get<BiologicalSystem>().SetupComponentData(entityManager);

            SystemManager.Get<TechniquesSystem>().SetupComponentData(entityManager);

            SystemManager.Get<RelationSystem>().SetupComponentData(entityManager);

            SystemManager.Get<SocialDialogSystem>().SetupComponentData(entityManager);

            SystemManager.Get<PrestigeSystem>().SetupComponentData(entityManager);

            SystemManager.Get<RelationSystem>().SetupComponentData(entityManager);

            SystemManager.Get<FactionSystem>().SetupComponentData(entityManager);

            SystemManager.Get<FamilySystem>().SetupComponentData(entityManager);

            StrategyCameraManager.Instance.SetTarget(new Vector3(-54.42019f, 50.3085f, 40.11046f));

            StrategyScene.Instance.InitStartUi();

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



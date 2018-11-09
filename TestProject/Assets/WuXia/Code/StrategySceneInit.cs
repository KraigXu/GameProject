﻿using System.Collections.Generic;
using DataAccessObject;
using TinyFrameWork;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.ThirdPerson;

namespace WX
{
    public sealed class StrategySceneInit
    {
        public static EntityArchetype DistrictArchetype;
        public static EntityArchetype LivingAreaArchetype;
        public static EntityArchetype BiologicalArchetype;
        public static EntityArchetype PlayerArchetype;
        public static EntityArchetype BasicEnemyArchetype;
        public static EntityArchetype ShotSpawnArchetype;

        public static MeshInstanceRenderer PlayerLook;
        public static MeshInstanceRenderer BiologicalLook;
        public static MeshInstanceRenderer LivingAreaLook;
        public static MeshInstanceRenderer PlayerShotLook;
        public static MeshInstanceRenderer EnemyShotLook;
        public static MeshInstanceRenderer EnemyLook;

        public static DemoSetting Settings;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            Debuger.EnableLog = true;
            // This method creates archetypes for entities we will spawn frequently in this game.
            // Archetypes are optional but can speed up entity spawning substantially.

            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            DistrictArchetype = entityManager.CreateArchetype(typeof(District));
            LivingAreaArchetype = entityManager.CreateArchetype(typeof(LivingArea),typeof(Position),typeof(Interactable));

            // Create player archetype typeof(Position), typeof(Rotation), typeof(PlayerInput),typeof(Health)[RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))][RequireComponent(typeof(ThirdPersonCharacter))]
            PlayerArchetype = entityManager.CreateArchetype(typeof(Biological), typeof(PlayerInput), typeof(Position), typeof(Rotation), typeof(AICharacterControl),typeof(Transform),typeof(NavMeshAgent),typeof(ThirdPersonCharacter),typeof(Player));
            BiologicalArchetype = entityManager.CreateArchetype(typeof(Biological),typeof(AICharacterControl));
            //BiologicalArchetype = entityManager.CreateArchetype(typeof(Biological),typeof(Position), typeof(NavMeshAgent));
            // BiologicalArchetype = entityManager.CreateArchetype(typeof(Biological), typeof(Rigidbody), typeof(Transform), typeof(CapsuleCollider), typeof(NavMeshAgent));
            //// Create an archetype for "shot spawn request" entities
            //ShotSpawnArchetype = entityManager.CreateArchetype(typeof(ShotSpawnData));

            //// Create an archetype for basic enemies.
            //BasicEnemyArchetype = entityManager.CreateArchetype(
            //    typeof(Enemy), typeof(Health), typeof(EnemyShootState),
            //    typeof(Position), typeof(Rotation),
            //    typeof(MoveSpeed), typeof(MoveForward));
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializeAfterSceneLoad()
        {
            var settingsGO = GameObject.Find("Settings");
            if (settingsGO == null)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                return;
            }
            InitializeWithScene();
        }

        // Begin a new game.
        public static void NewGame()
        {
            //获取配置
            DemoSetting settings = GameObject.Find("Settings").GetComponent<DemoSetting>();
            // Access the ECS entity manager
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            #region districtInit
            {
                //// Entity district = entityManager.CreateEntity(DistrictArchetype);
                //List<DistrictData> districtDatas = SqlData.GetAllDatas<DistrictData>();
                //District[] districtCom = GameObject.Find("StrategyManager").GetComponentsInChildren<District>();
                //for (int i = 0; i < districtDatas.Count; i++)
                //{
                //    for (int j = 0; j < districtCom.Length; j++)
                //    {
                //        if (districtDatas[i].Id == districtCom[j].Id)
                //        {
                //            districtCom[j].Name = districtDatas[i].Name;

                //            districtDatas.Remove(districtDatas[i]);
                //            continue;
                //        }
                //    }
                //}
            }
            #endregion

            #region LivingAreaInit
            {
                //DemoSetting demoSetting = GameObject.Find("Setting").GetComponent<DemoSetting>();
                //if (demoSetting.StartType == 0)
                //{
                //    GameObject playerGo = GameObject.Find("Biological").transform.Find(demoSetting.PlayerId.ToString()).gameObject;
                //    Player player = playerGo.AddComponent<Player>();
                //    PlayerInput playerInput = playerGo.AddComponent<PlayerInput>();
                //    UICenterMasterManager.Instance.ShowWindow(WindowID.StrategyWindow).GetComponent<StrategyWindow>();
                //}
                //else
                //{

                //}

            }
            #endregion

            #region BiologicalInit
            {
                Debuger.Log("BiologicalInit");

                List<BiologicalData> data = SqlData.GetWhereDatas<BiologicalData>(" IsDebut=? ", new object[] { 1 });

                for (int i = 0; i < data.Count; i++)
                {
                    Entity biologicalEntity = entityManager.CreateEntity(BiologicalArchetype);
                    entityManager.SetComponentData(biologicalEntity, new Biological
                    {
                        BiologicalId = data[i].Id,
                        RaceId = data[i].RaceType,
                        SexId = data[i].Sex,
                        Age = data[i].Age,
                        AgeMax = data[i].AgeMax,
                        Prestige = data[i].Prestige,
                        Influence = data[i].Influence,
                        Disposition = data[i].Disposition,
                        GenGu = data[i].Property1,
                        LingMin = data[i].Property2,
                        DongCha = data[i].Property3,
                        JiYi = data[i].Property4,
                        WuXing = data[i].Property5,
                        YunQi = data[i].Property6
                    });

                    float3 position = new float3(0.0f, 0.0f, -1.0f);
                    entityManager.SetComponentData(biologicalEntity, new Position { Value = position });
                    Transform t = entityManager.GetComponentObject<Transform>(biologicalEntity);
                    t.position = Vector3.zero;
                    Debug.Log(t.position);
                }

                //biologicalEntity.en
            }
            #endregion

            #region Player
            {
                PlayerControlSystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());
                Entity player = entityManager.CreateEntity(PlayerArchetype);

                // Finally we add a shared component which dictates the rendered look
                entityManager.AddSharedComponentData(player, PlayerLook);


                PlayerControlSystem.SetupPlayerView(World.Active.GetOrCreateManager<EntityManager>());
            }
            #endregion

        }

        public static void InitializeWithScene()
        {
            var settingsGO = GameObject.Find("Settings");
            if (settingsGO == null)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                return;
            }
            Settings = settingsGO?.GetComponent<DemoSetting>();
            if (!Settings)
                return;

            PlayerLook = GetLookFromPrototype("PlayerRenderPrototype");
            BiologicalLook = GetLookFromPrototype("BiologicalRenderPrototype");
            LivingAreaLook = GetLookFromPrototype("LivingAreaRenderPrototype");
            //PlayerShotLook = GetLookFromPrototype("PlayerShotRenderPrototype");
            //EnemyShotLook = GetLookFromPrototype("EnemyShotRenderPrototype");
            //EnemyLook = GetLookFromPrototype("EnemyRenderPrototype");
            //EnemySpawnSystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());

            //StrategySystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());

            //LivingAreaBuildingSystem     --暂无开启

            //  BiologicalSystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());
            // World.Active.GetOrCreateManager<UpdatePlayerHUD>().SetupGameObjects();
            //World.Active.GetOrCreateManager<StrategySystem>().Update();

            //获取配置
            DemoSetting settings = GameObject.Find("Settings").GetComponent<DemoSetting>();
            // Access the ECS entity manager
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            #region Time

            {
                WorldTimeSystem.CurWorldTime = settings.curTime;
            }

            #endregion

            #region districtInit
            {
                List<DistrictData> districtDatas = SqlData.GetAllDatas<DistrictData>();

                for (int i = 0; i < districtDatas.Count; i++)
                {
                    Entity district = entityManager.CreateEntity(DistrictArchetype);

                    entityManager.SetComponentData(district,new District
                    {
                         DistrictId=districtDatas[i].Id,
                         FactionId = districtDatas[i].Id,
                         GrowingModulus = districtDatas[i].GrowingModulus,
                         SecurityModulus = districtDatas[i].SecurityModulus,
                         Traffic = districtDatas[i].TrafficModulus
                    });
                }
                //DistrictSystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());
            }
            #endregion

            #region LivingAreaInit
            {
                List<LivingAreaData> livingAreaDatas = SqlData.GetAllDatas<LivingAreaData>();
                for (int i = 0; i < livingAreaDatas.Count; i++)
                {
                    var go = GameObject.Instantiate(Settings.LivingAreaPrefab,new Vector3(livingAreaDatas[i].PositionX, livingAreaDatas[i].PositionY, livingAreaDatas[i].PositionZ),Quaternion.identity);

                    Entity livingArea = go.GetComponent<GameObjectEntity>().Entity;
                    entityManager.AddComponent(livingArea,ComponentType.Create<LivingArea>());
                    entityManager.SetComponentData(livingArea,new LivingArea
                    {
                        Id=livingAreaDatas[i].Id,
                        PersonNumber=livingAreaDatas[i].PersonNumber,
                        CurLevel= livingAreaDatas[i].LivingAreaLevel,
                        MaxLevel=livingAreaDatas[i].LivingAreaMaxLevel,
                        TypeId = livingAreaDatas[i].LivingAreaType,
                        Money = livingAreaDatas[i].Money,
                        MoneyMax = livingAreaDatas[i].MoneyMax,
                        Iron = livingAreaDatas[i].Iron,
                        IronMax = livingAreaDatas[i].IronMax,
                        Wood = livingAreaDatas[i].Wood,
                        WoodMax = livingAreaDatas[i].WoodMax,
                        Food = livingAreaDatas[i].Food,
                        FoodMax = livingAreaDatas[i].FoodMax,
                        DefenseStrength = livingAreaDatas[i].DefenseStrength,
                        StableValue = livingAreaDatas[i].StableValue,
                        Renown = livingAreaDatas[i].StableValue
                    });

                    //entityManager.AddComponent(livingArea, ComponentType.Create<Position>());
                    //entityManager.SetComponentData(livingArea,new Position
                    //{
                    //    Value = new float3(livingAreaDatas[i].PositionX, livingAreaDatas[i].PositionY, livingAreaDatas[i].PositionZ)
                    //});

                    //entityManager.AddSharedComponentData(livingArea, LivingAreaLook);
                }

                LivingAreaSystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());
            }
            #endregion

            #region BiologicalInit
            {
                List<BiologicalData> data = SqlData.GetWhereDatas<BiologicalData>(" IsDebut=? ", new object[] { 1 });

                for (int i = 0; i < data.Count; i++)
                {
                    var go = GameObject.Instantiate(Settings.Biological, new Vector3(1620.703f, 80.7618f, 629.1682f), quaternion.identity);
                    Entity biologicalEntity = go.GetComponent<GameObjectEntity>().Entity;

                    entityManager.AddComponent(biologicalEntity, ComponentType.Create<Biological>());
                    entityManager.SetComponentData(biologicalEntity, new Biological
                    {
                        BiologicalId = data[i].Id,
                        RaceId = data[i].RaceType,
                        SexId = data[i].Sex,
                        Age = data[i].Age,
                        AgeMax = data[i].AgeMax,
                        Prestige = data[i].Prestige,
                        Influence = data[i].Influence,
                        Disposition = data[i].Disposition,
                        GenGu = data[i].Property1,
                        LingMin = data[i].Property2,
                        DongCha = data[i].Property3,
                        JiYi = data[i].Property4,
                        WuXing = data[i].Property5,
                        YunQi = data[i].Property6
                    });

                    // entityManager.AddSharedComponentData(biologicalEntity,BiologicalLook);

                    //  float3 position = new float3(10f, 10f, 5.1f);
                    //  entityManager.SetComponentData(biologicalEntity, new Position { Value = position });
                    //Transform t = entityManager.GetComponentData<>()
                    //t.position = Vector3.zero;
                    //Debug.Log(t.position);

                }

                //biologicalEntity.en
            }
            #endregion

            #region Player
            {

                var go = GameObject.Instantiate(Settings.PlayerBiological,new Vector3(1620.703f, 80.7618f, 629.1682f),quaternion.identity);
                var player = go.GetComponent<GameObjectEntity>().Entity;

                BiologicalData data = SqlData.GetDataId<BiologicalData>(settings.PlayerId);
                entityManager.AddComponent(player,ComponentType.Create<Biological>());
                entityManager.SetComponentData(player, new Biological
                {
                    BiologicalId = data.Id,
                    RaceId = data.RaceType,
                    SexId = data.Sex,
                    Age = data.Age,
                    AgeMax = data.AgeMax,
                    Prestige = data.Prestige,
                    Influence = data.Influence,
                    Disposition = data.Disposition,
                    GenGu = data.Property1,
                    LingMin = data.Property2,
                    DongCha = data.Property3,
                    JiYi = data.Property4,
                    WuXing = data.Property5,
                    YunQi = data.Property6
                });

                //entityManager.AddComponent(player, ComponentType.Create<Position>());
                //entityManager.SetComponentData(player, new Position { Value = new float3(1620.703f, 80.7618f, 629.1682f) });

                entityManager.AddComponent(player,ComponentType.Create<PlayerInput>());

                entityManager.AddComponent(player,ComponentType.Create<Player>());

                // Finally we add a shared component which dictates the rendered look
                //entityManager.AddSharedComponentData(player, PlayerLook);

                 UICenterMasterManager.Instance.ShowWindow(WindowID.StrategyWindow);

                //PlayerControlSystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());
                // PlayerControlSystem.SetupPlayerView(World.Active.GetOrCreateManager<EntityManager>());
            }
            #endregion

           
            



            var sceneSwitcher = GameObject.Find("SceneSwitcher");
            if (sceneSwitcher != null)
            {
                NewGame();
            }
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            InitializeWithScene();
        }

        private static MeshInstanceRenderer GetLookFromPrototype(string protoName)
        {
            var proto = GameObject.Find(protoName);
            var result = proto.GetComponent<MeshInstanceRendererComponent>().Value;
            Object.Destroy(proto);
            return result;
        }

    }

}



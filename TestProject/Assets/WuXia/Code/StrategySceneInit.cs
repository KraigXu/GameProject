using System.Collections.Generic;
using DataAccessObject;
using TinyFrameWork;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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

            DistrictArchetype = entityManager.CreateArchetype(typeof(District), typeof(LivingArea));
            // Create player archetype typeof(Position), typeof(Rotation), typeof(PlayerInput),typeof(Health)
            PlayerArchetype = entityManager.CreateArchetype(typeof(Biological), typeof(PlayerInput), typeof(Position), typeof(Rotation), typeof(NavMeshAgent), typeof(Player));
            BiologicalArchetype = entityManager.CreateArchetype(typeof(Biological));
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
                // Entity district = entityManager.CreateEntity(DistrictArchetype);
                List<DistrictData> districtDatas = SqlData.GetAllDatas<DistrictData>();
                District[] districtCom = GameObject.Find("StrategyManager").GetComponentsInChildren<District>();
                for (int i = 0; i < districtDatas.Count; i++)
                {
                    for (int j = 0; j < districtCom.Length; j++)
                    {
                        if (districtDatas[i].Id == districtCom[j].Id)
                        {
                            districtCom[j].Name = districtDatas[i].Name;

                            districtDatas.Remove(districtDatas[i]);
                            continue;
                        }
                    }
                }
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

            #region districtInit
            {
                // Entity district = entityManager.CreateEntity(DistrictArchetype);
                List<DistrictData> districtDatas = SqlData.GetAllDatas<DistrictData>();
                District[] districtCom = GameObject.Find("StrategyManager").GetComponentsInChildren<District>();
                for (int i = 0; i < districtDatas.Count; i++)
                {
                    for (int j = 0; j < districtCom.Length; j++)
                    {
                        if (districtDatas[i].Id == districtCom[j].Id)
                        {
                            districtCom[j].Name = districtDatas[i].Name;

                            districtDatas.Remove(districtDatas[i]);
                            continue;
                        }
                    }
                }
                DistrictSystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());
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
                LivingAreaSystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());
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
                Entity player = entityManager.CreateEntity(PlayerArchetype);
                BiologicalData data = SqlData.GetDataId<BiologicalData>(settings.PlayerId);
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

                entityManager.SetComponentData(player, new Position { Value = new float3(1620.703f, 80.7618f, 629.1682f) });

                // Finally we add a shared component which dictates the rendered look
                entityManager.AddSharedComponentData(player, PlayerLook);

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



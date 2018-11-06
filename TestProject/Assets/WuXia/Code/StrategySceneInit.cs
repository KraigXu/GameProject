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
        public static MeshInstanceRenderer PlayerShotLook;
        public static MeshInstanceRenderer EnemyShotLook;
        public static MeshInstanceRenderer EnemyLook;

        public static DemoSetting Settings;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            // This method creates archetypes for entities we will spawn frequently in this game.
            // Archetypes are optional but can speed up entity spawning substantially.

            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            DistrictArchetype = entityManager.CreateArchetype(typeof(District), typeof(LivingArea));
            // Create player archetype
            PlayerArchetype = entityManager.CreateArchetype(typeof(Biological), typeof(Rotation), typeof(PlayerInput));
            BiologicalArchetype=entityManager.CreateArchetype(typeof(Biological),typeof(Rigidbody),typeof(Transform),typeof(CapsuleCollider),typeof(NavMeshAgent));

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
                Entity biologicalEntity = entityManager.CreateEntity(BiologicalArchetype);

                //biologicalEntity.en
            }
            #endregion

            #region Player
            {
                //根据玩家原型创建实体。 它将被默认构造
                //我们列出的所有组件类型的默认值。
                Entity player = entityManager.CreateEntity(PlayerArchetype);
                //我们可以调整一些组件，使其更有意义。
                // We can tweak a few components to make more sense like this.
                entityManager.SetComponentData(player, new Position { Value = new float3(0.0f, 0.0f, 0.0f) });
                entityManager.SetComponentData(player, new Rotation { Value = quaternion.identity });
                //entityManager.SetComponentData(player, new Health { Value = Settings.playerInitialHealth });

                // Finally we add a shared component which dictates the rendered look
                entityManager.AddSharedComponentData(player, PlayerLook);
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
            //PlayerShotLook = GetLookFromPrototype("PlayerShotRenderPrototype");
            //EnemyShotLook = GetLookFromPrototype("EnemyShotRenderPrototype");
            //EnemyLook = GetLookFromPrototype("EnemyRenderPrototype");
            //EnemySpawnSystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());

            //StrategySystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());
            DistrictSystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());
            LivingAreaSystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());
            //LivingAreaBuildingSystem     --暂无开启

            BiologicalSystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());

            PlayerControlSystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());
            PlayerControlSystem.SetupPlayerView(World.Active.GetOrCreateManager<EntityManager>());

            // World.Active.GetOrCreateManager<UpdatePlayerHUD>().SetupGameObjects();
            //World.Active.GetOrCreateManager<StrategySystem>().Update();

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



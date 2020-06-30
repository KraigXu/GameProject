
using System.IO;
using Unity.Entities;
using UnityEngine;
using Unity.Transforms;

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


        public int PlayerId;
        public int TeamId;

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

            PlayerId = 19;
            TeamId = 2;
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

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitializeWithScene()
        {

            //DistrictArchetype = SystemManager.ActiveManager.CreateArchetype(typeof(District));
            //LivingAreaArchetype = SystemManager.ActiveManager.CreateArchetype(typeof(Position), typeof(Rotation), typeof(LivingArea));
            //TechniquesArchetype = SystemManager.ActiveManager.CreateArchetype(typeof(Techniques));
            //RelationArchetype = SystemManager.ActiveManager.CreateArchetype(typeof(Relation));
           
            //EventInfotype = SystemManager.ActiveManager.CreateArchetype(typeof(EventInfo));
            //BiologicalSocialArchetype = SystemManager.ActiveManager.CreateArchetype(typeof(BiologicalSocial));
            //AncientTombArchetype = SystemManager.ActiveManager.CreateArchetype(typeof(Position), typeof(Rotation));
            //ArticleArchetype = SystemManager.ActiveManager.CreateArchetype(typeof(ArticleItem));
            //FactionArchetype = SystemManager.ActiveManager.CreateArchetype(typeof(Faction));


            //BiologicalSystem.SetupComponentData(SystemManager.ActiveManager);
            //FamilySystem.SetupComponentData(SystemManager.ActiveManager);
            //TeamSystem.SetupComponentData(SystemManager.ActiveManager);
            //CurOpeningInfo= new OpeningInfo();
            //CurOpeningInfo.TestValue();

            //Debuger.EnableLog = true;
        }

        //private static MeshInstanceRenderer GetLookFromPrototype(string protoName)
        //{
        //    var proto = GameObject.Find(protoName);
        //    var result = proto.GetComponent<MeshInstanceRendererComponent>().Value;
        //    UnityEngine.Object.Destroy(proto);
        //    return result;
        //}

        public static void Load(string sceneName)
        {

          //  World.DisposeAllWorlds();
            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("SceneSwitcher/LoadCanvas"));
            SceneSwitcher ss = go.GetComponent<SceneSwitcher>();
            ss.SceneNameNext = sceneName;

        }



    }

}



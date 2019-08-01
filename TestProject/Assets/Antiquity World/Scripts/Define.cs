using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Entities;
using UnityEngine;

public delegate void ActionDefault();


public delegate void GameObjectCollider(GameObject go, Collider other);
public delegate void EntityCallBack(Entity entity, int id);
public delegate void EntityGameObjectCallBack(Entity entity,GameObject go, Collision collision);
public delegate int[] SocialDialogEvent(int resoult, int a, int b);
public delegate void SingleParameterEvent(int key);
public delegate void OnBuildingClick(Entity buildingEntity);
public delegate void EntitySingleEvent(Entity a, Entity b);
public delegate void BuildingEvent(Entity entity, int id);



/// <summary>
/// 记录整个程序运行中所有的确定 和流动数据，不应该被删除
/// </summary>
public sealed class Define
{
    public static readonly string TaskSceneName = "Start";
    public static readonly string FightingSceneName = "Demo";
    public static readonly string ManagerSceneName = "Manager";
    public static readonly string LoadingSceneName = "Loading";

    /// <summary>
    /// 当前游戏状态             -1:发生错误  0:正常界面 1正常战略 2正常战役
    /// </summary>
    public static byte GameStatus = 0;
    public static int PlayerId = 1;
    public static string CurrentSceneName = "";
    public static string NextSceneName = "";
    public static int CurStatus = 3;
    public static int PlayerBiologicalId = 1;
    public static float LoadingValue = 0;


    //----->按键
    public static KeyCode CofirmCode = KeyCode.Mouse0;
    public static KeyCode CancelCode = KeyCode.Mouse1;
    public static KeyCode GameRoationLeft = KeyCode.Z;
    public static KeyCode GameRoationRight = KeyCode.X;
    public static KeyCode ItemSlot1 = KeyCode.Alpha1;
    public static KeyCode ItemSlot2 = KeyCode.Alpha2;
    public static KeyCode ItemSlot3 = KeyCode.Alpha3;
    public static KeyCode ItemSlot4 = KeyCode.Alpha4;
    public static KeyCode ItemSlot5 = KeyCode.Alpha5;
    public static KeyCode ItemSlot6 = KeyCode.Alpha6;
    public static KeyCode ItemSlot7 = KeyCode.Alpha7;
    public static KeyCode ItemSlot8 = KeyCode.Alpha8;
    public static KeyCode CommandSkill1 = KeyCode.Q;
    public static KeyCode CommandSkill2 = KeyCode.W;
    public static KeyCode CommandSkill3 = KeyCode.E;
    public static KeyCode Intelligent = KeyCode.LeftShift;
    public static KeyCode LevelMap = KeyCode.M;
    public static KeyCode SkillSlot1 = KeyCode.Alpha1;
    public static KeyCode SkillSlot2 = KeyCode.Alpha2;
    public static KeyCode SkillSlot3 = KeyCode.Alpha3;
    public static KeyCode SkillSlot4 = KeyCode.Alpha4;
    public static KeyCode SkillSlot5 = KeyCode.Alpha5;
    public static KeyCode SkillSlot6 = KeyCode.Alpha6;
    public static KeyCode SkillSlot7 = KeyCode.Alpha7;
    public static KeyCode SkillSlot8 = KeyCode.Alpha8;


    public static readonly string TagTerrain = "Terrain";

    public static readonly string TagLivingArea = "LivingArea";

    public static readonly string TagBiological = "Biological";

    public static readonly string GeneratedPool = "GeneratedPool";
    public static readonly string ParticlePool = "ParticlePool";

    public static bool IsEdit;


   // public const 




    /// <summary>
    /// 分辨率适配
    /// </summary>
    public static List<Vector2> ImageResolutionDef = new List<Vector2>()
    {
        new Vector2(1024, 768),
        new Vector2(1152, 864),
        new Vector2(1280, 1024),
        new Vector2(1400, 1050),
        new Vector2(1600, 1200),
        new Vector2(2048, 1536)
    };





    /// <summary>
    /// 在场景加载之前初始化数据
    /// 此方法为我们将在此游戏中频繁生成的实体创建原型。
    ///Archetypes是可选的，但可以大大加速实体产生。
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        Debug.Log("场景进入");
        SQLService.GetInstance("TD.db");
        GameObject.Instantiate(Resources.Load<GameObject>("GameRuntimeEdit"));
    }

    /// <summary>
    /// 在场景加载后初始化数据
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitializeAfterSceneLoad()
    {


    }



    public static bool IsAPointInACamera(Camera camera ,Vector3 position,bool isNeedModelBlockOut=false)
    {
        // 是否在视野内
        bool result1 = false;
        Vector3 posViewport = camera.WorldToViewportPoint(position);
        Rect rect = new Rect(0, 0, 1, 1);
        result1 = rect.Contains(posViewport);
        // 是否在远近平面内
        bool result2 = false;
        if (posViewport.z >= camera.nearClipPlane && posViewport.z <= camera.farClipPlane)
        {
            result2 = true;
        }
        // 综合判断
        bool result = result1 && result2;
        if (isNeedModelBlockOut)
        {
            Vector3 dir = position - camera.transform.position;
            RaycastHit[] hits;
            bool isBlock = false;
            hits = Physics.RaycastAll(position, -dir);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                Debug.DrawLine(position, hit.point, Color.black);
                if (hit.transform.tag == "Terrain")
                {
                    isBlock = true;
                    break;
                }
            }
            return result && !isBlock;
        }
        else
            return result;

    }

    public static string[] SkillDataSplit(string value)
    {
        return value.Split(',');
    }

    

    //-------------------------Fighting
    //public static int

  
}
public static class TransExpV2<TIn, TOut>
{

    private static readonly Func<TIn, TOut> cache = GetFunc();
    private static Func<TIn, TOut> GetFunc()
    {
        ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
        List<MemberBinding> memberBindingList = new List<MemberBinding>();

        foreach (var item in typeof(TOut).GetProperties())
        {
            if (!item.CanWrite)
                continue;

            MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
            MemberBinding memberBinding = Expression.Bind(item, property);
            memberBindingList.Add(memberBinding);
        }

        MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
        Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[] { parameterExpression });

        return lambda.Compile();
    }

    public static TOut Trans(TIn tIn)
    {
        return cache(tIn);
    }

}